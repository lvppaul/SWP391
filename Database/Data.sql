﻿use KoiCareSystemAtHome
go
--  [User] table
INSERT INTO [User] (UserID, UserName, FullName, Password, Phone, Sex, Email, Street, District, City, Country, Role, isActive)
VALUES 
('U001', 'Tri', 'Huynh Cong Tri', '123', '1234567890', 'Male', 'tri@example.com', '123 Elm St', 'Downtown', 'New York', 'USA', 0, 1),
('U002', 'Kiet', 'Nguyen Tuan Kiet', '123', '0987654321', 'Male', 'kiet@example.com', '456 Oak St', 'Midtown', 'Los Angeles', 'USA', 1, 1),
('U003', 'Thang', 'Nguyen Duc Thang', '123', '1231231234', 'Male', 'Thang@example.com', '789 Pine St', 'Uptown', 'Chicago', 'USA', 0, 1),
('U004', 'Quan', 'Luu Minh Quan', '123', '3213214321', 'Male', 'Quan@example.com', '101 Birch St', 'Old Town', 'Houston', 'USA', 0, 1),
('A006', 'Phat', 'Le Vinh Phat', '123', '7897899876', 'Male', 'Phat@example.com', '303 Cedar St', 'East Side', 'Seattle', 'USA', 0, 1);
go
--  Pond table
INSERT INTO Pond (PondID, UserID, Name, Volume, Depth, PumpingCapacity, Drain, Skimmer, Note, Thumbnail)
VALUES 
('P001', 'U001', 'Alpha Pond', 5000.5, 15, 200, 1, 1, 'Located near the east wing.', 'https://www.lawnstarter.com/blog/wp-content/uploads/2022/12/Koi_Pond_in_MUST.jpg'),
('P002', 'U002', 'Beta Pond', 7500.75, 20, 300, 1, 0, 'For fish farming.', 'https://premierpond.com/wp-content/uploads/2023/03/Beautiful-and-healthy-koi-pond.webp'),
('P003', 'U003', 'Gamma Pond', 6000.0, 18, 250, 1, 1, 'Contains ornamental fish.', 'https://www.lawnstarter.com/blog/wp-content/uploads/2023/02/25074502855_e63aafe6c1_o-1.jpg'),
('P004', 'U004', 'Delta Pond', 8000.25, 22, 350, 0, 0, 'Main irrigation pond.', 'https://nualgiponds.com/wp-content/uploads/2014/05/image-6.jpg'),
('P005', 'U004', 'Epsilon Pond', 4500.5, 12, 180, 1, 1, 'Located next to the storage area.', 'https://www.grassrootspondandgarden.com/wp-content/uploads/2022/10/How-to-build-a-koi-pond-like-this.jpeg'),
('P006', 'U004', 'Zeta Pond', 9000.9, 25, 400, 1, 0, 'Newly constructed for water reserve.', 'https://gradexco.com/wp-content/uploads/2021/12/Ecosystem-koi-pond-that-is-a-good-size.jpeg');
go
-- Category
INSERT INTO Category (Name, Description)
VALUES 
('Antibacterial Medications', 'Medications designed to treat bacterial infections in Koi fish, such as ulcers and fin rot.'),
('Antifungal Treatments', 'Treatments for fungal infections that commonly affect Koi fish, particularly during colder months.'),
('Parasitic Treatments', 'Medications for eliminating parasites like flukes, anchor worms, and ich in Koi ponds.'),
('Water Conditioners with Medication', 'Conditioners that treat water while simultaneously medicating fish for stress relief and healing.'),
('Antibiotics', 'Specialized antibiotics to combat internal and external infections in Koi fish.'),
('Wound Care and Healing Products', 'Products specifically for treating open wounds and injuries, promoting faster recovery for Koi fish.');
go
--Product 
INSERT INTO Product (ProductID, Name, Description, Quantity, Price, Status, CategoryID, UserID)
VALUES
('P001', 'Koi Fish Antibacterial Treatment', 'Effective treatment for bacterial infections in Koi fish, including ulcers and fin rot.', 50, 25.99, 1, 1, 'U004'),
('P002', 'Koi Fish Antifungal Solution', 'Fungal treatment designed to prevent and cure fungal diseases in Koi fish.', 30, 18.50, 1, 2, 'U004'),
('P003', 'Koi Parasitic Removal Medication', 'Eliminates common parasites such as flukes and anchor worms, safe for Koi fish.', 40, 22.75, 1, 3, 'U004'),
('P004', 'Koi Pond Water Conditioner & Medication', 'Conditioner that stabilizes water quality while also treating Koi for stress and injuries.', 25, 15.99, 1, 4, 'U004'),
('P005', 'Broad-Spectrum Koi Fish Antibiotics', 'Broad-spectrum antibiotics to combat both internal and external bacterial infections.', 60, 35.00, 1, 5, 'U004'),
('P006', 'Koi Fish Wound Healing Gel', 'A healing gel specifically formulated for treating wounds and injuries in Koi fish.', 20, 12.49, 1, 3, 'U001');
go
--  Product_Image 
INSERT INTO Product_Image (ProductID, ImageUrl)
VALUES
('P001', 'https://i.ebayimg.com/images/g/ElkAAOSwxXFl-ft-/s-l1200.png'),
('P002', 'https://www.koipharma.com/cdn/shop/files/KoiFishParasiteTreatment-KoiPharma_1024x1024.png?v=1719717065'),
('P003', 'https://www.absolute-koi.com/media/catalog/product/c/l/cloverleaf_absolute_parasite_plus.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=426&width=426&canvas=426:426'),
('P004', 'https://www.koipharma.com/cdn/shop/files/KoiDefenderPathogenicBacterialControl-PowerfulBacterialMedication_176x.jpg?v=1721438339'),
('P005', 'https://koimarket.com/cdn/shop/products/ProformC.png?v=1659625450&width=360'),
('P006', 'https://www.absolute-koi.com/media/catalog/product/k/o/koi-clear-gel.png?optimize=medium&bg-color=255,255,255&fit=bounds&height=660&width=660&canvas=660:660&format=jpeg');
go
-- Shop 
INSERT INTO Shop (UserID, ShopName, Thumbnail, Description, Phone, Email, Rating)
VALUES
('U004', 'Koi Fish First Aid', 'https://i.pinimg.com/236x/59/06/32/5906320f53564acf6d72e65df577e16a.jpg', 'Specialized in emergency care and wound treatment for Koi fish.', '4564564567', 'shop5@example.com', 4.9);
go
--  Koi table
INSERT INTO Koi (KoiID, UserID, PondID, Age, Name, Note, Origin, Length, Weight, Color, Status)
VALUES 
('K001', 'U001', 'P001', 3, 'Sakura', 'Imported from Japan, Showa variety.', 'Japan', 45, 12, 'Red, Black, White', 1),
('K002', 'U001', 'P001', 5, 'Dragon', 'Sanke variety, highly active fish.', 'USA', 50, 15, 'Black, White', 1),
('K003', 'U001', 'P001', 2, 'Pearl', 'Breeder: Niigata Koi Farm.', 'Japan', 40, 10, 'White, Yellow', 1),
('K004', 'U002', 'P002', 4, 'Emperor', 'Tancho Koi, highly prized for its head marking.', 'China', 55, 18, 'White, Red', 1),
('K005', 'U002', 'P002', 1, 'Goldie', 'Young Koi with vibrant golden colors.', 'Thailand', 35, 8, 'Gold, Yellow', 1),
('K006', 'U002', 'P002', 6, 'Shadow', 'Shiro Utsuri variety, good health condition.', 'Japan', 60, 20, 'Black, White', 1);

