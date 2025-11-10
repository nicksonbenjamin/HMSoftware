CREATE TABLE room_master (
    room_id INT AUTO_INCREMENT PRIMARY KEY,
    room_no VARCHAR(10) NOT NULL,
    floor_no VARCHAR(10),
    type_id INT,
    no_of_beds INT,
    rent_per_day DECIMAL(10,2),
    rent_per_hour DECIMAL(10,2),
    nursing_charge_per_day DECIMAL(10,2),
    remarks VARCHAR(255),
    charge_description VARCHAR(100),
    amount_per_day DECIMAL(10,2),
    is_active BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (type_id) REFERENCES room_type_master(type_id)
);
INSERT INTO room_master 
(room_no, floor_no, type_id, no_of_beds, rent_per_day, rent_per_hour, 
 nursing_charge_per_day, remarks, charge_description, amount_per_day, is_active)
VALUES
('101', '1', 1, 1, 2500.00, 200.00, 300.00, 'Sea view single room', 'AC Charge', 150.00, TRUE),
('202', '2', 2, 2, 5000.00, 400.00, 500.00, 'ICCU bed with monitoring', 'Oxygen Charge', 200.00, TRUE),
('303', '3', 6, 8, 800.00, 80.00, 100.00, 'General ward shared room', 'Bed Charge', 50.00, TRUE);
