USE [ClaimBuddy];
GO

set identity_insert [Category] on
insert into [Category]
	([Id], [Name])
values
	(1, 'Other'),
	(2, 'Electronics'),
	(3, 'Jewelry'),
	(4, 'Appliances')
set identity_insert [Category] off

set identity_insert [UserProfile] on
insert into UserProfile
	(Id, FirstName, LastName, Email, CreateDateTime, FirebaseUserId)
values
	(1, 'Chris', 'LastName', 'test@test.com', '2020-10-27', 'YDdsnrAa2Db8yoju0fM3eB0Zr4W2');
set identity_insert [UserProfile] off

set identity_insert [Item] on
insert into Item
	(Id, Name, Notes, Price, CreateDateTime, PurchaseDateTime, IsDeleted, CategoryId, UserProfileId, Image, ReceiptImage, Model)
values
	(1, 'TCL TV', '4K, 55in', 399.99, '2019-08-01', '2018-12-04', 0, 2, 1, 'tv.jpg', 'receipt.png', 'TV123-S');
insert into Item
	(Id, Name, Notes, Price, CreateDateTime, PurchaseDateTime, IsDeleted, CategoryId, UserProfileId, Image, ReceiptImage, Model)
values
	(2, 'LG Washer', 'White, front-load', 799.99, '2019-08-01', '2020-03-04', 0, 4, 1, 'washer.jpg', 'receipt.png', 'WM3343');
set identity_insert [Item] off