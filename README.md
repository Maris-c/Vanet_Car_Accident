# 🚗 VANET Car Accident Warning System

A simulation-based project to demonstrate accident detection and warning in Vehicular Ad-hoc Networks (VANET) using IoT devices and embedded systems. This system is designed to enhance vehicle-to-vehicle (V2V) communication for safer driving environments.

## 📌 Project Overview

This project models a smart vehicle network that can detect car accidents and share warning signals with nearby vehicles in real-time. It's built using mini toy cars, embedded hardware components, and wireless modules to simulate the VANET environment.

## 🧠 Key Features

- 🧭 **Accident Detection** using the MPU6050 accelerometer and gyroscope module
- 📡 **Vehicle Communication** using NRF24L01 transceiver modules
- 🖥️ **Real-Time Display** via LCD screen
- 💾 **Data Logging** to a backend or cloud database 
- 🌐 **Web-based Visualization** displays data from vehicles in real-time

## 🛠️ Technologies & Components

| Component       | Description                                 |
|-----------------|---------------------------------------------|
| Arduino Uno     | Microcontroller for sensor and logic control|
| MPU6050         | Detects sudden accelerations or tilts       |
| GPS GY-NEO6MV2  | Garther information of time and location    |
| HC-SR04         | Infrared sensor to detect objects           |
| NRF24L01+       | Handles wireless communication between vehicles|
| LCD (120x160)   | Displays status and warnings                |
| Toy Cars        | Used as simulated vehicles for testing      |
| Web App         | Displays data from vehicles in real-time |

## ⚙️ System Workflow

1. **Accident Detection Phase**:
   - `MPU6050` detects tilt or rollover.
   - `HC-SR04` detects a nearby obstacle or impact.
   - If both are triggered → system confirms an accident.

2. **Alert Broadcasting Phase**:
   - Accident vehicle sends a warning via `NRF24L01` to:
     - Nearby vehicles (V2V)
     - RSU (Road Side Unit) for further actions

3. **Data Reporting Phase**:
   - RSU receives and sends accident data to the web server, including:
     - `timestamp`
     - `vehicle_id`
     - `location (latitude, longitude)`
     - `severity` of accident
     - `isBlockWay` (true/false)

4. **Web Processing Phase**:
   - Web server analyzes accident data.
   - Finds optimal alternate routes and sends them to affected vehicles via RSU.

5. **Route Proposal Phase**:
   - Vehicles receive the suggested route.
   - Each vehicle responds:
     - `ok` → accept new route
     - no response → proposal ignored or rejected

6. **Confirmation Phase**:
   - RSU informs the web server of accepted/rejected routes.
   - Web may send additional route suggestions if necessary.

