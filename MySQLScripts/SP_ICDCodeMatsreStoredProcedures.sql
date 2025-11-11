-- =====================================================
-- ICDCodeMaster Stored Procedures - Drop and Recreate
-- =====================================================

DELIMITER $$

-- 1. Get all ICD codes
DROP PROCEDURE IF EXISTS `GetICDCodeMasterList`$$
CREATE PROCEDURE `GetICDCodeMasterList`()
BEGIN
    SELECT 
        icdcodemasterId AS ICDCodeMasterId,
        icd_code AS ICDCode,
        diagnosis_condition AS DiagnosisCondition,
        description_usage AS DescriptionUsage,
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM icdcode_master;
END $$

-- 2. Get ICD code by ID
DROP PROCEDURE IF EXISTS `GetICDCodeMasterById`$$
CREATE PROCEDURE `GetICDCodeMasterById`(
    IN p_ICDCodeMasterId INT
)
BEGIN
    SELECT 
        icdcodemasterId AS ICDCodeMasterId,
        icd_code AS ICDCode,
        diagnosis_condition AS DiagnosisCondition,
        description_usage AS DescriptionUsage,
        IsActive,
        CreatedAt,
        UpdatedAt
    FROM icdcode_master
    WHERE icdcodemasterId = p_ICDCodeMasterId;
END $$

-- 3. Create a new ICD code
DROP PROCEDURE IF EXISTS `CreateICDCodeMaster`$$
CREATE PROCEDURE `CreateICDCodeMaster`(
    IN p_ICDCode VARCHAR(10),
    IN p_DiagnosisCondition VARCHAR(255),
    IN p_DescriptionUsage VARCHAR(255),
    IN p_IsActive BOOLEAN
)
BEGIN
    INSERT INTO icdcode_master (
        icd_code,
        diagnosis_condition,
        description_usage,
        IsActive,
        CreatedAt,
        UpdatedAt
    ) VALUES (
        p_ICDCode,
        p_DiagnosisCondition,
        p_DescriptionUsage,
        p_IsActive,
        NOW(),
        NOW()
    );
END $$

-- 4. Update an existing ICD code
DROP PROCEDURE IF EXISTS `UpdateICDCodeMaster`$$
CREATE PROCEDURE `UpdateICDCodeMaster`(
    IN p_ICDCodeMasterId INT,
    IN p_ICDCode VARCHAR(10),
    IN p_DiagnosisCondition VARCHAR(255),
    IN p_DescriptionUsage VARCHAR(255),
    IN p_IsActive BOOLEAN
)
BEGIN
    UPDATE icdcode_master
    SET 
        icd_code = p_ICDCode,
        diagnosis_condition = p_DiagnosisCondition,
        description_usage = p_DescriptionUsage,
        IsActive = p_IsActive,
        UpdatedAt = NOW()
    WHERE icdcodemasterId = p_ICDCodeMasterId;
END $$

-- 5. Delete an ICD code
DROP PROCEDURE IF EXISTS `DeleteICDCodeMaster`$$
CREATE PROCEDURE `DeleteICDCodeMaster`(
    IN p_ICDCodeMasterId INT
)
BEGIN
    DELETE FROM icdcode_master
    WHERE icdcodemasterId = p_ICDCodeMasterId;
END $$

DELIMITER ;

-- =====================================================
-- All procedures for ICDCodeMaster have been recreated
-- =====================================================
