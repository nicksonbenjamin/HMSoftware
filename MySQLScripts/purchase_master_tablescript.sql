CREATE TABLE purchase_master (
    id INT AUTO_INCREMENT PRIMARY KEY,

    purchase_no VARCHAR(50) NOT NULL,
    purchase_date DATE NOT NULL,

    supplier_id INT NOT NULL,   -- FK from ledgermaster.id
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

ALTER TABLE productmaster
ADD StockQty DECIMAL(18,2) DEFAULT 0,
ADD StockUnit VARCHAR(50);


CREATE TABLE stockledger (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ProductId INT,
    QtyIn DECIMAL(18,2),
    QtyOut DECIMAL(18,2),
    RefType VARCHAR(20),
    RefId INT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

