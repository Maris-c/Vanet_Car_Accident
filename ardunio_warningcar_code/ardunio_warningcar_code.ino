#include <Wire.h>
#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <TinyGPSPlus.h>
#include <NeoSWSerial.h>
#include <SPI.h>
#include <nRF24L01.h>
#include <RF24.h>
#include <ArduinoJson.h>
#include <Arduino.h>
#include <String.h>
#include <stdbool.h>

#define CAR_ID "30F256.58"
#define GMT 7
static const int RXPin = 3, TXPin = 2;
static const uint32_t GPSBaud = 9600;
#define CE_PIN 9
#define CSN_PIN 10

enum Severity {
  L = 0,
  M = 1,
  H = 2
};

TinyGPSPlus gps;
NeoSWSerial ss(RXPin, TXPin);

Adafruit_MPU6050 mpu;

RF24 radio(CE_PIN, CSN_PIN);
static const byte address[6] = "00001";
static char jsonBuffer[32];

static const int trig = 5;
static const int echo = 6;

static sensors_event_t a, g, temp;
static unsigned long duration; 
static int distance; 
static float accel_x, accel_y, accel_z;
static float roll, pitch;
static float lng, lat;
static TinyGPSDate d;
static TinyGPSTime t;
static char dateTime[21] = "";
static int failCount = 0;

static void printGPS();
static void sendData(char* time, float lng, float lat);
static void encodeGPS(unsigned long ms);
static void processDataMPU6050();
static void getDistance();
static Severity isAccident(float roll, float pitch, int distance);
static void getCoordinates();
static bool isBlockingWay(int sev);
static String severityCar(int sev);

void setup() {
  Serial.begin(9600); // Khởi động Serial với baudrate 9600
  ss.begin(GPSBaud); // Khởi động SoftwareSerial
  while (!Serial);
  pinMode(trig, OUTPUT);
  pinMode(echo, INPUT); 

  if (!radio.begin()) { // Kiểm tra kết nối với nRF24L01+
    Serial.println("Lỗi kết nối nRF24L01+!");
    while (1);
  }

  if (!mpu.begin()) { //Kiểm tra kết nối giữa Arduino và MPU6050
    Serial.println("Không tìm thấy MPU6050! Kiểm tra kết nối.");
    while (1);
  }

  // Cấu hình cảm biến
  mpu.setAccelerometerRange(MPU6050_RANGE_2_G);
  mpu.setGyroRange(MPU6050_RANGE_250_DEG);
  mpu.setFilterBandwidth(MPU6050_BAND_21_HZ);

  const byte addressRSU[6] = "00000";
  const byte addressPlubic[6] ="00001";
  // Cấu hình cho nRF24L01+
  radio.setPALevel(RF24_PA_HIGH);
  radio.setDataRate(RF24_250KBPS);
  radio.setChannel(124);
  radio.openWritingPipe(addressRSU);
  radio.openReadingPipe(0, addressPlubic);
  radio.startListening();
}

void loop() {
  getDistance();
  processDataMPU6050();
  delay(100);

  // Phát tín hiệu cảnh báo khi góc nghiêng vượt quá điều kiện
  if (isAccident(roll, pitch, distance) >= 1) {
    getCoordinates();
    delay(100);
    sendData(dateTime, lng, lat);
  }  
  delay(100);
}

static void encodeGPS(unsigned long ms) {
  unsigned long start = millis();
  do 
  {
    while (ss.available())
      gps.encode(ss.read());
  } while (millis() - start < ms);
}

static void processDataMPU6050() {
  mpu.getEvent(&a, &g, &temp);

  // Lấy gia tốc của 3 trục x, y, z
  accel_x = a.acceleration.x;
  accel_y = a.acceleration.y;
  accel_z = a.acceleration.z;

  // Tính toán góc Roll & Pitch
  roll = atan2(accel_y, accel_z) * 180.0 / PI;
  pitch = atan2(-accel_x, sqrt(accel_y * accel_y + accel_z * accel_z)) * 180.0 / PI;
}

static void getDistance() {
  // Lấy dữ liệu từ module khoảng cách
  digitalWrite(trig,0);
  delayMicroseconds(2);
  digitalWrite(trig,1);
  delayMicroseconds(5);
  digitalWrite(trig,0);
  duration = pulseIn(echo,HIGH);
  distance = int(duration/2/29.412);
}

static Severity isAccident(float roll, float pitch, int distance) {
  if (roll > 40 || roll < -40 || pitch > 50 || pitch < -50 || distance <= 1)
    return 2;
  else if (roll > 30 || roll < -30 || pitch > 40 || pitch < -40 || distance < 3)
    return 1;
  else if (roll > 20 || roll < -20 || pitch > 30 || pitch < -30 || distance < 5)
    return 0;
  else
    return -1;
}

static void getCoordinates() {
  // if (gps.location.isValid()) {
    lng = gps.location.lng();
    lat = gps.location.lat();
    d = gps.date;
    t = gps.time;
  // }
  encodeGPS(0);
}

static String severityCar(Severity sev) {
  String label;

  switch (sev) {
    case 0:
        label = "Low";
        break;
    case 1:
        label = "Medium";
        break;
    case 2:
        label = "High";
        break;
    default:
        return "Unknown";
  }

  return label;
}

bool isBlockingWay(Severity sev) {  
  switch (sev) {
    case 0:
        return false;
    case 1:
        return false;
    case 2:
        return true;
    default:
        return false;
  }
  return false;
}

static void sendData(char* time, float lon, float lat) {
  radio.stopListening();
  StaticJsonDocument<64> doc;
  char jsonString[32];
  int day = d.day();
  int month = d.month();
  int year = d.year();
  int hour = t.hour();
  int minute = t.minute();
  int second = t.second();
  Severity acc = isAccident(roll, pitch, distance);
  String sev = severityCar(acc);
  bool block = isBlockingWay(acc);
  sprintf(dateTime, "%02d-%02d-%2dT%02d%02d%02d.000Z", day, month, year, hour + GMT, minute, second);
  Serial.println("ALTER: Accident detection");

  doc["v_id"] = CAR_ID;
  serializeJson(doc, jsonString);
  radio.write(&jsonString, sizeof(jsonString));
  Serial.println(jsonString);
  doc.clear();
  delay(100);

  doc["lat"] = (long int)(lat * 1000000);
  serializeJson(doc, jsonString);
  radio.write(&jsonString, sizeof(jsonString));
  Serial.println(jsonString);
  doc.clear();
  delay(100);

  doc["lon"] = (long int)(lon * 1000000);
  serializeJson(doc, jsonString);
  radio.write(&jsonString, sizeof(jsonString));
  Serial.println(jsonString);
  doc.clear();
  delay(100);

  doc["t"] = dateTime;
  serializeJson(doc, jsonString);
  radio.write(&jsonString, sizeof(jsonString));
  Serial.println(jsonString);   
  doc.clear();
  delay(100);

  doc["Severity"] = sev;
  serializeJson(doc, jsonString);
  radio.write(&jsonString, sizeof(jsonString));
  Serial.println(jsonString);
  doc.clear();
  delay(100);

  doc["IsBlockingWay"] = block;
  serializeJson(doc, jsonString);
  radio.write(&jsonString, sizeof(jsonString));
  Serial.println(jsonString);
  doc.clear();
  delay(100);

  radio.startListening();
  delay(100);
  Serial.println("--------------------------");
}
