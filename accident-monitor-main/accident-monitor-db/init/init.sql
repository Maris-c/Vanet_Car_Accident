USE [master];
GO
-- Create a new database called 'AccidentMonitorDB'
-- Connect to the 'master' database to run this snippet
-- Create the new database if it does not exist already
IF NOT EXISTS (
    SELECT name
        FROM sys.databases
        WHERE name = N'AccidentMonitorDB'
)
CREATE DATABASE AccidentMonitorDB
GO
