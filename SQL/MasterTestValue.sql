CREATE TABLE DescriptionsMasterTestValue (
    TestValueID INT AUTO_INCREMENT PRIMARY KEY,
    DescriptionId integer,
    Specimen VARCHAR(100),
    TestName VARCHAR(255),
    Male VARCHAR(100),
    Female VARCHAR(100),
    Children VARCHAR(100),
    General VARCHAR(100),
    Unit VARCHAR(50),
    Method VARCHAR(100),
    HtmlStyle VARCHAR(255),
    Instrument VARCHAR(100),
    SortOrder INT,
    FOREIGN KEY (DescriptionId) REFERENCES DescriptionsMaster(DescriptionId)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
