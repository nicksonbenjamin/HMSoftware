DELIMITER $$

-- ===============================
-- Drop if exists
-- ===============================
DROP PROCEDURE IF EXISTS GetProductById$$
DROP PROCEDURE IF EXISTS UpdateStockAfterPurchase$$

-- ===============================
-- Get Product Details
-- ===============================
CREATE PROCEDURE GetProductById(IN p_ProductId INT)
BEGIN
    SELECT 
        ProductCode,
        StockUnit,
        TaxPercent,
        PurchaseRate
    FROM productmaster
    WHERE ProductCode = p_ProductId;
END$$

-- ===============================
-- Update Stock After Purchase
-- ===============================
CREATE PROCEDURE UpdateStockAfterPurchase(
    IN p_ProductId INT,
    IN p_Qty DECIMAL(18,2),
    IN p_PurchaseId INT
)
BEGIN
    UPDATE productmaster
    SET StockQty = IFNULL(StockQty,0) + p_Qty
    WHERE ProductCode = p_ProductId;

    INSERT INTO stockledger
        (ProductId, QtyIn, QtyOut, RefType, RefId)
    VALUES
        (p_ProductId, p_Qty, 0, 'PURCHASE', p_PurchaseId);
END$$

DELIMITER ;
