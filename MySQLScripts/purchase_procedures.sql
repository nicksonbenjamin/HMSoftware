-- ===============================================
-- DROP EXISTING PROCEDURES (if they exist)
-- ===============================================

DROP PROCEDURE IF EXISTS GetPurchaseList;
DROP PROCEDURE IF EXISTS GetPurchaseById;
DROP PROCEDURE IF EXISTS CreatePurchase;
DROP PROCEDURE IF EXISTS CreatePurchaseItem;
DROP PROCEDURE IF EXISTS DeletePurchase;

-- ===============================================
-- GET ALL PURCHASES
-- ===============================================
DELIMITER $$

CREATE PROCEDURE GetPurchaseList()
BEGIN
    SELECT 
        pm.id AS PurchaseId,
        pm.purchase_no AS PurchaseNo,
        pm.purchase_date AS PurchaseDate,
        lm.ledger_name AS SupplierName,
        pm.total_qty AS TotalQty,
        pm.total_amount AS TotalAmount,
        pm.total_discount AS TotalDiscount,
        pm.total_tax AS TotalTax,
        pm.round_off AS RoundOff,
        pm.net_amount AS NetAmount,
        pm.supplier_id AS SupplierId
    FROM purchase_master pm
    INNER JOIN ledgermaster lm ON pm.supplier_id = lm.id
    ORDER BY pm.purchase_date DESC;
END $$

-- ===============================================
-- GET PURCHASE BY ID
-- ===============================================
CREATE PROCEDURE GetPurchaseById(IN p_Id INT)
BEGIN
    SELECT 
        pm.id AS PurchaseId,
        pm.purchase_no AS PurchaseNo,
        pm.purchase_date AS PurchaseDate,
        lm.ledger_name AS SupplierName,
        pm.total_qty AS TotalQty,
        pm.total_amount AS TotalAmount,
        pm.total_discount AS TotalDiscount,
        pm.total_tax AS TotalTax,
        pm.round_off AS RoundOff,
        pm.net_amount AS NetAmount,
        pm.remarks AS Remarks,
        pm.supplier_id AS SupplierId
    FROM purchase_master pm
    INNER JOIN ledgermaster lm ON pm.supplier_id = lm.id
    WHERE pm.id = p_Id;
END $$

-- ===============================================
-- CREATE PURCHASE (returns last inserted ID)
-- ===============================================
CREATE PROCEDURE CreatePurchase(
    IN p_PurchaseNo VARCHAR(50),
    IN p_PurchaseDate DATE,
    IN p_SupplierId INT,
    IN p_TotalAmount DECIMAL(12,2),
    IN p_NetAmount DECIMAL(12,2),
    IN p_Remarks VARCHAR(255)
)
BEGIN
    INSERT INTO purchase_master
        (purchase_no, purchase_date, supplier_id, total_amount, net_amount, remarks)
    VALUES
        (p_PurchaseNo, p_PurchaseDate, p_SupplierId, p_TotalAmount, p_NetAmount, p_Remarks);

    SELECT LAST_INSERT_ID() AS NewPurchaseId;
END $$

-- ===============================================
-- CREATE PURCHASE ITEM
-- ===============================================
CREATE PROCEDURE CreatePurchaseItem(
    IN p_PurchaseId INT,
    IN p_ProductId INT,
    IN p_BatchNo VARCHAR(50),
    IN p_ExpiryDate DATE,
    IN p_Qty DECIMAL(10,2),
    IN p_FreeQty DECIMAL(10,2),
    IN p_Rate DECIMAL(12,2),
    IN p_MRP DECIMAL(12,2),
    IN p_DiscountPercent DECIMAL(5,2),
    IN p_TaxPercent DECIMAL(5,2),
    IN p_NetAmount DECIMAL(12,2)
)
BEGIN
    INSERT INTO purchase_item
        (purchase_id, product_id, batch_no, expiry_date, qty, free_qty, rate, mrp, discount_percent, tax_percent, net_amount)
    VALUES
        (p_PurchaseId, p_ProductId, p_BatchNo, p_ExpiryDate, p_Qty, p_FreeQty, p_Rate, p_MRP, p_DiscountPercent, p_TaxPercent, p_NetAmount);
END $$

-- ===============================================
-- DELETE PURCHASE (will also delete purchase items)
-- ===============================================
CREATE PROCEDURE DeletePurchase(IN p_Id INT)
BEGIN
    DELETE FROM purchase_item WHERE purchase_id = p_Id;
    DELETE FROM purchase_master WHERE id = p_Id;
END $$

DELIMITER ;

-- ===============================================
-- GRANT EXECUTE PRIVILEGE (optional for root user)
-- ===============================================
GRANT EXECUTE ON PROCEDURE GetPurchaseList TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE GetPurchaseById TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE CreatePurchase TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE CreatePurchaseItem TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE DeletePurchase TO 'root'@'localhost';
FLUSH PRIVILEGES;
