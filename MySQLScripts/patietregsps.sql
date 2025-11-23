-- DROP existing SPs if they exist
DROP PROCEDURE IF EXISTS spGetAllPatients;
DROP PROCEDURE IF EXISTS spGetPatientById;
DROP PROCEDURE IF EXISTS spInsertPatient;
DROP PROCEDURE IF EXISTS spInsertPatientEntry;
DROP PROCEDURE IF EXISTS spUpdatePatient;
DROP PROCEDURE IF EXISTS spDeletePatient;


DELIMITER //

-- =========================================================
-- Get all patients
-- =========================================================
CREATE PROCEDURE spGetAllPatients()
BEGIN
    SELECT 
        pm.PatientId,
        pm.PatientName,
        pm.MobileNo,
        pm.Sex,
        pm.UHIDNo
    FROM patients_master pm
    ORDER BY pm.PatientName;
END //

-- =========================================================
-- Get patient by ID (for Edit)
-- =========================================================
CREATE PROCEDURE spGetPatientById(IN p_PatientId INT)
BEGIN
    SELECT 
        pm.*,
        pe.EntryId,
        pe.RegistrationDate,
        pe.RegistrationTime,
        pe.Age,
        pe.ConsultantDoctorId,
        pe.RefDoctorId,
        pe.PaymentTerms,
        pe.RegistrationType,
        pe.CompOrInsOrCamp,
        pe.PatientCondition,
        pe.ReferenceOrPicmeNo
    FROM patients_master pm
    LEFT JOIN patient_entry pe ON pm.PatientId = pe.PatientId
    WHERE pm.PatientId = p_PatientId;
END //

-- =========================================================
-- Insert patient into patients_master
-- =========================================================
CREATE PROCEDURE spInsertPatient(
    IN p_UHIDType VARCHAR(50),
    IN p_UHIDNo VARCHAR(50),
    IN p_PatientTitle VARCHAR(10),
    IN p_PatientName VARCHAR(100),
    IN p_DOB DATE,
    IN p_Sex VARCHAR(10),
    IN p_GuardianTitle VARCHAR(10),
    IN p_Guardian VARCHAR(100),
    IN p_Address TEXT,
    IN p_Place VARCHAR(100),
    IN p_District VARCHAR(100),
    IN p_GSTState VARCHAR(100),
    IN p_Country VARCHAR(100),
    IN p_PinCode VARCHAR(10),
    IN p_PatientAadhar VARCHAR(12),
    IN p_GuardianAadhar VARCHAR(12),
    IN p_MobileNo VARCHAR(15),
    IN p_Email VARCHAR(100),
    IN p_Photo LONGBLOB,
    IN p_Occupation VARCHAR(100),
    IN p_MaritalStatus VARCHAR(50),
    IN p_BloodGroup VARCHAR(10),
    IN p_AllergicTo TEXT,
    IN p_IsActive TINYINT(1),
    IN p_CreatedBy VARCHAR(50),
    OUT p_NewPatientId INT
)
BEGIN
    INSERT INTO patients_master (
        UHIDType, UHIDNo, PatientTitle, PatientName, DOB, Sex, GuardianTitle, Guardian,
        Address, Place, District, GSTState, Country, PinCode, PatientAadhar, GuardianAadhar,
        MobileNo, Email, Photo, Occupation, MaritalStatus, BloodGroup, AllergicTo, IsActive, CreatedBy
    )
    VALUES (
        p_UHIDType, p_UHIDNo, p_PatientTitle, p_PatientName, p_DOB, p_Sex, p_GuardianTitle, p_Guardian,
        p_Address, p_Place, p_District, p_GSTState, p_Country, p_PinCode, p_PatientAadhar, p_GuardianAadhar,
        p_MobileNo, p_Email, p_Photo, p_Occupation, p_MaritalStatus, p_BloodGroup, p_AllergicTo, p_IsActive, p_CreatedBy
    );

    SET p_NewPatientId = LAST_INSERT_ID();
END //