go
--  Koi_Record table
INSERT INTO Koi_Record (UserID, KoiID, Weight, Length, UpdatedTime)
VALUES 
('U001', 'K001', 12, 45, GETDATE()),
('U001', 'K001', 15, 50, GETDATE()),
('U001', 'K002', 10, 40, GETDATE()),
('U001', 'K003', 18, 55, GETDATE()),
('U001', 'K003', 8, 35, GETDATE()),
('U001', 'K003', 20, 60, GETDATE());
go
--  Koi_Image table
INSERT INTO Koi_Image (KoiID, Url)
VALUES 
('K001', 'https://koishop.vn/images/image.php?width=270&image=/admin/sanpham/5_9326_anh1.jpg'),
('K002', 'https://koishop.vn/images/image.php?width=270&image=/admin/sanpham/1_9329_anh1.jpg'),
('K003', 'https://koishop.vn/images/image.php?width=270&image=/admin/sanpham/s1029n008-2_9330_anh1.jpg'),
('K004', 'https://koishop.vn/images/image.php?width=270&image=/admin/sanpham/5_9326_anh1.jpg'),
('K005', 'https://koishop.vn/images/image.php?width=270&image=/admin/sanpham/1_9323_anh1.jpg'),
('K006', 'https://koishop.vn/images/image.php?width=270&image=/admin/sanpham/1_9328_anh1.jpg');
go
-- Blogs table
INSERT INTO Blogs (UserID, PublishDate, Content, Title)
VALUES 
('U001', '2023-10-01', 'This article guides how to care for Koi fish in the summer, including temperature control and nutrition.', 'Summer Care for Koi Fish'),
('U001', '2023-10-02', 'Learn about common diseases in Koi fish and effective treatment methods.', 'Common Diseases in Koi Fish'),
('U001', '2023-10-03', 'How to build a Koi pond from scratch, from choosing a location to installing the filtration system.', 'Building a Koi Pond'),
('U002', '2023-10-04', 'Tips and tricks to maintain clean and clear water in your Koi pond.', 'Maintaining Clean Water for Koi Ponds'),
('U003', '2023-10-05', 'Explore popular Koi varieties and their unique characteristics.', 'Popular Koi Varieties'),
('U003', '2023-10-06', 'A guide on how to feed Koi fish properly to ensure their health and growth.', 'Feeding Koi Fish the Right Way');
go
-- PaymentMethod table
INSERT INTO PaymentMethod (PaymentName)
VALUES 
('Credit Card'),
('PayPal'),
('Bank Transfer'),
('Cash on Delivery'),
('Digital Wallet');
go
-- Water_Parameter
INSERT INTO Water_Parameter (UserID, PondID, Nitrite, Oxygen, Nitrate, [DateTime], Temperature, Phosphate, pH, Ammonium, Hardness, CarbonDioxide, CarbonHardness, Salt, TotalChlorines, OutdoorTemp, AmountFed)
VALUES 
('U001', 'P001', 0.1, 6.5, 5.0, '2024-10-01 08:00:00', 24.5, 0.2, 7.5, 0.03, 150, 10, 2.0, 0.5, 0.1, 30.0, 1.0),
('U002', 'P002', 0.2, 7.0, 4.8, '2024-10-02 09:00:00', 25.0, 0.3, 7.2, 0.04, 160, 12, 1.8, 0.6, 0.2, 28.0, 1.5),
('U003', 'P003', 0.15, 6.8, 4.5, '2024-10-03 10:00:00', 23.0, 0.25, 7.6, 0.02, 140, 11, 1.5, 0.4, 0.15, 29.0, 2.0),
('U004', 'P004', 0.05, 7.2, 5.5, '2024-10-04 11:00:00', 26.5, 0.1, 7.4, 0.01, 155, 9, 2.5, 0.3, 0.25, 31.0, 0.8),
('U004', 'P005', 0.12, 6.9, 5.2, '2024-10-05 12:00:00', 24.0, 0.22, 7.1, 0.03, 145, 13, 1.7, 0.5, 0.1, 27.0, 1.2),
('U004', 'P006', 0.08, 7.5, 4.9, '2024-10-06 13:00:00', 22.0, 0.18, 7.3, 0.02, 135, 8, 2.2, 0.6, 0.3, 26.0, 1.7);
go
-- News
INSERT INTO News (Title, PublishDate, Content) 
VALUES 
('The Growth of Koi Fish in Vietnam', '2024-10-01', 'Koi fish are becoming increasingly popular in Vietnam, with many enthusiasts investing in breeding and caring for this species.'),
('International Koi Fish Competition in Japan', '2024-09-25', 'The world’s largest koi fish competition recently took place in Japan, attracting thousands of enthusiasts from around the globe.'),
('The Spiritual Meaning of Koi Fish', '2024-09-28', 'Koi fish are often seen as a symbol of perseverance, strength, and good fortune, especially in Japanese and Chinese cultures.'),
('Caring for Koi Fish: Best Practices', '2024-10-03', 'Proper care and maintenance are essential for koi fish, including maintaining water quality, diet, and regular health checks.'),
('Koi Fish: A Symbol of Wealth and Prosperity', '2024-09-20', 'In many cultures, koi fish are associated with wealth and prosperity, making them a popular choice for ornamental ponds and gardens.'),
('The Science Behind Koi Fish Coloration', '2024-09-18', 'Researchers are studying the genetics behind the vibrant colors of koi fish, hoping to better understand the factors that influence their patterns and hues.');
go
-- Koi_Remind
INSERT INTO Koi_Remind (UserID, KoiID, RemindDescription, DateRemind)
VALUES 
('U001', 'K001', N'Nhắc nhở kiểm tra sức khỏe hàng tuần.', '2024-10-15 09:00:00'),
('U001', 'K002', N'Theo dõi cân nặng và chiều dài koi.', '2024-10-20 14:30:00'),
('U002', 'K003', N'Nhắc cho ăn thức ăn đặc biệt.', '2024-10-18 07:00:00'),
('U003', 'K004', N'Kiểm tra nước ao vào thứ bảy hàng tuần.', '2024-10-19 10:00:00'),
('U001', 'K005', N'Thuốc trị bệnh cho koi cần được bổ sung.', '2024-10-21 16:00:00');
go
-- Blog_Image
INSERT INTO Blog_Image (ImageUrl, BlogID)
VALUES 
(N'https://example.com/images/koi1.jpg', 1),
(N'https://example.com/images/koi2.jpg', 1),
(N'https://example.com/images/koi3.jpg', 2),
(N'https://example.com/images/koi4.jpg', 3),
(N'https://example.com/images/koi5.jpg', 3);
go
-- News_Image
INSERT INTO News_Image (ImageUrl, NewsID)
VALUES 
(N'https://example.com/images/news1.jpg', 1),
(N'https://example.com/images/news2.jpg', 1),
(N'https://example.com/images/news3.jpg', 2),
(N'https://example.com/images/news4.jpg', 3),
(N'https://example.com/images/news5.jpg', 3);
