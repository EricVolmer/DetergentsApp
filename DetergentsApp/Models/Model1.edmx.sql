
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/26/2021 12:01:24
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
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK__Product__categor__025D5595];
GO
IF OBJECT_ID(N'[dbo].[FK__UserFile__sheetT__0539C240]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserFiles] DROP CONSTRAINT [FK__UserFile__sheetT__0539C240];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductSheetType_Product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductSheetType] DROP CONSTRAINT [FK_ProductSheetType_Product];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductSheetType_SheetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductSheetType] DROP CONSTRAINT [FK_ProductSheetType_SheetType];
GO
IF OBJECT_ID(N'[dbo].[FK_VendorProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_VendorProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_CountryProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_CountryProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_StoreProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_articleDetailsProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK_articleDetailsProduct];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[SheetTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SheetTypes];
GO
IF OBJECT_ID(N'[dbo].[UserFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserFiles];
GO
IF OBJECT_ID(N'[dbo].[Vendor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vendor];
GO
IF OBJECT_ID(N'[dbo].[Country]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Country];
GO
IF OBJECT_ID(N'[dbo].[StoreAPI]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StoreAPI];
GO
IF OBJECT_ID(N'[dbo].[articleDetailsAPI]', 'U') IS NOT NULL
    DROP TABLE [dbo].[articleDetailsAPI];
GO
IF OBJECT_ID(N'[dbo].[vendorLogin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vendorLogin];
GO
IF OBJECT_ID(N'[dbo].[ProductSheetType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductSheetType];
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
    [EAN] nvarchar(max)  NOT NULL,
    [sheetTypeID] int  NOT NULL,
    [productDescription] nvarchar(150)  NULL,
    [categoryID] int  NOT NULL,
    [articleNumber] int  NOT NULL,
    [vendorID] int  NOT NULL,
    [countryID] int  NOT NULL,
    [storeID] int  NOT NULL,
    [articleID] bigint  NOT NULL,
    [adminToPublic] bit  NOT NULL
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
    [productID] int  NOT NULL,
    [sheetTypeID] int  NOT NULL,
    [vendorID] int  NULL,
    [adminApproved] bit  NOT NULL,
    [languageType] nvarchar(max)  NOT NULL,
    [oldFile] bit  NOT NULL
);
GO

-- Creating table 'Vendor'
CREATE TABLE [dbo].[Vendor] (
    [vendorID] int  NOT NULL,
    [vendorName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Country'
CREATE TABLE [dbo].[Country] (
    [CountryID] int IDENTITY(1,1) NOT NULL,
    [CountryName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'StoreAPI'
CREATE TABLE [dbo].[StoreAPI] (
    [storeID] int IDENTITY(1,1) NOT NULL,
    [storeName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'articleDetailsAPI'
CREATE TABLE [dbo].[articleDetailsAPI] (
    [articleID] bigint IDENTITY(1,1) NOT NULL,
    [articleTextReceipt] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'vendorLogin'
CREATE TABLE [dbo].[vendorLogin] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [userName] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL
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

-- Creating primary key on [vendorID] in table 'Vendor'
ALTER TABLE [dbo].[Vendor]
ADD CONSTRAINT [PK_Vendor]
    PRIMARY KEY CLUSTERED ([vendorID] ASC);
GO

-- Creating primary key on [CountryID] in table 'Country'
ALTER TABLE [dbo].[Country]
ADD CONSTRAINT [PK_Country]
    PRIMARY KEY CLUSTERED ([CountryID] ASC);
GO

-- Creating primary key on [storeID] in table 'StoreAPI'
ALTER TABLE [dbo].[StoreAPI]
ADD CONSTRAINT [PK_StoreAPI]
    PRIMARY KEY CLUSTERED ([storeID] ASC);
GO

-- Creating primary key on [articleID] in table 'articleDetailsAPI'
ALTER TABLE [dbo].[articleDetailsAPI]
ADD CONSTRAINT [PK_articleDetailsAPI]
    PRIMARY KEY CLUSTERED ([articleID] ASC);
GO

-- Creating primary key on [Id] in table 'vendorLogin'
ALTER TABLE [dbo].[vendorLogin]
ADD CONSTRAINT [PK_vendorLogin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating foreign key on [vendorID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_VendorProduct]
    FOREIGN KEY ([vendorID])
    REFERENCES [dbo].[Vendor]
        ([vendorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VendorProduct'
CREATE INDEX [IX_FK_VendorProduct]
ON [dbo].[Products]
    ([vendorID]);
GO

-- Creating foreign key on [countryID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_CountryProduct]
    FOREIGN KEY ([countryID])
    REFERENCES [dbo].[Country]
        ([CountryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CountryProduct'
CREATE INDEX [IX_FK_CountryProduct]
ON [dbo].[Products]
    ([countryID]);
GO

-- Creating foreign key on [storeID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_StoreProduct]
    FOREIGN KEY ([storeID])
    REFERENCES [dbo].[StoreAPI]
        ([storeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreProduct'
CREATE INDEX [IX_FK_StoreProduct]
ON [dbo].[Products]
    ([storeID]);
GO

-- Creating foreign key on [articleID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK_articleDetailsProduct]
    FOREIGN KEY ([articleID])
    REFERENCES [dbo].[articleDetailsAPI]
        ([articleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_articleDetailsProduct'
CREATE INDEX [IX_FK_articleDetailsProduct]
ON [dbo].[Products]
    ([articleID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------