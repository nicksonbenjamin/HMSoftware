LOAD DATA LOCAL INFILE 'D:\\PrjectNew\\Downlaods\\051120252208321001_descriptions.csv'
INTO TABLE DescriptionMaster
FIELDS TERMINATED BY ','
ENCLOSED BY '"'
LINES TERMINATED BY '\r\n'
IGNORE 1 LINES
(DescriptionCode, DescriptionName, `Group`, Section, NormalCharges, EmergencyCharges, DrCharges, Remarks, IsActive);
