DROP TABLE IF EXISTS `patient_entry`;
DROP TABLE IF EXISTS `doctor_master`;
DROP TABLE IF EXISTS `patients_master`;

CREATE TABLE `patients_master` (
  `PatientId` INT NOT NULL AUTO_INCREMENT,
  `UHIDType` VARCHAR(50) NOT NULL,
  `UHIDNo` VARCHAR(50) DEFAULT NULL,
  `PatientTitle` VARCHAR(10) NOT NULL,
  `PatientName` VARCHAR(100) NOT NULL,
  `DOB` DATE DEFAULT NULL,
  `Sex` VARCHAR(10) NOT NULL,
  `GuardianTitle` VARCHAR(10) DEFAULT NULL,
  `Guardian` VARCHAR(100) DEFAULT NULL,
  `Address` TEXT NOT NULL,
  `Place` VARCHAR(100) DEFAULT NULL,
  `District` VARCHAR(100) DEFAULT NULL,
  `GSTState` VARCHAR(100) DEFAULT NULL,
  `Country` VARCHAR(100) DEFAULT NULL,
  `PinCode` VARCHAR(10) DEFAULT NULL,
  `PatientAadhar` VARCHAR(12) DEFAULT NULL,
  `GuardianAadhar` VARCHAR(12) DEFAULT NULL,
  `MobileNo` VARCHAR(15) DEFAULT NULL,
  `Email` VARCHAR(100) DEFAULT NULL,
  `Photo` LONGBLOB,
  `Occupation` VARCHAR(100) DEFAULT NULL,
  `MaritalStatus` VARCHAR(50) DEFAULT NULL,
  `BloodGroup` VARCHAR(10) DEFAULT NULL,
  `AllergicTo` TEXT,
  `IsActive` TINYINT(1) DEFAULT 1,
  `CreatedBy` VARCHAR(50) DEFAULT NULL,
  `UpdatedBy` VARCHAR(50) DEFAULT NULL,
  `CreatedDate` DATETIME DEFAULT CURRENT_TIMESTAMP,
  `UpdatedDate` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`PatientId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `doctor_master` (
  `doctor_id` INT NOT NULL AUTO_INCREMENT,
  `doctor_name` VARCHAR(100) NOT NULL,
  `degree` VARCHAR(50) DEFAULT NULL,
  `address` TEXT,
  `place` VARCHAR(100) DEFAULT NULL,
  `type` VARCHAR(50) DEFAULT NULL,
  `phone_no` VARCHAR(20) DEFAULT NULL,
  `mobile_no` VARCHAR(20) DEFAULT NULL,
  `register_no` VARCHAR(50) DEFAULT NULL,
  `section` VARCHAR(50) DEFAULT NULL,
  `email` VARCHAR(100) DEFAULT NULL,
  `dob` DATE DEFAULT NULL,
  `dom` DATE DEFAULT NULL,
  `reg_amount` DECIMAL(10,2) DEFAULT NULL,
  `second_visit_amt` DECIMAL(10,2) DEFAULT NULL,
  `second_visit_upto_days` INT DEFAULT NULL,
  `regular_fees` DECIMAL(10,2) DEFAULT NULL,
  `active` TINYINT(1) DEFAULT 1,
  PRIMARY KEY (`doctor_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `patient_entry` (
  `EntryId` INT NOT NULL AUTO_INCREMENT,
  `PatientId` INT NOT NULL,
  `RegistrationDate` DATE NOT NULL,
  `RegistrationTime` TIME NOT NULL,
  `Age` INT NOT NULL,
  `ConsultantDoctorId` INT NOT NULL,
  `RefDoctorId` INT DEFAULT NULL,
  `PaymentTerms` VARCHAR(100) DEFAULT NULL,
  `RegistrationType` VARCHAR(50) DEFAULT NULL,
  `CompOrInsOrCamp` VARCHAR(100) DEFAULT NULL,
  `PatientCondition` TEXT,
  `ReferenceOrPicmeNo` VARCHAR(15) DEFAULT NULL,
  `CreatedBy` VARCHAR(50) DEFAULT NULL,
  `UpdatedBy` VARCHAR(50) DEFAULT NULL,
  `CreatedDate` DATETIME DEFAULT CURRENT_TIMESTAMP,
  `UpdatedDate` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`EntryId`),
  FOREIGN KEY (`PatientId`) REFERENCES `patients_master`(`PatientId`) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (`ConsultantDoctorId`) REFERENCES `doctor_master`(`doctor_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  FOREIGN KEY (`RefDoctorId`) REFERENCES `doctor_master`(`doctor_id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;




INSERT INTO doctor_master 
(doctor_name, degree, address, place, type, phone_no, mobile_no, register_no, section, email, dob, dom, reg_amount, second_visit_amt, second_visit_upto_days, regular_fees, active)
VALUES
('Dr. Ramesh Kumar', 'MBBS, MD', '12 Gandhi Street', 'Chennai', 'Consultant', '044-2345678', '9876543210', 'TN12345', 'General Medicine', 'ramesh.md@gmail.com', '1975-06-15', NULL, 300.00, 150.00, 7, 500.00, 1),

('Dr. Priya Mani', 'MBBS, DGO', '45 Lake Road', 'Chennai', 'Gynecologist', '044-2211223', '9876501234', 'TN56789', 'Gynecology', 'priya.obg@gmail.com', '1982-02-20', NULL, 350.00, 200.00, 10, 600.00, 1),

('Dr. S. Abdul Rahman', 'MBBS, MS Ortho', '22 Anna Nagar', 'Chennai', 'Orthopedic', '044-2323456', '9876001122', 'TN44556', 'Orthopedics', 'abdul.ortho@gmail.com', '1978-11-11', NULL, 400.00, 250.00, 5, 700.00, 1),

('Dr. Meenakshi Narayan', 'MBBS, DCH', '88 Park View', 'Chennai', 'Pediatrician', '044-2667788', '9876123456', 'TN88990', 'Pediatrics', 'meena.ped@gmail.com', '1985-08-18', NULL, 280.00, 150.00, 6, 450.00, 1),

('Dr. Aravind Ravi', 'MBBS, DM Cardio', '77 MG Road', 'Chennai', 'Cardiologist', '044-2998877', '9876332211', 'TN99887', 'Cardiology', 'aravind.cardio@gmail.com', '1974-09-02', NULL, 500.00, 300.00, 7, 900.00, 1);
