USE [ClaimBuddy];
GO

set identity_insert [Category] on
insert into [Category]
	([Id], [Name])
values
	(1, 'Other'),
	(2, 'Electronics'),
	(3, 'Jewelry')
set identity_insert [Category] off

set identity_insert [UserProfile] on
insert into UserProfile
	(Id, FirstName, LastName, Email, CreateDateTime, FirebaseUserId)
values
	(1, 'Test', 'Last', 'test@test.com', '2020-04-23', 'jpuhyzaicsokywncxveknzowfpdu');
set identity_insert [UserProfile] off

set identity_insert [Item] on
insert into Item
	(Id, Name, Notes, Price, CreateDateTime, PurchaseDate, IsDeleted, CategoryId, UserProfileId, Image)
values
	(1, 'Test Item', 'Integer ac leo. Pellentesque ultrices mattis odio. Donec vitae nisi.', 19.99, '2019-08-01', '2018-12-04', 0, 1, 1, null);
set identity_insert [Item] off