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
      while (1);
    }

    radio.openReadingPipe(0, address);
    radio.setPALevel(RF24_PA_LOW);
    radio.setDataRate(RF24_250KBPS);
    radio.setChannel(108); 
    radio.startListening();
}

void loop() {
  if (radio.available()) {
    radio.read(&jsonBuffer, sizeof(jsonBuffer));
    Serial.println(jsonBuffer);
  }
}