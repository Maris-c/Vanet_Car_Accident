#include <SPI.h>
#include <nRF24L01.h>
#include <RF24.h>

#define CE_PIN 4
#define CSN_PIN 15

RF24 radio(CE_PIN, CSN_PIN);
const byte address[6] = "00001";
static char jsonBuffer[32];
void setup() {
  Serial.begin(9600);
  if (!radio.begin()) {
    Serial.println("Lỗi nRF24L01+!");
  }

  radio.setChannel(124);       // Chọn kênh 100 (2.5GHz)
  radio.setDataRate(RF24_250KBPS);
  radio.setPALevel(RF24_PA_HIGH);
  radio.openWritingPipe(0xF0F0F0F0E1LL); // Địa chỉ TX
  
  radio.setAutoAck(true);
  radio.setRetries(15, 15); // delay 15 x 250us, lên đến 15 lần lặp lại

  radio.openReadingPipe(0, address);
  radio.startListening();
  Serial.println("ready...");
}

char mess[16] = "Accident ahead!";
char instruct[32] = "Find another way?";
char buffer[32] = "";
bool responseReceived = false;

void loop() {
if (!responseReceived) {
    radio.stopListening();

    if (!radio.write(&mess, 16, true)) {
      Serial.println(mess);
    } else {
      Serial.println("Error sending mess!");
    }

    delay(10);

    if (!radio.write(&instruct, 32, true)) {
      Serial.println(instruct);
    } else {
      Serial.println("Error sending instruct!");
    }

    radio.startListening();
    delay(1000); // chờ phản hồi

    if (radio.available()) {
      radio.read(&buffer, sizeof(buffer));
      Serial.println(buffer);
      responseReceived = true;  // Đã nhận phản hồi => ngừng gửi tiếp
    } else {
      delay(5000); // chờ trước khi thử lại
      Serial.println("No response. Will retry...");
    }
  }
}