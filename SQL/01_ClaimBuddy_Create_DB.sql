USE [master]

IF db_id('ClaimBuddy') IS NULL
  CREATE DATABASE [ClaimBuddy]
GO

USE [ClaimBuddy]
GO

DROP TABLE IF EXISTS [UserProfile];
DROP TABLE IF EXISTS [Item];
DROP TABLE IF EXISTS [List];
DROP TABLE IF EXISTS [ListItem];
DROP TABLE IF EXISTS [Category];
GO

CREATE TABLE [UserProfile]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [FirebaseUserId] nvarchar(28) NOT NULL,
  [FirstName] nvarchar(50) NOT NULL,
  [LastName] nvarchar(50) NOT NULL,
  [Email] nvarchar(100) NOT NULL,
  [CreateDateTime] datetime NOT NULL,

  CONSTRAINT UQ_FirebaseUserId UNIQUE(FirebaseUserId),
  CONSTRAINT UQ_Email UNIQUE(Email)
)

CREATE TABLE [Category]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(50) NOT NULL
)

CREATE TABLE [Item]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(255) NOT NULL,
  [Notes] text,
  [Price] money NOT NULL,
  [Model] nvarchar(100),
  [ReceiptImage] nvarchar(255),
  [Image] nvarchar(255),
  [PurchaseDateTime] datetime NOT NULL,
  [CreateDateTime] datetime NOT NULL,
  [IsDeleted] bit NOT NULL DEFAULT 0,
  [CategoryId] integer NOT NULL,
  [UserProfileId] integer NOT NULL,

  CONSTRAINT [FK_Item_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]),
  CONSTRAINT [FK_Item_UserProfile] FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
)

CREATE TABLE [List]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(50) NOT NULL,
  [UserProfileId] integer NOT NULL,

  CONSTRAINT [FK_List_UserProfileId] FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
)

CREATE TABLE [ListItem]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [ItemId] integer NOT NULL,
  [ListId] integer NOT NULL,

  CONSTRAINT [FK_ListItem_List] FOREIGN KEY ([ListId]) REFERENCES [List] ([Id]),
  CONSTRAINT [FK_ListItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [Item] ([Id])
)
GO   