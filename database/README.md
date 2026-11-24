# Chitrashala — MySQL Sensor Database

This folder contains the MySQL database schema, imported sensor records (335 frames at ~48 Hz), and raw sample dataset for **Chitrashala**.

## Contents
- `chitrashala_db.sql` — Full MySQL dump containing the `patients` and `palm_movements` tables plus all 335 imported sensor rows.
- `sample_data/parkinsons_movement_dataset.csv` — Raw 3D hand-tracking dataset (`patient_id = 101`).

## How to Load Into Your Local MySQL (MySQL 8.0+)
1. Open **MySQL Workbench** (File → Run SQL Script → select `database/chitrashala_db.sql` → Start), **or** run in PowerShell:
   ```powershell
   & "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p -e "SOURCE database/chitrashala_db.sql;"