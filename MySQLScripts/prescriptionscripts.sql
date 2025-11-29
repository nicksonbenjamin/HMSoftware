/* ------------------------------------------------
   TABLE: DrPrescriptionDetails
------------------------------------------------ */
CREATE TABLE IF NOT EXISTS DrPrescriptionDetails (
    DrPrescriptionDetailsId INT AUTO_INCREMENT PRIMARY KEY,
    DrPrescriptionId INT,
    ProductId INT,
    DosePatternId INT,
    DosePattern VARCHAR(20),
    DoseUsage VARCHAR(20),
    DoseDays INT,
    Qty INT
);

/* ------------------------------------------------
   DROP ALL EXISTING PROCEDURES
------------------------------------------------ */
DROP PROCEDURE IF EXISTS spGetAllPatients;
DROP PROCEDURE IF EXISTS spGetPatientById;
DROP PROCEDURE IF EXISTS spInsertPatient;
DROP PROCEDURE IF EXISTS spInsertPatientEntry;
DROP PROCEDURE IF EXISTS spUpdatePatient;
DROP PROCEDURE IF EXISTS spDeletePatient;
DROP PROCEDURE IF EXISTS sp_GetPatients;
DROP PROCEDURE IF EXISTS sp_GetPatientEntry;
DROP PROCEDURE IF EXISTS sp_GetMedicines;
DROP PROCEDURE IF EXISTS sp_SavePrescription;
DROP PROCEDURE IF EXISTS sp_SavePrescriptionMedicine;
DROP PROCEDURE IF EXISTS sp_GetPrescriptionList;
DROP PROCEDURE IF EXISTS sp_SavePrescriptionClinical;

DELIMITER //

/* ------------------------------------------------
   Get All Patients
------------------------------------------------ */
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

/* ------------------------------------------------
   Get Patient By ID
------------------------------------------------ */
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

/* ------------------------------------------------
   Insert Patient
------------------------------------------------ */
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
    IN p_PhotoBase64 LONGTEXT,
    OUT p_NewPatientId INT
)
BEGIN
    INSERT INTO patients_master (
        UHIDType, UHIDNo, PatientTitle, PatientName, DOB, Sex,
        GuardianTitle, Guardian, Address, Place, District, GSTState, Country,
        PinCode, PatientAadhar, GuardianAadhar, MobileNo, Email,
        Photo, PhotoFileName, PhotoBase64,
        Occupation, MaritalStatus, BloodGroup, AllergicTo, IsActive, CreatedBy
    )
    VALUES (
        p_UHIDType, p_UHIDNo, p_PatientTitle, p_PatientName, p_DOB, p_Sex,
        p_GuardianTitle, p_Guardian, p_Address, p_Place, p_District, p_GSTState, p_Country,
        p_PinCode, p_PatientAadhar, p_GuardianAadhar, p_MobileNo, p_Email,
        p_Photo, p_PhotoFileName, p_PhotoBase64,
        p_Occupation, p_MaritalStatus, p_BloodGroup, p_AllergicTo, p_IsActive, p_CreatedBy
    );

    SET p_NewPatientId = LAST_INSERT_ID();
END //

/* ------------------------------------------------
   Insert Patient Entry
------------------------------------------------ */
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
        PatientId, RegistrationDate, RegistrationTime, Age,
        ConsultantDoctorId, RefDoctorId, PaymentTerms,
        RegistrationType, CompOrInsOrCamp, PatientCondition,
        ReferenceOrPicmeNo, CreatedBy
    )
    VALUES (
        p_PatientId, p_RegistrationDate, p_RegistrationTime, p_Age,
        p_ConsultantDoctorId, p_RefDoctorId, p_PaymentTerms,
        p_RegistrationType, p_CompOrInsOrCamp, p_PatientCondition,
        p_ReferenceOrPicmeNo, p_CreatedBy
    );
END //

/* ------------------------------------------------
   Update Patient
------------------------------------------------ */
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

/* ------------------------------------------------
   Delete Patient
------------------------------------------------ */
CREATE PROCEDURE spDeletePatient(IN p_PatientId INT)
BEGIN
    DELETE FROM patients_master WHERE PatientId = p_PatientId;
END //

