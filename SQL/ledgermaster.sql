CREATE TABLE `ledgermaster` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ledger_type` enum('Company','Supplier','Customer') NOT NULL,
  `ledger_name` varchar(255) NOT NULL,
  `address` text,
  `place` varchar(100) DEFAULT NULL,
  `gstin` varchar(20) DEFAULT NULL,
  `gst_state` varchar(50) DEFAULT NULL,
  `credit_days` int DEFAULT '0',
  `contact_person` varchar(100) DEFAULT NULL,
  `phone_no` varchar(15) DEFAULT NULL,
  `mobile_no` varchar(15) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `drug_license_no` varchar(50) DEFAULT NULL,
  `remarks` text,
  `is_active` tinyint(1) DEFAULT '1',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

