CREATE TABLE PrescriptionDetail (
    PrescriptionDetailId INT NOT NULL,
    Description VARCHAR(255) NOT NULL,
    Amount DECIMAL(10,2),
    PrescriptionId INT NOT NULL,  -- Foreign key column
    PRIMARY KEY (PrescriptionDetailId),
    CONSTRAINT FK_PrescriptionDetail_Prescription
        FOREIGN KEY (PrescriptionId)
        REFERENCES Prescription(PrescriptionId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
