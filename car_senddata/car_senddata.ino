#include <SPI.h>
#include <nRF24L01.h>
#include <RF24.h>
#include <Adafruit_GFX.h>
#include <Adafruit_ST7735.h>
#include <SPI.h>

// Khai báo các chân kết nối
#define TFT_CS     6
#define TFT_RST    4
#define TFT_DC     5

#define YES 3
#define NO 2

// Khởi tạo màn hình
Adafruit_ST7735 tft = Adafruit_ST7735(TFT_CS, TFT_DC, TFT_RST);

#define CE_PIN 9
#define CSN_PIN 10

int temp = 0;

RF24 radio(CE_PIN, CSN_PIN);
const byte address[6] = "00001";
static char jsonBuffer[32];
bool responseReceived = false;

void setup() {
  Serial.begin(9600);
  
  if (!radio.begin()) {
    Serial.println("Loi ket noi NRF24L01!");
    while (1);
  }

  // Cấu hình thông số RF24
  radio.setChannel(124);       // Chọn kênh 100 (2.5GHz)
  radio.setDataRate(RF24_250KBPS);
  radio.setPALevel(RF24_PA_HIGH);
  radio.openWritingPipe(address); // Địa chỉ TX
  
  radio.setAutoAck(true);
  radio.setRetries(15, 15); // delay 15 x 250us, lên đến 15 lần lặp lại

  radio.openReadingPipe(0, 0xF0F0F0F0E1LL);
  radio.startListening();
  
  Serial.println("San sang nhan du lieu...");

  tft.initR(INITR_GREENTAB); // Khởi tạo màn hình (Black tab thường dùng)
  tft.fillScreen(ST77XX_BLACK); // Xóa màn hình
  pinMode(YES, INPUT);
  pinMode(NO, INPUT);

  tft.setTextColor(ST77XX_WHITE);
  tft.setTextSize(2);
  tft.setCursor(0, 1);
  tft.print("Hi Maris!");
}

char buffer[32] = "";

void loop() {
  if (!responseReceived) {
    if (radio.available()) {
      radio.read(&jsonBuffer, 32);
      if (jsonBuffer == nullptr || jsonBuffer[0] == '\0') {
        Serial.println("Du lieu rong");
      }
      else {
      Serial.println(jsonBuffer);
      }

      tft.fillScreen(ST77XX_BLACK); // Xóa màn hình
      tft.setTextColor(ST77XX_WHITE);
      tft.setTextSize(2);
      tft.setCursor(0, 0);
      tft.print(jsonBuffer);

      radio.read(&jsonBuffer, 32);
      Serial.println(jsonBuffer);

      tft.setCursor(0, 50);
      tft.print(jsonBuffer);

      while (!temp) {
        if (digitalRead(YES) == HIGH) {
          tft.fillScreen(ST77XX_BLACK); // Xóa màn hình
          tft.setTextColor(ST77XX_WHITE);
          tft.setTextSize(2);
          tft.setCursor(0, 1);
      
          tft.print("Yes");
          temp++;

          radio.stopListening();
          sprintf(buffer, "Yes");
          delay(100);
          tft.setCursor(0, 50);
          tft.print("Wait a minute!");
          if (!radio.write(&buffer, sizeof(buffer))) {
            Serial.println(buffer);
          } else {
            Serial.println("Error!");
          }

          delay(200);
          
          radio.startListening();
          if (radio.available()) {
            radio.read(&jsonBuffer, 32);
            tft.fillScreen(ST77XX_BLACK); // Xóa màn hình
            tft.setTextColor(ST77XX_WHITE);
            tft.setTextSize(2);
            tft.setCursor(0, 1);
            tft.print("Tuyen duong de xuat");
            tft.print(jsonBuffer);
            radio.stopListening();
          }
          delay(5000);
          radio.startListening();
        }
          
        if (digitalRead(NO) == HIGH) {
          tft.fillScreen(ST77XX_BLACK); // Xóa màn hình
          tft.setTextColor(ST77XX_WHITE);
          tft.setTextSize(2);
          tft.setCursor(0, 1);
          tft.print("No");
          temp++;

          radio.stopListening();
          sprintf(buffer, "No");
          if (!radio.write(&buffer, sizeof(buffer))) {
            Serial.println(buffer);
          } else {
            Serial.println("Error!");
          }
          radio.startListening();
          responseReceived = true;  // Đã nhận phản hồi => ngừng gửi tiếp
          delay(5000);
        }
        responseReceived = true;  // Đã nhận phản hồi => ngừng gửi tiếp
      }
    }
      temp = 0;
  }
}