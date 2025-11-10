DROP PROCEDURE IF EXISTS GetDescriptionMasterById;
DELIMITER $$

CREATE PROCEDURE GetDescriptionMasterById(IN p_DescriptionId INT)
BEGIN
    SELECT * 
    FROM DescriptionMaster 
    WHERE DescriptionId = p_DescriptionId;

    SELECT * 
    FROM DescriptionMasterTestValue 
    WHERE DescriptionId = p_DescriptionId
    ORDER BY SortOrder;
END$$

DELIMITER ;