-- =========================================================
-- Insert patient entry into patient_entry
-- =========================================================
CREATE PROCEDURE spInsertPatientEntry(
    IN p_PatientId INT,
    IN p_RegistrationDate DATE,
    IN p_RegistrationTime TIME,
    IN p_Age INT,
    IN p_ConsultantDoctorId INT,
    IN p_RefDoctorId INT,
    IN p_PaymentTerms VARCHAR(100),
    IN p_RegistrationType VARCHAR(50),
    IN p_CompOrInsOrCamp VARCHAR(100),
    IN p_PatientCondition TEXT,
    IN p_ReferenceOrPicmeNo VARCHAR(15),
    IN p_CreatedBy VARCHAR(50)
)
BEGIN
    INSERT INTO patient_entry (
        PatientId, RegistrationDate, RegistrationTime, Age, ConsultantDoctorId, RefDoctorId,
        PaymentTerms, RegistrationType, CompOrInsOrCamp, PatientCondition, ReferenceOrPicmeNo, CreatedBy
    )
    VALUES (
        p_PatientId, p_RegistrationDate, p_RegistrationTime, p_Age, p_ConsultantDoctorId, p_RefDoctorId,
        p_PaymentTerms, p_RegistrationType, p_CompOrInsOrCamp, p_PatientCondition, p_ReferenceOrPicmeNo, p_CreatedBy
    );
END //

-- =========================================================
-- Update patient master record
-- =========================================================
CREATE PROCEDURE spUpdatePatient(
    IN p_PatientId INT,
    IN p_UHIDType VARCHAR(50),
    IN p_UHIDNo VARCHAR(50),
    IN p_PatientTitle VARCHAR(10),
    IN p_PatientName VARCHAR(100),
    IN p_DOB DATE,
    IN p_Sex VARCHAR(10),
    IN p_GuardianTitle VARCHAR(10),
    IN p_Guardian VARCHAR(100),
    IN p_Address TEXT,
    IN p_Place VARCHAR(100),
    IN p_District VARCHAR(100),
    IN p_GSTState VARCHAR(100),
    IN p_Country VARCHAR(100),
    IN p_PinCode VARCHAR(10),
    IN p_PatientAadhar VARCHAR(12),
    IN p_GuardianAadhar VARCHAR(12),
    IN p_MobileNo VARCHAR(15),
    IN p_Email VARCHAR(100),
    IN p_Photo LONGBLOB,
    IN p_Occupation VARCHAR(100),
    IN p_MaritalStatus VARCHAR(50),
    IN p_BloodGroup VARCHAR(10),
    IN p_AllergicTo TEXT,
    IN p_IsActive TINYINT(1),
    IN p_UpdatedBy VARCHAR(50)
)
BEGIN
    UPDATE patients_master
    SET
        UHIDType = p_UHIDType,
        UHIDNo = p_UHIDNo,
        PatientTitle = p_PatientTitle,
        PatientName = p_PatientName,
        DOB = p_DOB,
        Sex = p_Sex,
        GuardianTitle = p_GuardianTitle,
        Guardian = p_Guardian,
        Address = p_Address,
        Place = p_Place,
        District = p_District,
        GSTState = p_GSTState,
        Country = p_Country,
        PinCode = p_PinCode,
        PatientAadhar = p_PatientAadhar,
        GuardianAadhar = p_GuardianAadhar,
        MobileNo = p_MobileNo,
        Email = p_Email,
        Photo = p_Photo,
        Occupation = p_Occupation,
        MaritalStatus = p_MaritalStatus,
        BloodGroup = p_BloodGroup,
        AllergicTo = p_AllergicTo,
        IsActive = p_IsActive,
        UpdatedBy = p_UpdatedBy,
        UpdatedDate = NOW()
    WHERE PatientId = p_PatientId;
END //

DELIMITER ;
DELIMITER //

CREATE PROCEDURE spDeletePatient(IN p_PatientId INT)
BEGIN
    DELETE FROM patients_master WHERE PatientId = p_PatientId;
END //

DELIMITER ;


DROP PROCEDURE IF EXISTS spGetAllPatients;
DELIMITER //

CREATE PROCEDURE spGetAllPatients()
BEGIN
    SELECT 
        pm.PatientId,
        pm.PatientName,
        pm.MobileNo,
        pm.Sex,
        pm.UHIDNo,
        pm.PhotoBase64,
        pm.PhotoFileName
    FROM patients_master pm
    ORDER BY pm.PatientName;
END //

DELIMITER ;

DROP PROCEDURE IF EXISTS spGetPatientById;
DELIMITER //

CREATE PROCEDURE spGetPatientById(IN p_PatientId INT)
BEGIN
    SELECT 
        pm.*,
        pe.EntryId,
        pe.RegistrationDate,
        pe.RegistrationTime,
        pe.Age,
        pe.ConsultantDoctorId,
        pe.RefDoctorId,
        pe.PaymentTerms,
        pe.RegistrationType,
        pe.CompOrInsOrCamp,
        pe.PatientCondition,
        pe.ReferenceOrPicmeNo
    FROM patients_master pm
    LEFT JOIN patient_entry pe ON pm.PatientId = pe.PatientId
    WHERE pm.PatientId = p_PatientId;
