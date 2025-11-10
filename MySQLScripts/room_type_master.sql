CREATE TABLE room_type_master (
    type_id INT AUTO_INCREMENT PRIMARY KEY,
    type_name VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO room_type_master (type_name) VALUES
('Single'),
('ICCU'),
('Special'),
('VIP'),
('Emergency'),
('General Ward');
