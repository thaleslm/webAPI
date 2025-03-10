@echo off
echo Iniciando Zookeeper...
start "" C:\kafka_2.13-3.9.0\bin\windows\zookeeper-server-start.bat C:\kafka_2.13-3.9.0\config\zookeeper.properties
timeout /t 20 /nobreak  REM Tempo maior para garantir que o Zookeeper inicie corretamente

echo Iniciando Kafka...
start "" C:\kafka_2.13-3.9.0\bin\windows\kafka-server-start.bat C:\kafka_2.13-3.9.0\config\server.properties
timeout /t 20 /nobreak  REM Tempo maior para garantir que o Kafka inicie corretamente

echo Iniciando consumidor Kafka para o tópico electrical_measurements...
start "" C:\kafka_2.13-3.9.0\bin\windows\kafka-console-consumer.bat --topic electrical_measurements --from-beginning --bootstrap-server localhost:9092

pause
