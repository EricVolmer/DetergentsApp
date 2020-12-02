
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/27/2020 14:24:19
-- Generated from EDMX file: C:\Users\Eric\Documents\GitHub\DetergentsApp\DetergentsApp\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Detergents];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Product__categor__025D5595]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK__Product__categor__025D5595];
GO
IF OBJECT_ID(N'[dbo].[FK__UserFile__sheetT__0539C240]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFile] DROP CONSTRAINT [FK__UserFile__sheetT__0539C240];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[SheetType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SheetType];
GO
IF OBJECT_ID(N'[dbo].[UserFile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFile];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [categoryID] int IDENTITY(1,1) NOT NULL,
    [categoryName] nvarchar(50)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [productID] int IDENTITY(1,1) NOT NULL,
    [EAN] bigint  NOT NULL,
    [sheetTypeID] int  NOT NULL,
    [productName] nvarchar(50)  NOT NULL,
    [productDescription] nvarchar(150)  NULL,
    [categoryID] int  NOT NULL
);
GO

-- Creating table 'SheetTypes'
CREATE TABLE [dbo].[SheetTypes] (
    [sheetTypeID] int  NOT NULL,
    [sheetTypeName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'UserFiles'
CREATE TABLE [dbo].[UserFiles] (
    [fileID] int IDENTITY(1,1) NOT NULL,
    [fileName] varchar(50)  NOT NULL,
    [fileData] varbinary(max)  NOT NULL,
    [productID] int  NOT NULL,
    [sheetTypeID] int  NOT NULL
);
GO

-- Creating table 'ProductSheetType'
CREATE TABLE [dbo].[ProductSheetType] (
    [Product_productID] int  NOT NULL,
    [SheetType_sheetTypeID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [categoryID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([categoryID] ASC);
GO

-- Creating primary key on [productID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([productID] ASC);
GO

-- Creating primary key on [sheetTypeID] in table 'SheetTypes'
ALTER TABLE [dbo].[SheetTypes]
ADD CONSTRAINT [PK_SheetTypes]
    PRIMARY KEY CLUSTERED ([sheetTypeID] ASC);
GO

-- Creating primary key on [fileID] in table 'UserFiles'
ALTER TABLE [dbo].[UserFiles]
ADD CONSTRAINT [PK_UserFiles]
    PRIMARY KEY CLUSTERED ([fileID] ASC);
GO

-- Creating primary key on [Product_productID], [SheetType_sheetTypeID] in table 'ProductSheetType'
ALTER TABLE [dbo].[ProductSheetType]
ADD CONSTRAINT [PK_ProductSheetType]
    PRIMARY KEY CLUSTERED ([Product_productID], [SheetType_sheetTypeID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [categoryID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK__Product__categor__025D5595]
    FOREIGN KEY ([categoryID])
    REFERENCES [dbo].[Categories]
        ([categoryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Product__categor__025D5595'
CREATE INDEX [IX_FK__Product__categor__025D5595]
ON [dbo].[Products]
    ([categoryID]);
GO

-- Creating foreign key on [sheetTypeID] in table 'UserFiles'
ALTER TABLE [dbo].[UserFiles]
ADD CONSTRAINT [FK__UserFile__sheetT__0539C240]
    FOREIGN KEY ([sheetTypeID])
    REFERENCES [dbo].[SheetTypes]
        ([sheetTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__UserFile__sheetT__0539C240'
CREATE INDEX [IX_FK__UserFile__sheetT__0539C240]
ON [dbo].[UserFiles]
    ([sheetTypeID]);
GO

-- Creating foreign key on [Product_productID] in table 'ProductSheetType'
ALTER TABLE [dbo].[ProductSheetType]
ADD CONSTRAINT [FK_ProductSheetType_Product]
    FOREIGN KEY ([Product_productID])
    REFERENCES [dbo].[Products]
        ([productID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SheetType_sheetTypeID] in table 'ProductSheetType'
ALTER TABLE [dbo].[ProductSheetType]
ADD CONSTRAINT [FK_ProductSheetType_SheetType]
    FOREIGN KEY ([SheetType_sheetTypeID])
    REFERENCES [dbo].[SheetTypes]
        ([sheetTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductSheetType_SheetType'
CREATE INDEX [IX_FK_ProductSheetType_SheetType]
ON [dbo].[ProductSheetType]
    ([SheetType_sheetTypeID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------