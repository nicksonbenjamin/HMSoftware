/* =========================================================
   PURCHASE MODULE – STORED PROCEDURES
   ========================================================= */

DELIMITER $$

/* =========================================================
   1️⃣ GET PURCHASE LIST
   ========================================================= */
DROP PROCEDURE IF EXISTS GetPurchaseList $$
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

/* =========================================================
   2️⃣ GET PURCHASE MASTER BY ID
   ========================================================= */
DROP PROCEDURE IF EXISTS GetPurchaseById $$
CREATE PROCEDURE GetPurchaseById(IN p_Id INT)
BEGIN
    SELECT 
        pm.id AS PurchaseId,
        pm.purchase_no AS PurchaseNo,
        pm.purchase_date AS PurchaseDate,
        lm.ledger_name AS SupplierName,
        pm.supplier_invoice_no AS SupplierInvoiceNo,
        pm.supplier_invoice_date AS SupplierInvoiceDate,
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

/* =========================================================
   3️⃣ GET PURCHASE ITEMS BY PURCHASE ID
   ========================================================= */
DROP PROCEDURE IF EXISTS GetPurchaseItemsByPurchaseId $$
CREATE PROCEDURE GetPurchaseItemsByPurchaseId(IN p_PurchaseId INT)
BEGIN
    SELECT
        pi.product_id AS ProductId,
        pr.ProductName,
        pi.batch_no AS BatchNo,
        pi.expiry_date AS ExpiryDate,
        pr.StockUnit AS UOM,
        pi.qty AS Qty,
        pi.free_qty AS FreeQty,
        pi.rate AS Rate,
        pi.mrp AS MRP,
        pi.discount_percent AS DiscountPercent,
        pi.tax_percent AS TaxPercent,
        pi.net_amount AS NetAmount
    FROM purchase_item pi
    INNER JOIN productmaster pr 
        ON pi.product_id = pr.ProductCode
    WHERE pi.purchase_id = p_PurchaseId;
END $$

/* =========================================================
   4️⃣ CREATE PURCHASE MASTER
   ========================================================= */
DROP PROCEDURE IF EXISTS CreatePurchase $$
CREATE PROCEDURE CreatePurchase(
    IN p_PurchaseNo VARCHAR(50),
    IN p_PurchaseDate DATE,
    IN p_SupplierId INT,
    IN p_TotalQty DECIMAL(10,2),
    IN p_TotalAmount DECIMAL(12,2),
    IN p_TotalDiscount DECIMAL(12,2),
    IN p_TotalTax DECIMAL(12,2),
    IN p_RoundOff DECIMAL(12,2),
    IN p_NetAmount DECIMAL(12,2),
    IN p_Remarks VARCHAR(255),
    IN p_SupplierInvoiceNo VARCHAR(50),
    IN p_SupplierInvoiceDate DATE
)
BEGIN
    INSERT INTO purchase_master
    (
        purchase_no,
        purchase_date,
        supplier_id,
        supplier_invoice_no,
        supplier_invoice_date,
        total_qty,
        total_amount,
        total_discount,
        total_tax,
        round_off,
        net_amount,
        remarks
    )
    VALUES
    (
        p_PurchaseNo,
        p_PurchaseDate,
        p_SupplierId,
        p_SupplierInvoiceNo,
        p_SupplierInvoiceDate,
        p_TotalQty,
        p_TotalAmount,
        p_TotalDiscount,
        p_TotalTax,
        p_RoundOff,
        p_NetAmount,
        p_Remarks
    );

    SELECT LAST_INSERT_ID() AS NewPurchaseId;
END $$

/* =========================================================
   5️⃣ CREATE PURCHASE ITEM
   ========================================================= */
DROP PROCEDURE IF EXISTS CreatePurchaseItem $$
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
    (
        purchase_id,
        product_id,
        batch_no,
        expiry_date,
        qty,
        free_qty,
        rate,
        mrp,
        discount_percent,
        tax_percent,
        net_amount
    )
    VALUES
    (
        p_PurchaseId,
        p_ProductId,
        p_BatchNo,
        p_ExpiryDate,
        p_Qty,
        p_FreeQty,
        p_Rate,
        p_MRP,
        p_DiscountPercent,
        p_TaxPercent,
        p_NetAmount
    );
END $$

/* =========================================================
   6️⃣ DELETE PURCHASE (MASTER + ITEMS)
   ========================================================= */
DROP PROCEDURE IF EXISTS DeletePurchase $$
CREATE PROCEDURE DeletePurchase(IN p_Id INT)
BEGIN
    DELETE FROM purchase_item WHERE purchase_id = p_Id;
    DELETE FROM purchase_master WHERE id = p_Id;
END $$

/* =========================================================
   7️⃣ GET PRODUCT DETAILS (FOR AJAX)
   ========================================================= */
