DELIMITER $$

CREATE PROCEDURE DeleteDescriptionMaster(IN p_DescriptionId INT)
BEGIN
    START TRANSACTION;
        DELETE FROM DescriptionsMasterTestValue WHERE DescriptionId = p_DescriptionId;
        DELETE FROM DescriptionsMaster WHERE DescriptionId = p_DescriptionId;
    COMMIT;
END$$

DELIMITER ;