END //

DELIMITER ;



DROP PROCEDURE IF EXISTS spGetPatientById;

DELIMITER //

CREATE PROCEDURE spGetPatientById(IN p_PatientId INT)
BEGIN
    SELECT 
        pm.PatientId,
        pm.UHIDType,
        pm.UHIDNo,
        pm.PatientTitle,
        pm.PatientName,
        pm.DOB,
        pm.Sex,
        pm.GuardianTitle,
        pm.Guardian,
        pm.Address,
        pm.Place,
        pm.District,
        pm.GSTState,
        pm.Country,
        pm.PinCode,
        pm.PatientAadhar,
        pm.GuardianAadhar,
        pm.MobileNo,
        pm.Email,
        pm.Photo, -- keep as LONGBLOB
        pm.Occupation,
        pm.MaritalStatus,
        pm.BloodGroup,
        pm.AllergicTo,
        pm.IsActive,
        pm.CreatedBy,
        pm.UpdatedBy,
        pm.CreatedDate,
        pm.UpdatedDate,
        pe.EntryId,
        pe.RegistrationDate,
        pe.RegistrationTime,
        pe.Age,
        pe.ConsultantDoctorId,
        pe.RefDoctorId,
        pe.PaymentTerms,
        pe.RegistrationType,
        pe.CompOrInsOrCamp,
        pe.PatientCondition,
        pe.ReferenceOrPicmeNo
    FROM patients_master pm
    LEFT JOIN patient_entry pe ON pm.PatientId = pe.PatientId
    WHERE pm.PatientId = p_PatientId;
END //

DELIMITER ;
DELIMITER //
drop procedure spInsertPatient

DELIMITER //

CREATE PROCEDURE spInsertPatient(
    IN p_UHIDType VARCHAR(50),
    IN p_UHIDNo VARCHAR(50),
    IN p_PatientTitle VARCHAR(10),
    IN p_PatientName VARCHAR(100),
    IN p_DOB DATE,
    IN p_Sex VARCHAR(10),
    IN p_GuardianTitle VARCHAR(10),
    IN p_Guardian VARCHAR(100),
    IN p_Address TEXT,
    IN p_Place VARCHAR(100),
    IN p_District VARCHAR(100),
    IN p_GSTState VARCHAR(100),
    IN p_Country VARCHAR(100),
    IN p_PinCode VARCHAR(10),
    IN p_PatientAadhar VARCHAR(12),
    IN p_GuardianAadhar VARCHAR(12),
    IN p_MobileNo VARCHAR(15),
    IN p_Email VARCHAR(100),
    IN p_Photo LONGBLOB,
    IN p_PhotoFileName VARCHAR(255),
    IN p_Occupation VARCHAR(100),
    IN p_MaritalStatus VARCHAR(50),
    IN p_BloodGroup VARCHAR(10),
    IN p_AllergicTo TEXT,
    IN p_IsActive TINYINT(1),
    IN p_CreatedBy VARCHAR(50),
    IN p_PhotoBase64 longtext,
    OUT p_NewPatientId INT
)
BEGIN
    INSERT INTO patients_master (
        UHIDType, UHIDNo, PatientTitle, PatientName, DOB, Sex, GuardianTitle, Guardian,
        Address, Place, District, GSTState, Country, PinCode, PatientAadhar, GuardianAadhar,
        MobileNo, Email, Photo, PhotoFileName, PhotoBase64,Occupation, MaritalStatus, BloodGroup, AllergicTo, IsActive, CreatedBy
    )
    VALUES (
        p_UHIDType, p_UHIDNo, p_PatientTitle, p_PatientName, p_DOB, p_Sex, p_GuardianTitle, p_Guardian,
        p_Address, p_Place, p_District, p_GSTState, p_Country, p_PinCode, p_PatientAadhar, p_GuardianAadhar,
        p_MobileNo, p_Email, p_Photo, p_PhotoFileName, p_PhotoBase64, p_Occupation, p_MaritalStatus, p_BloodGroup, p_AllergicTo, p_IsActive, p_CreatedBy
    );

    SET p_NewPatientId = LAST_INSERT_ID();
END //

DELIMITER ;
