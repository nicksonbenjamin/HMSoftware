DELIMITER $$

CREATE PROCEDURE DeleteDescriptionTestValues(IN p_DescriptionId INT)
BEGIN
    DELETE FROM DescriptionsMasterTestValue WHERE DescriptionId = p_DescriptionId;
END$$

DELIMITER ;