DROP PROCEDURE IF EXISTS GetProductById $$
CREATE PROCEDURE GetProductById(IN p_ProductId INT)
BEGIN
    SELECT 
        ProductCode,
        ProductName,
        StockUnit,
        TaxPercent,
        PurchaseRate,
        MRP
    FROM productmaster
    WHERE ProductCode = p_ProductId;
END $$

/* =========================================================
   8️⃣ UPDATE STOCK AFTER PURCHASE
   ========================================================= */
DROP PROCEDURE IF EXISTS UpdateStockAfterPurchase $$
CREATE PROCEDURE UpdateStockAfterPurchase(
    IN p_ProductId INT,
    IN p_Qty DECIMAL(18,2),
    IN p_PurchaseId INT
)
BEGIN
    UPDATE productmaster
    SET StockQty = IFNULL(StockQty, 0) + p_Qty
    WHERE ProductCode = p_ProductId;

    INSERT INTO stockledger
    (
        ProductId,
        QtyIn,
        QtyOut,
        RefType,
        RefId
    )
    VALUES
    (
        p_ProductId,
        p_Qty,
        0,
        'PURCHASE',
        p_PurchaseId
    );
END $$

/* =========================================================
   9️⃣ REVERSE STOCK (FOR EDIT/DELETE)
   ========================================================= */
DROP PROCEDURE IF EXISTS ReverseStockAfterPurchase $$
CREATE PROCEDURE ReverseStockAfterPurchase(
    IN p_ProductId INT,
    IN p_Qty DECIMAL(18,2)
)
BEGIN
    UPDATE productmaster
    SET StockQty = StockQty - p_Qty
    WHERE ProductCode = p_ProductId;

    INSERT INTO stockledger
    (
        ProductId,
        QtyIn,
        QtyOut,
        RefType,
        RefId
    )
    VALUES
    (
        p_ProductId,
        0,
        p_Qty,
        'PURCHASE_REVERSE',
        0
    );
END $$


DROP PROCEDURE IF EXISTS UpdatePurchase $$
CREATE PROCEDURE UpdatePurchase(
    IN p_PurchaseId INT,
    IN p_PurchaseDate DATE,
    IN p_SupplierId INT,
    IN p_TotalQty DECIMAL(10,2),
    IN p_TotalAmount DECIMAL(12,2),
    IN p_TotalDiscount DECIMAL(12,2),
    IN p_TotalTax DECIMAL(12,2),
    IN p_RoundOff DECIMAL(12,2),
    IN p_NetAmount DECIMAL(12,2),
    IN p_Remarks VARCHAR(255),
    IN p_SupplierInvoiceNo VARCHAR(50),
    IN p_SupplierInvoiceDate DATE
)
BEGIN
    UPDATE purchase_master
    SET
        purchase_date = p_PurchaseDate,
        supplier_id = p_SupplierId,
        supplier_invoice_no = p_SupplierInvoiceNo,
        supplier_invoice_date = p_SupplierInvoiceDate,
        total_qty = p_TotalQty,
        total_amount = p_TotalAmount,
        total_discount = p_TotalDiscount,
        total_tax = p_TotalTax,
        round_off = p_RoundOff,
        net_amount = p_NetAmount,
        remarks = p_Remarks
    WHERE id = p_PurchaseId;
END $$

DROP PROCEDURE IF EXISTS DeletePurchaseItems $$
CREATE PROCEDURE DeletePurchaseItems(IN p_PurchaseId INT)
BEGIN
    DELETE FROM purchase_item WHERE purchase_id = p_PurchaseId;
END $$

DROP PROCEDURE IF EXISTS ReverseStockAfterPurchase $$
CREATE PROCEDURE ReverseStockAfterPurchase(
    IN p_ProductId INT,
    IN p_Qty DECIMAL(18,2),
    IN p_PurchaseId INT
)
BEGIN
    UPDATE productmaster
    SET StockQty = StockQty - p_Qty
    WHERE ProductCode = p_ProductId;

    INSERT INTO stockledger
    (
        ProductId,
        QtyIn,
        QtyOut,
        RefType,
        RefId
    )
    VALUES
    (
        p_ProductId,
        0,
        p_Qty,
        'PURCHASE_REVERSE',
        p_PurchaseId
    );
END $$

/* =========================================================
   EXECUTION GRANTS
   ========================================================= */
GRANT EXECUTE ON PROCEDURE GetPurchaseList TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE GetPurchaseById TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE GetPurchaseItemsByPurchaseId TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE CreatePurchase TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE CreatePurchaseItem TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE DeletePurchase TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE GetProductById TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE UpdateStockAfterPurchase TO 'root'@'localhost';
GRANT EXECUTE ON PROCEDURE ReverseStockAfterPurchase TO 'root'@'localhost';

FLUSH PRIVILEGES;

DELIMITER ;