/* ------------------------------------------------
   Search Patients
------------------------------------------------ */
CREATE PROCEDURE sp_GetPatients(IN searchTxt VARCHAR(50))
BEGIN
    SELECT 
        PatientId, PatientName, UHIDNo, Address, MobileNo, Sex, BloodGroup
    FROM patients_master
    WHERE PatientName LIKE CONCAT('%', searchTxt, '%')
       OR UHIDNo LIKE CONCAT('%', searchTxt, '%')
    LIMIT 50;
END //

/* ------------------------------------------------
   Get Latest Patient Entry
------------------------------------------------ */
CREATE PROCEDURE sp_GetPatientEntry(IN pid INT)
BEGIN
    SELECT *
    FROM patient_entry
    WHERE PatientId = pid
    ORDER BY EntryId DESC
    LIMIT 1;
END //

/* ------------------------------------------------
   Get Active Medicines
------------------------------------------------ */
CREATE PROCEDURE sp_GetMedicines()
BEGIN
    SELECT ProductCode, ProductName
    FROM productmaster
    WHERE IsActive = 1;
END //

/* ------------------------------------------------
   Save Prescription
------------------------------------------------ */
CREATE PROCEDURE sp_SavePrescription(
    IN pPatientId INT,
    IN pEntryType VARCHAR(10),
    IN pEntryNumber INT,
    IN pEntryPeriod VARCHAR(15),
    IN pDeseaseId INT,
    IN pHeight DECIMAL(10,2),
    IN pWeight DECIMAL(10,2),
    IN pBMI DECIMAL(10,2),
    IN pTemp DECIMAL(10,2),
    IN pBP VARCHAR(10),
    IN pSPO2 VARCHAR(10),
    IN pPulse DECIMAL(10,2),
    IN pNextVisit DATETIME
)
BEGIN
    INSERT INTO DrPrescription (
        PatientId, EntryType, EntryNumber, EntryPeriod,
        DeseaseId, Height, Weight, BMI, TempInCelcius,
        BP, SPO2, PulseRate, PrescriptionDate, NextVisitDate
    )
    VALUES (
        pPatientId, pEntryType, pEntryNumber, pEntryPeriod,
        pDeseaseId, pHeight, pWeight, pBMI, pTemp,
        pBP, pSPO2, pPulse, NOW(), pNextVisit
    );

    SELECT LAST_INSERT_ID() AS NewPrescriptionId;
END //

/* ------------------------------------------------
   Save Prescription Medicine
------------------------------------------------ */
CREATE PROCEDURE sp_SavePrescriptionMedicine(
    IN pDrPrescriptionId INT,
    IN pProductId INT,
    IN pDosePatternId INT,
    IN pDosePattern VARCHAR(20),
    IN pDoseUsage VARCHAR(20),
    IN pDays INT,
    IN pQty INT
)
BEGIN
    INSERT INTO DrPrescriptionDetails (
        DrPrescriptionId, ProductId, DosePatternId,
        DosePattern, DoseUsage, DoseDays, Qty
    )
    VALUES (
        pDrPrescriptionId, pProductId, pDosePatternId,
        pDosePattern, pDoseUsage, pDays, pQty
    );
END //

/* ------------------------------------------------
   Get Prescription List
------------------------------------------------ */
CREATE PROCEDURE sp_GetPrescriptionList()
BEGIN
    SELECT 
        dp.DrPrescriptionId,
        dp.PatientId,
        p.PatientName,
        dp.EntryType,
        dp.EntryNumber,
        dp.EntryPeriod,
        dp.DeseaseId AS DiseaseId,
        d.Desease,
        dp.Height,
        dp.Weight,
        dp.BMI,
        dp.TempInCelcius,
        dp.BP,
        dp.SPO2,
        dp.PulseRate,
        dp.PrescriptionDate,
        dp.NextVisitDate
    FROM DrPrescription dp
    LEFT JOIN patients_master p ON dp.PatientId = p.PatientId
    LEFT JOIN diseasemaster d ON dp.DeseaseId = d.DeseaseId
    ORDER BY dp.DrPrescriptionId DESC;
END //

/* ------------------------------------------------
   Save Clinical Notes
------------------------------------------------ */
CREATE PROCEDURE sp_SavePrescriptionClinical(
    IN P_PrescriptionId INT,
    IN P_ClinicalNote VARCHAR(500),
    IN P_Result VARCHAR(500)
)
BEGIN
    INSERT INTO DrPrescriptionClinical (
        DrPrescriptionId, ClinicalNote, Result
    )
    VALUES (
        P_PrescriptionId, P_ClinicalNote, P_Result
    );
END //

DELIMITER ;
