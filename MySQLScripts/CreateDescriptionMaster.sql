DROP PROCEDURE IF EXISTS CreateDescriptionMaster;
DELIMITER $$

CREATE PROCEDURE CreateDescriptionMaster(
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
    DECLARE v_NewId INT;

    START TRANSACTION;
        INSERT INTO DescriptionMaster
        (
            DescriptionCode, DescriptionName, `Group`, Section,
            Applicable_All, Applicable_LAB, Applicable_OP, Consultation,
            NormalCharges, EmergencyCharges, DrCharges, IsActive
        )
        VALUES
        (
            p_DescriptionCode, p_DescriptionName, p_Group, p_Section,
            p_Applicable_All, p_Applicable_LAB, p_Applicable_OP, p_Consultation,
            p_NormalCharges, p_EmergencyCharges, p_DrCharges, p_IsActive
        );

        SET v_NewId = LAST_INSERT_ID();
    COMMIT;

    SELECT v_NewId AS NewDescriptionId;
END$$

DELIMITER ;
