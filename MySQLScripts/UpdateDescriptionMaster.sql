DELIMITER $$

CREATE PROCEDURE UpdateDescriptionMaster(
    IN p_DescriptionId INT,
    IN p_DescriptionCode VARCHAR(50),
    IN p_DescriptionName VARCHAR(100),
    IN p_Group VARCHAR(50),
    IN p_Section VARCHAR(50),
    IN p_Applicable_All BOOLEAN,
    IN p_Applicable_LAB BOOLEAN,
    IN p_Applicable_OP BOOLEAN,
    IN p_Consultation BOOLEAN,
    IN p_NormalCharges DECIMAL(10,2),
    IN p_EmergencyCharges DECIMAL(10,2),
    IN p_DrCharges DECIMAL(10,2),
    IN p_IsActive BOOLEAN
)
BEGIN
    UPDATE DescriptionMaster SET
        DescriptionCode = p_DescriptionCode,
        DescriptionName = p_DescriptionName,
        `Group` = p_Group,
        Section = p_Section,
        Applicable_All = p_Applicable_All,
        Applicable_LAB = p_Applicable_LAB,
        Applicable_OP = p_Applicable_OP,
        Consultation = p_Consultation,
        NormalCharges = p_NormalCharges,
        EmergencyCharges = p_EmergencyCharges,
        DrCharges = p_DrCharges,
        IsActive = p_IsActive
    WHERE DescriptionId = p_DescriptionId;
END$$

DELIMITER ;
