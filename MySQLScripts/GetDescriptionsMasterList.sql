DROP PROCEDURE IF EXISTS GetDescriptionsMasterList;
DELIMITER $$

CREATE PROCEDURE GetDescriptionsMasterList()
BEGIN
    SELECT * FROM DescriptionMaster;
END$$

DELIMITER ;
