-- Drop existing tables if re-running
DROP TABLE IF EXISTS room_master;
DROP TABLE IF EXISTS room_type_master;

-- ========================
-- 1️⃣ Create room_type_master
-- ========================
CREATE TABLE room_type_master (
    type_id INT AUTO_INCREMENT PRIMARY KEY,
    type_name VARCHAR(50) NOT NULL UNIQUE
);

-- ========================
-- 2️⃣ Insert sample room types
-- ========================
INSERT INTO room_type_master (type_name)
VALUES 
('Single Deluxe'),
('ICCU'),
('General Ward'),
('Semi Deluxe'),
('Suite'),
('Dormitory');

-- ========================
-- 3️⃣ Create room_master
-- ========================
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

-- ========================
-- 4️⃣ Insert sample room data
-- ========================
INSERT INTO room_master 
(room_no, floor_no, type_id, no_of_beds, rent_per_day, rent_per_hour, 
 nursing_charge_per_day, remarks, charge_description, amount_per_day, is_active)
VALUES
('101', '1', 1, 1, 2500.00, 200.00, 300.00, 'Sea view single room', 'AC Charge', 150.00, TRUE),
('202', '2', 2, 2, 5000.00, 400.00, 500.00, 'ICCU bed with monitoring', 'Oxygen Charge', 200.00, TRUE),
('303', '3', 6, 8, 800.00, 80.00, 100.00, 'General ward shared room', 'Bed Charge', 50.00, TRUE);

-- ========================
-- 5️⃣ Stored Procedures
-- ========================

DELIMITER $$

-- Get all rooms
CREATE PROCEDURE GetRoomMasterList()
BEGIN
    SELECT 
        r.room_id AS RoomId,
        r.room_no AS RoomNo,
        r.floor_no AS FloorNo,
        r.type_id AS TypeId,
        r.no_of_beds AS NoOfBeds,
        r.rent_per_day AS RentPerDay,
        r.rent_per_hour AS RentPerHour,
        r.nursing_charge_per_day AS NursingChargePerDay,
        r.remarks AS Remarks,
        r.charge_description AS ChargeDescription,
        r.amount_per_day AS AmountPerDay,
        r.is_active AS IsActive,
        t.type_name AS RoomTypeName
   
    FROM room_master r
    LEFT JOIN room_type_master t ON r.type_id = t.type_id
    ORDER BY r.room_no;
END$$

DELIMITER $$
-- Get room by ID
CREATE PROCEDURE GetRoomMasterById(IN p_RoomId INT)
BEGIN
    SELECT 
        r.room_id AS RoomId,
        r.room_no AS RoomNo,
        r.floor_no AS FloorNo,
        r.type_id AS TypeId,
        r.no_of_beds AS NoOfBeds,
        r.rent_per_day AS RentPerDay,
        r.rent_per_hour AS RentPerHour,
        r.nursing_charge_per_day AS NursingChargePerDay,
        r.remarks AS Remarks,
        r.charge_description AS ChargeDescription,
        r.amount_per_day AS AmountPerDay,
        r.is_active AS IsActive,
        t.type_name AS RoomTypeName
    FROM room_master r
    LEFT JOIN room_type_master t ON r.type_id = t.type_id
    WHERE r.room_id = p_RoomId;
END$$


-- Create new room
CREATE PROCEDURE CreateRoomMaster(
    IN p_RoomNo VARCHAR(10),
    IN p_FloorNo VARCHAR(10),
    IN p_TypeId INT,
    IN p_NoOfBeds INT,
    IN p_RentPerDay DECIMAL(10,2),
    IN p_RentPerHour DECIMAL(10,2),
    IN p_NursingChargePerDay DECIMAL(10,2),
    IN p_Remarks VARCHAR(255),
    IN p_ChargeDescription VARCHAR(100),
    IN p_AmountPerDay DECIMAL(10,2),
    IN p_IsActive BOOLEAN
)
BEGIN
    INSERT INTO room_master (
        room_no, floor_no, type_id, no_of_beds, rent_per_day, rent_per_hour,
        nursing_charge_per_day, remarks, charge_description, amount_per_day, is_active
    )
    VALUES (
        p_RoomNo, p_FloorNo, p_TypeId, p_NoOfBeds, p_RentPerDay, p_RentPerHour,
        p_NursingChargePerDay, p_Remarks, p_ChargeDescription, p_AmountPerDay, p_IsActive
    );
END$$


-- Update room
CREATE PROCEDURE UpdateRoomMaster(
    IN p_RoomId INT,
    IN p_RoomNo VARCHAR(10),
    IN p_FloorNo VARCHAR(10),
    IN p_TypeId INT,
    IN p_NoOfBeds INT,
    IN p_RentPerDay DECIMAL(10,2),
    IN p_RentPerHour DECIMAL(10,2),
    IN p_NursingChargePerDay DECIMAL(10,2),
    IN p_Remarks VARCHAR(255),
    IN p_ChargeDescription VARCHAR(100),
    IN p_AmountPerDay DECIMAL(10,2),
    IN p_IsActive BOOLEAN
)
BEGIN
    UPDATE room_master
    SET 
        room_no = p_RoomNo,
        floor_no = p_FloorNo,
        type_id = p_TypeId,
        no_of_beds = p_NoOfBeds,
        rent_per_day = p_RentPerDay,
        rent_per_hour = p_RentPerHour,
        nursing_charge_per_day = p_NursingChargePerDay,
        remarks = p_Remarks,
        charge_description = p_ChargeDescription,
        amount_per_day = p_AmountPerDay,
        is_active = p_IsActive
    WHERE room_id = p_RoomId;
END$$


-- Delete room
CREATE PROCEDURE DeleteRoomMaster(IN p_RoomId INT)
BEGIN
    DELETE FROM room_master WHERE room_id = p_RoomId;
END$$


-- Get room type list (for dropdown)
CREATE PROCEDURE GetRoomTypeMasterList()
BEGIN
    SELECT type_id AS TypeId, type_name AS TypeName
    FROM room_type_master
    ORDER BY type_name;
END$$

DELIMITER ;
