#!/bin/bash 

cd .. & docker compose up -d accident-monitor-db accident-monitor-db.configurator mosquitto ors-app   
