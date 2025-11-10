DROP PROCEDURE IF EXISTS AddDescriptionMasterTestValue;
DELIMITER $$

CREATE PROCEDURE AddDescriptionMasterTestValue(
    IN p_DescriptionId INT,
    IN p_Specimen VARCHAR(100),
    IN p_TestName VARCHAR(100),
    IN p_Male VARCHAR(100),
    IN p_Female VARCHAR(100),
    IN p_Children VARCHAR(100),
    IN p_General VARCHAR(100),
    IN p_Unit VARCHAR(50),
    IN p_Method VARCHAR(50),
    IN p_HtmlStyle VARCHAR(255),
    IN p_Instrument VARCHAR(100),
    IN p_SortOrder INT
)
BEGIN
    INSERT INTO DescriptionMasterTestValue
    (
        DescriptionId, Specimen, TestName, Male, Female, Children, General,
        Unit, Method, HtmlStyle, Instrument, SortOrder
    )
    VALUES
    (
        p_DescriptionId, p_Specimen, p_TestName, p_Male, p_Female, p_Children, p_General,
        p_Unit, p_Method, p_HtmlStyle, p_Instrument, p_SortOrder
    );

    SELECT LAST_INSERT_ID() AS NewTestValueId;
END$$

DELIMITER ;
