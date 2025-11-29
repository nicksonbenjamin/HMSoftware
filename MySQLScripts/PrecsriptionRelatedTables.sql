-- Table: EntryType
CREATE TABLE EntryType (
    EntryTypeId INT AUTO_INCREMENT PRIMARY KEY,
    EntryTypePeriod VARCHAR(10)
);

-- ---------------------------------------------

-- Table: DrPrescription
CREATE TABLE DrPrescription (
    DrPrescriptionId INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT,
    PatientName VARCHAR(100),
    PrescriptionDate DATETIME,
    EntryType VARCHAR(10),
    EntryNumber INT,
    EntryPeriod VARCHAR(10),
    DeseaseId INT,
    Height DECIMAL(10,2),
    Weight DECIMAL(10,2),
    BMI DECIMAL(10,2),
    tempinCelcius DECIMAL(10,2),
    BP VARCHAR(10),
    SPO2 VARCHAR(20),
    PulseRate DECIMAL(10,2),
    NextVisitDate DATETIME
);

-- ---------------------------------------------

-- Table: DiseaseMaster
CREATE TABLE DiseaseMaster (
    DeseaseId INT AUTO_INCREMENT PRIMARY KEY,
    Desease VARCHAR(50),
    DeseaseDetails VARCHAR(100)
);

-- ---------------------------------------------

-- Table: DrPrescriptionDetails
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

-- ---------------------------------------------

-- Table: DosePattern
CREATE TABLE DosePattern (
    DosePatternId INT AUTO_INCREMENT PRIMARY KEY,
    DoseDescription VARCHAR(20)
);

-- ---------------------------------------------

-- Table: DrPrescriptionClinical
CREATE TABLE DrPrescriptionClinical (
    DrPrescriptionClinicalId INT AUTO_INCREMENT PRIMARY KEY,
    DrPrescriptionId INT,
    ClinicalNote VARCHAR(200),
    Result VARCHAR(100)
);

INSERT INTO DiseaseMaster (Desease, DeseaseDetails)
VALUES 
('Flu', 'Influenza viral infection affecting respiratory tract'),
('Diabetes', 'Chronic condition affecting blood sugar levels'),
('Hypertension', 'High blood pressure condition'),
('Migraine', 'Severe recurring headache condition'),
('Asthma', 'Chronic respiratory condition with airway inflammation');



INSERT  INTO DosePattern (DoseDescription)
VALUES 
('1-0-0'),
('0-1-1'),
('1-1-1'),
('0-0-1'),
('1-0-1');

desc DiseaseMaster