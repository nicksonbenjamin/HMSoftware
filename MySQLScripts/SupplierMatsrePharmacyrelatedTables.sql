-- DROP existing tables if they exist
DROP TABLE IF EXISTS PharmacyBillDetails;
DROP TABLE IF EXISTS PharmacyBill;
DROP TABLE IF EXISTS PharmacyStock;
DROP TABLE IF EXISTS ProductSupplier;
DROP TABLE IF EXISTS SupplierMaster;



-- ===============================================
-- 2️⃣ SupplierMaster Table
-- ===============================================
CREATE TABLE SupplierMaster (
    SupplierId INT AUTO_INCREMENT PRIMARY KEY,
    SupplierName VARCHAR(255) NOT NULL,
    ContactPerson VARCHAR(100),
    MobileNo VARCHAR(20),
    Email VARCHAR(100),
    Address TEXT,
    GSTNo VARCHAR(50),
    IsActive TINYINT(1) DEFAULT 1,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===============================================
-- 3️⃣ ProductSupplier Table
-- ===============================================
CREATE TABLE ProductSupplier (
    ProductSupplierId INT AUTO_INCREMENT PRIMARY KEY,
    ProductCode INT NOT NULL,
    SupplierId INT NOT NULL,
    PurchaseRate DECIMAL(18,2),
    PurchaseDiscount DECIMAL(18,2),
    SaleRate DECIMAL(18,2),
    SaleDiscount DECIMAL(18,2),
    TaxPercent DECIMAL(5,2),
    MRP DECIMAL(18,2),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
--    FOREIGN KEY (ProductCode) REFERENCES ProductMaster(ProductCode),
 --   FOREIGN KEY (SupplierId) REFERENCES SupplierMaster(SupplierId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===============================================
-- 4️⃣ PharmacyStock Table
-- ===============================================
CREATE TABLE PharmacyStock (
    StockId INT AUTO_INCREMENT PRIMARY KEY,
    ProductSupplierId INT NOT NULL,
    BatchNo VARCHAR(50) NOT NULL,
    ExpiryDate DATE NOT NULL,
    QuantityInStock DECIMAL(18,2) NOT NULL DEFAULT 0,
    Rack VARCHAR(50),
    Bin VARCHAR(50),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
  --  FOREIGN KEY (ProductSupplierId) REFERENCES ProductSupplier(ProductSupplierId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===============================================
-- 5️⃣ PharmacyBill Table
-- ===============================================
CREATE TABLE PharmacyBill (
    BillId INT AUTO_INCREMENT PRIMARY KEY,
    PatientId INT NOT NULL,
    EntryId INT NOT NULL,
    BillDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0,
    Tax DECIMAL(18,2) DEFAULT 0,
    NetAmount DECIMAL(18,2) NOT NULL,
    PaymentMode VARCHAR(20) DEFAULT 'Cash', -- Cash, Card, Insurance
    CreatedBy VARCHAR(50),
    UpdatedBy VARCHAR(50),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    -- FOREIGN KEYS to patients_master and patient_entry can be added if needed
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===============================================
-- 6️⃣ PharmacyBillDetails Table
-- ===============================================
CREATE TABLE PharmacyBillDetails (
    BillDetailsId INT AUTO_INCREMENT PRIMARY KEY,
    BillId INT NOT NULL,
    StockId INT NOT NULL,
    ProductCode INT NOT NULL,
    ProductName VARCHAR(255) NOT NULL,
    BatchNo VARCHAR(50),
    Qty DECIMAL(18,2) NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0,
    Tax DECIMAL(5,2) DEFAULT 0,
    Total DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (BillId) REFERENCES PharmacyBill(BillId) ON DELETE CASCADE,
    FOREIGN KEY (StockId) REFERENCES PharmacyStock(StockId) ON DELETE RESTRICT,
    FOREIGN KEY (ProductCode) REFERENCES ProductMaster(ProductCode) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ===============================================
-- ✅ Script Completed
-- ===============================================


