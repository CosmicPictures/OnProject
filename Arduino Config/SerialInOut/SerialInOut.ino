//fan pin
const int fanPin = 4;
//heater pin
const int heaterPin = 6;

//Analog input
//const int analogInPin = A0;

void setup() {
  // initialize serial communications at 9600 bps:
  Serial.begin(9600);

  pinMode(fanPin, OUTPUT);
  pinMode(heaterPin, OUTPUT);
}



void loop() {
  //handleLightSensor();
  handleSerialIn();
  delay(50);
}



String readString;
unsigned long loopStartMillis;

void handleSerialIn() {
  while(Serial.available()) {
    delay(1);  //delay to allow buffer to fill 
    if (Serial.available() > 0) {
      char c = Serial.read();  //gets one byte from serial buffer
      readString += c; //makes the string readString
    }
  }
  
  if (readString.length() > 0) {
    int value = readString.toInt(); 
    switch(value)
    {
      case 0: //turn fan off
        digitalWrite(fanPin, LOW);
      break;
      case 1: // turn fan on
        digitalWrite(fanPin, HIGH);
      break;
      case 2: // turn fan on
        digitalWrite(heaterPin, LOW);
      break;
      case 3: // turn fan on
        digitalWrite(heaterPin, HIGH);
      break;
    }
    
    readString = "";
  }
}


/*
void handleLightSensor() {
  // read the analog in value:
  int sensorValue = analogRead(analogInPin);

  // print the results to the serial monitor. This is read by Unity
  Serial.println(sensorValue);
}
*/

