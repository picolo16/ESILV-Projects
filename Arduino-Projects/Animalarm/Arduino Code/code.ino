#define echoPin 2
#define trigPin 3
#define fireLED 9
#define alarmPin 10

long duration;
int distance;
int rdm;
int timer = 0;

void setup() {
  Serial.begin(9600);

  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(fireLED, OUTPUT);
  pinMode(alarmPin, OUTPUT);

  digitalWrite(fireLED, LOW);
  digitalWrite(alarmPin, LOW);
  digitalWrite(trigPin, LOW);
}
void fire()
{
  while (timer < 2)
  {
    for (int i = 0; i < 255; i++)
    {
      rdm = random(0, 256);
      analogWrite(fireLED, rdm);
      analogWrite(alarmPin, i%40);
      delay(20);
    }

    for (int i = 255; i > 0; i--)
    {
      rdm = random(0, 256);
      analogWrite(fireLED, rdm);
      analogWrite(alarmPin, i%40);
      delay(20);
    }
    timer+=1;
  }
  timer = 0;
  digitalWrite(fireLED,LOW);
  digitalWrite(alarmPin,LOW);
  
}
void loop() {
  
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);

  duration = pulseIn(echoPin, HIGH);
  distance = duration * 0.034 / 2;

  if (distance < 10) {
    fire();
  }

  Serial.print("Distance: ");
  Serial.print(distance);
  Serial.println(" cm");
  delay(50);
}
