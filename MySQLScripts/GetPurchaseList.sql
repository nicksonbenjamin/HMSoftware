-- =====================================================
-- 1️⃣ Tables for Purchase
-- =====================================================

-- Purchase Master
CREATE TABLE IF NOT EXISTS purchase_master (
    id INT AUTO_INCREMENT PRIMARY KEY,
    purchase_no VARCHAR(50) NOT NULL,
    purchase_date DATE NOT NULL,
    supplier_id INT NOT NULL,
    supplier_invoice_no VARCHAR(50),
    supplier_invoice_date DATE,
    total_qty DECIMAL(10,2) DEFAULT 0,
    total_amount DECIMAL(12,2) DEFAULT 0,
    total_discount DECIMAL(12,2) DEFAULT 0,
    total_tax DECIMAL(12,2) DEFAULT 0,
    round_off DECIMAL(12,2) DEFAULT 0,
    net_amount DECIMAL(12,2) DEFAULT 0,
    remarks VARCHAR(255),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (supplier_id) REFERENCES ledgermaster(id)
);

-- Purchase Item
CREATE TABLE IF NOT EXISTS purchase_item (
    id INT AUTO_INCREMENT PRIMARY KEY,
    purchase_id INT NOT NULL,
    product_id INT NOT NULL,
    batch_no VARCHAR(50),
    expiry_date DATE,
    qty DECIMAL(10,2) DEFAULT 0,
    free_qty DECIMAL(10,2) DEFAULT 0,
    rate DECIMAL(12,2) DEFAULT 0,
    mrp DECIMAL(12,2) DEFAULT 0,
    discount_percent DECIMAL(5,2) DEFAULT 0,
    tax_percent DECIMAL(5,2) DEFAULT 0,
    net_amount DECIMAL(12,2) DEFAULT 0,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (purchase_id) REFERENCES purchase_master(id),
    FOREIGN KEY (product_id) REFERENCES productmaster(ProductCode)
);

-- =====================================================
-- 2️⃣ Stored Procedure: GetPurchaseList
-- =====================================================

DELIMITER $$

DROP PROCEDURE IF EXISTS GetPurchaseList $$
CREATE PROCEDURE GetPurchaseList()
BEGIN
    SELECT 
        pm.id AS PurchaseId,
        pm.purchase_no AS PurchaseNo,
        pm.purchase_date AS PurchaseDate,
        lm.LedgerName AS SupplierName,
        pm.total_qty AS TotalQty,
        pm.total_amount AS TotalAmount,
        pm.total_discount AS TotalDiscount,
        pm.total_tax AS TotalTax,
        pm.round_off AS RoundOff,
        pm.net_amount AS NetAmount
    FROM purchase_master pm
    INNER JOIN ledgermaster lm ON pm.supplier_id = lm.id
    ORDER BY pm.purchase_date DESC;
END $$

DELIMITER ;

-- Grant execution rights
GRANT EXECUTE ON PROCEDURE `GetPurchaseList` TO 'root'@'localhost';
FLUSH PRIVILEGES;
