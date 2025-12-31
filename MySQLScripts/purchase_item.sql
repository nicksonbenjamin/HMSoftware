CREATE TABLE purchase_item (
    id INT AUTO_INCREMENT PRIMARY KEY,
    purchase_id INT NOT NULL,       -- FK to purchase_master.id
    product_id INT NOT NULL,        -- FK to productmaster.ProductCode
    batch_no VARCHAR(50),
    expiry_date DATE,
    qty DECIMAL(10,2) NOT NULL DEFAULT 0,
    free_qty DECIMAL(10,2) NOT NULL DEFAULT 0,
    rate DECIMAL(12,2) NOT NULL DEFAULT 0,
    mrp DECIMAL(12,2) NOT NULL DEFAULT 0,
    discount_percent DECIMAL(5,2) NOT NULL DEFAULT 0,
    tax_percent DECIMAL(5,2) NOT NULL DEFAULT 0,
    net_amount DECIMAL(12,2) NOT NULL DEFAULT 0,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (purchase_id) REFERENCES purchase_master(id),
    FOREIGN KEY (product_id) REFERENCES productmaster(ProductCode)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
