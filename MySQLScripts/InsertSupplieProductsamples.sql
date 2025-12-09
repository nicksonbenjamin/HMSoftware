-- ===============================================
-- Sample Data Inserts for Pharmacy System
-- Tables: ProductMaster, SupplierMaster, ProductSupplier
-- ===============================================

-- 1️⃣ ProductMaster Sample Data
INSERT INTO ProductMaster 
(ProductName, GenericName, ProductGroup_List, ProductGroup_Txt, PurchaseUnit_List, PurchaseUnit_Txt, Packing, StockUnit_List, StockUnit_Txt, ProductType_List, ProductType_Txt, Remarks, MinStock, MaxStock, IsActive)
VALUES
('Paracetamol 500mg', 'Paracetamol', 'Analgesic', 'Painkiller', 'Tablet', 'Tablet', 10, 'Tablet', 'Tablet', 'Allopathic', 'Allopathic', 'Used for fever & pain', 50, 500, 1),
('Amoxicillin 250mg', 'Amoxicillin', 'Antibiotic', 'Penicillin', 'Capsule', 'Capsule', 10, 'Capsule', 'Capsule', 'Allopathic', 'Allopathic', 'Bacterial infection', 20, 200, 1),
('Cetirizine 10mg', 'Cetirizine', 'Antihistamine', 'Allergy', 'Tablet', 'Tablet', 10, 'Tablet', 'Tablet', 'Allopathic', 'Allopathic', 'For allergy & cold', 30, 300, 1),
('Metformin 500mg', 'Metformin', 'Antidiabetic', 'Diabetes', 'Tablet', 'Tablet', 10, 'Tablet', 'Tablet', 'Allopathic', 'Allopathic', 'For type 2 diabetes', 25, 250, 1),
('Omeprazole 20mg', 'Omeprazole', 'Proton Pump Inhibitor', 'Acid Reducer', 'Capsule', 'Capsule', 10, 'Capsule', 'Capsule', 'Allopathic', 'Allopathic', 'For acidity & ulcer', 20, 200, 1);

-- 2️⃣ SupplierMaster Sample Data
INSERT INTO SupplierMaster
(SupplierName, ContactPerson, MobileNo, Email, Address, GSTNo, IsActive)
VALUES
('MediSupply Pvt Ltd', 'Ramesh Kumar', '9876543210', 'ramesh@medisupply.com', '12 Gandhi Street, Chennai', 'GSTTN001', 1),
('HealthPharma Ltd', 'Priya Singh', '9876501234', 'priya@healthpharma.com', '45 Lake Road, Chennai', 'GSTTN002', 1),
('CareMed Distributors', 'S. Raj', '9876001122', 'raj@caremed.com', '22 Anna Nagar, Chennai', 'GSTTN003', 1),
('GlobalPharma', 'Meena Narayan', '9876123456', 'meena@globalpharma.com', '88 Park View, Chennai', 'GSTTN004', 1),
('LifeLine Suppliers', 'Aravind Ravi', '9876332211', 'aravind@lifeline.com', '77 MG Road, Chennai', 'GSTTN005', 1);

-- 3️⃣ ProductSupplier Sample Data
INSERT INTO ProductSupplier
(ProductCode, SupplierId, PurchaseRate, PurchaseDiscount, SaleRate, SaleDiscount, TaxPercent, MRP)
VALUES
(1, 1, 25.00, 0, 30.00, 0, 5.0, 35.00),
(1, 2, 24.50, 0, 29.50, 0, 5.0, 35.00),
(2, 2, 50.00, 0, 60.00, 0, 12.0, 70.00),
(2, 3, 48.00, 0, 58.00, 0, 12.0, 70.00),
(3, 3, 15.00, 0, 20.00, 0, 5.0, 25.00),
(4, 4, 30.00, 0, 35.00, 0, 5.0, 40.00),
(5, 5, 45.00, 0, 50.00, 0, 12.0, 60.00),
(3, 1, 14.50, 0, 19.50, 0, 5.0, 25.00);
