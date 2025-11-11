-- Drop procedures if they exist
DROP PROCEDURE IF EXISTS GetCommonMasterList;
DROP PROCEDURE IF EXISTS GetCommonMasterById;
DROP PROCEDURE IF EXISTS CreateCommonMaster;
DROP PROCEDURE IF EXISTS UpdateCommonMaster;
DROP PROCEDURE IF EXISTS DeleteCommonMaster;

DELIMITER $$

-- Procedure to get all CommonMaster records
CREATE PROCEDURE GetCommonMasterList()
BEGIN
    SELECT 
        Id,
        MasterType,
        MasterName
    FROM CommonMaster;
END $$

-- Procedure to get a single CommonMaster record by Id
CREATE PROCEDURE GetCommonMasterById(IN p_Id INT)
BEGIN
    SELECT 
        Id,
        MasterType,
        MasterName
    FROM CommonMaster
    WHERE Id = p_Id;
END $$

-- Procedure to insert a new CommonMaster record
CREATE PROCEDURE CreateCommonMaster(
    IN p_MasterType VARCHAR(50),
    IN p_MasterName VARCHAR(255)
)
BEGIN
    INSERT INTO CommonMaster (MasterType, MasterName)
    VALUES (p_MasterType, p_MasterName);
END $$

-- Procedure to update an existing CommonMaster record
CREATE PROCEDURE UpdateCommonMaster(
    IN p_Id INT,
    IN p_MasterType VARCHAR(50),
    IN p_MasterName VARCHAR(255)
)
BEGIN
    UPDATE CommonMaster
    SET 
        MasterType = p_MasterType,
        MasterName = p_MasterName
    WHERE Id = p_Id;
END $$

-- Procedure to delete a CommonMaster record by Id
CREATE PROCEDURE DeleteCommonMaster(IN p_Id INT)
BEGIN
    DELETE FROM CommonMaster WHERE Id = p_Id;
END $$

DELIMITER ;
