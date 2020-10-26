USE [master]

IF db_id('ClaimBuddy') IS NULL
  CREATE DATABASE [ClaimBuddy]
GO

USE [ClaimBuddy]
GO

DROP TABLE IF EXISTS [UserProfile];
DROP TABLE IF EXISTS [Item];
DROP TABLE IF EXISTS [Claim];
DROP TABLE IF EXISTS [ClaimItem];
DROP TABLE IF EXISTS [Category];
GO

CREATE TABLE [UserProfile]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [FirebaseUserId] NVARCHAR(28) NOT NULL,
  [FirstName] nvarchar(50) NOT NULL,
  [LastName] nvarchar(50) NOT NULL,
  [Email] nvarchar(555) NOT NULL,
  [CreateDateTime] datetime NOT NULL

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
  [Image] nvarchar(255),
  [PurchaseDate] date NOT NULL,
  [CreateDateTime] datetime NOT NULL,
  [IsDeleted] bit NOT NULL DEFAULT 0,
  [CategoryId] integer,
  [UserProfileId] integer NOT NULL,

  CONSTRAINT [FK_Post_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]),
  CONSTRAINT [FK_Post_UserProfile] FOREIGN KEY ([UserProfileId]) REFERENCES [UserProfile] ([Id])
)

CREATE TABLE [Claim]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [Name] nvarchar(50) NOT NULL
)

CREATE TABLE [ClaimItem]
(
  [Id] integer PRIMARY KEY IDENTITY,
  [ItemId] integer NOT NULL,
  [ClaimId] integer NOT NULL,

  CONSTRAINT [FK_ClaimItem_Claim] FOREIGN KEY ([ClaimId]) REFERENCES [Claim] ([Id]) ON DELETE CASCADE,
  CONSTRAINT [FK_ClaimItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [Item] ([Id])
)
GO