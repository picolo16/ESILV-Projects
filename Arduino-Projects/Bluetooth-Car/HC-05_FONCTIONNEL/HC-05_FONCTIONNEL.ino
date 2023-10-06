#define CUSTOM_SETTINGS
#define INCLUDE_GAMEPAD_MODULE
#include <Dabble.h>
#include <SoftwareSerial.h>

SoftwareSerial BTSerial(2, 3);

const int motorGA = 11;
const int motorGR = 10;
const int motorDA = 5 ;
const int motorDR = 6;
const int pwrON = 12;

int increment = 0;
bool enable = false;


void setup()
{
  BTSerial.begin(38400);
  Dabble.begin(38400);      //Enter baudrate of your bluetooth.Connect bluetooth on Bluetooth port present on evive.
  pinMode(motorGA, OUTPUT);
  pinMode(motorGR, OUTPUT);
  pinMode(motorDA, OUTPUT);
  pinMode(motorDR, OUTPUT);
  pinMode(pwrON, OUTPUT);
  digitalWrite(pwrON², LOW);
}

void loop()
{
  Dabble.processInput();             //this function is used to refresh data obtained from smartphone.Hence calling this function is mandatory in order to get data properly from your mobile.
  BTSerial.print("KeyPressed: ");
  if (GamePad.isUpPressed())
  {
    analogWrite(motorDA, 255);
    analogWrite(motorGA, 255);
    analogWrite(motorDR, 0);
    analogWrite(motorGR, 0);
    if (increment < 255)
      increment++;

  }

  if (GamePad.isDownPressed())
  {
    analogWrite(motorDA, 0);
    analogWrite(motorGA, 0);
    analogWrite(motorDR, 255);
    analogWrite(motorGR, 255);
    increment++;
  }

  if (GamePad.isLeftPressed())
  {
    analogWrite(motorDA, 255);
    analogWrite(motorGA, 0);
    analogWrite(motorDR, 0);
    analogWrite(motorGR, 255);

  }

  if (GamePad.isRightPressed())
  {
    analogWrite(motorDA, 0);
    analogWrite(motorGA, 255);
    analogWrite(motorDR, 255);
    analogWrite(motorGR, 0);
  }

  if (GamePad.isSquarePressed())
  {
    if (enable)
      digitalWrite(pwrON, LOW);
    else
      digitalWrite(pwrON, HIGH);
  }

  increment = 0;
  analogWrite(motorDA, increment);
  analogWrite(motorGA, increment);
  analogWrite(motorDR, 0);
  analogWrite(motorGR, 0);
  delay(30);

}
