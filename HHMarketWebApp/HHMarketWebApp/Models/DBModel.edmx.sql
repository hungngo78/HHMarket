
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/30/2018 21:48:42
-- Generated from EDMX file: C:\Users\them\Documents\GitHub\hungngo78\master\HHMarket\HHMarketWebApp\HHMarketWebApp\Models\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HHMarketDB2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MainCategoryCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Category] DROP CONSTRAINT [FK_MainCategoryCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_CategoryProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_UserProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDetails] DROP CONSTRAINT [FK_ProductProductDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCart]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cart] DROP CONSTRAINT [FK_UserCart];
GO
IF OBJECT_ID(N'[dbo].[FK_CartCartDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartDetails] DROP CONSTRAINT [FK_CartCartDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDetailsCartDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartDetails] DROP CONSTRAINT [FK_ProductDetailsCartDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_UserOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Order] DROP CONSTRAINT [FK_UserOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderOrderDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK_OrderOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDetailsOrderDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK_ProductDetailsOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_UserReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Review] DROP CONSTRAINT [FK_UserReview];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Review] DROP CONSTRAINT [FK_ProductReview];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[MainCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MainCategory];
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[ProductDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductDetails];
GO
IF OBJECT_ID(N'[dbo].[Cart]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cart];
GO
IF OBJECT_ID(N'[dbo].[CartDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CartDetails];
GO
IF OBJECT_ID(N'[dbo].[Order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Order];
GO
IF OBJECT_ID(N'[dbo].[OrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderDetails];
GO
IF OBJECT_ID(N'[dbo].[Review]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Review];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [UserId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [State] nvarchar(max)  NOT NULL,
    [ZipCode] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MainCategory'
CREATE TABLE [dbo].[MainCategory] (
    [MainCategoryId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Category'
CREATE TABLE [dbo].[Category] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [MainCategoryId] int  NOT NULL
);
GO

-- Creating table 'Product'
CREATE TABLE [dbo].[Product] (
    [ProductId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CategoryId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'ProductDetails'
CREATE TABLE [dbo].[ProductDetails] (
    [ProductDetailsId] int IDENTITY(1,1) NOT NULL,
    [Size] nvarchar(max)  NOT NULL,
    [Color] nvarchar(max)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [Amount] smallint  NOT NULL,
    [ProductId] int  NOT NULL
);
GO

-- Creating table 'Cart'
CREATE TABLE [dbo].[Cart] (
    [CartId] int IDENTITY(1,1) NOT NULL,
    [DateOpen] datetime  NOT NULL,
    [UserId] int  NULL
);
GO

-- Creating table 'CartDetails'
CREATE TABLE [dbo].[CartDetails] (
    [CartDetailsId] int IDENTITY(1,1) NOT NULL,
    [Amount] smallint  NOT NULL,
    [ExtendedPrice] decimal(18,0)  NOT NULL,
    [Type] smallint  NOT NULL,
    [ProductDetailsId] int  NOT NULL,
    [CartId] int  NOT NULL
);
GO

-- Creating table 'Order'
CREATE TABLE [dbo].[Order] (
    [OrderId] int IDENTITY(1,1) NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [Status] smallint  NOT NULL,
    [DeliveryDate] datetime  NOT NULL,
    [DeliveryFee] decimal(18,0)  NOT NULL,
    [Note] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [OrderDetailsId] int IDENTITY(1,1) NOT NULL,
    [Amount] smallint  NOT NULL,
    [ExtendedPrice] decimal(18,0)  NOT NULL,
    [OrderId] int  NOT NULL,
    [ProductDetailsId] int  NOT NULL
);
GO

-- Creating table 'Review'
CREATE TABLE [dbo].[Review] (
    [ReviewId] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [OverallRating] smallint  NOT NULL,
    [ReviewDate] datetime  NOT NULL,
    [UserId] int  NOT NULL,
    [ProductId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserId] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [MainCategoryId] in table 'MainCategory'
ALTER TABLE [dbo].[MainCategory]
ADD CONSTRAINT [PK_MainCategory]
    PRIMARY KEY CLUSTERED ([MainCategoryId] ASC);
GO

-- Creating primary key on [CategoryId] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [ProductId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [PK_Product]
    PRIMARY KEY CLUSTERED ([ProductId] ASC);
GO

-- Creating primary key on [ProductDetailsId] in table 'ProductDetails'
ALTER TABLE [dbo].[ProductDetails]
ADD CONSTRAINT [PK_ProductDetails]
    PRIMARY KEY CLUSTERED ([ProductDetailsId] ASC);
GO

-- Creating primary key on [CartId] in table 'Cart'
ALTER TABLE [dbo].[Cart]
ADD CONSTRAINT [PK_Cart]
    PRIMARY KEY CLUSTERED ([CartId] ASC);
GO

-- Creating primary key on [CartDetailsId] in table 'CartDetails'
ALTER TABLE [dbo].[CartDetails]
ADD CONSTRAINT [PK_CartDetails]
    PRIMARY KEY CLUSTERED ([CartDetailsId] ASC);
GO

-- Creating primary key on [OrderId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [PK_Order]
    PRIMARY KEY CLUSTERED ([OrderId] ASC);
GO

-- Creating primary key on [OrderDetailsId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([OrderDetailsId] ASC);
GO

-- Creating primary key on [ReviewId] in table 'Review'
ALTER TABLE [dbo].[Review]
ADD CONSTRAINT [PK_Review]
    PRIMARY KEY CLUSTERED ([ReviewId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MainCategoryId] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [FK_MainCategoryCategory]
    FOREIGN KEY ([MainCategoryId])
    REFERENCES [dbo].[MainCategory]
        ([MainCategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MainCategoryCategory'
CREATE INDEX [IX_FK_MainCategoryCategory]
ON [dbo].[Category]
    ([MainCategoryId]);
GO

-- Creating foreign key on [CategoryId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_CategoryProduct]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Category]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryProduct'
CREATE INDEX [IX_FK_CategoryProduct]
ON [dbo].[Product]
    ([CategoryId]);
GO

-- Creating foreign key on [UserId] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_UserProduct]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProduct'
CREATE INDEX [IX_FK_UserProduct]
ON [dbo].[Product]
    ([UserId]);
GO

-- Creating foreign key on [ProductId] in table 'ProductDetails'
ALTER TABLE [dbo].[ProductDetails]
ADD CONSTRAINT [FK_ProductProductDetails]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductDetails'
CREATE INDEX [IX_FK_ProductProductDetails]
ON [dbo].[ProductDetails]
    ([ProductId]);
GO

-- Creating foreign key on [UserId] in table 'Cart'
ALTER TABLE [dbo].[Cart]
ADD CONSTRAINT [FK_UserCart]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCart'
CREATE INDEX [IX_FK_UserCart]
ON [dbo].[Cart]
    ([UserId]);
GO

-- Creating foreign key on [CartId] in table 'CartDetails'
ALTER TABLE [dbo].[CartDetails]
ADD CONSTRAINT [FK_CartCartDetails]
    FOREIGN KEY ([CartId])
    REFERENCES [dbo].[Cart]
        ([CartId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CartCartDetails'
CREATE INDEX [IX_FK_CartCartDetails]
ON [dbo].[CartDetails]
    ([CartId]);
GO

-- Creating foreign key on [ProductDetailsId] in table 'CartDetails'
ALTER TABLE [dbo].[CartDetails]
ADD CONSTRAINT [FK_ProductDetailsCartDetails]
    FOREIGN KEY ([ProductDetailsId])
    REFERENCES [dbo].[ProductDetails]
        ([ProductDetailsId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDetailsCartDetails'
CREATE INDEX [IX_FK_ProductDetailsCartDetails]
ON [dbo].[CartDetails]
    ([ProductDetailsId]);
GO

-- Creating foreign key on [UserId] in table 'Order'
ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_UserOrder]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserOrder'
CREATE INDEX [IX_FK_UserOrder]
ON [dbo].[Order]
    ([UserId]);
GO

-- Creating foreign key on [OrderId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_OrderOrderDetails]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Order]
        ([OrderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderOrderDetails'
CREATE INDEX [IX_FK_OrderOrderDetails]
ON [dbo].[OrderDetails]
    ([OrderId]);
GO

-- Creating foreign key on [ProductDetailsId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_ProductDetailsOrderDetails]
    FOREIGN KEY ([ProductDetailsId])
    REFERENCES [dbo].[ProductDetails]
        ([ProductDetailsId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDetailsOrderDetails'
CREATE INDEX [IX_FK_ProductDetailsOrderDetails]
ON [dbo].[OrderDetails]
    ([ProductDetailsId]);
GO

-- Creating foreign key on [UserId] in table 'Review'
ALTER TABLE [dbo].[Review]
ADD CONSTRAINT [FK_UserReview]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserReview'
CREATE INDEX [IX_FK_UserReview]
ON [dbo].[Review]
    ([UserId]);
GO

-- Creating foreign key on [ProductId] in table 'Review'
ALTER TABLE [dbo].[Review]
ADD CONSTRAINT [FK_ProductReview]
    FOREIGN KEY ([ProductId])
    REFERENCES [dbo].[Product]
        ([ProductId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductReview'
CREATE INDEX [IX_FK_ProductReview]
ON [dbo].[Review]
    ([ProductId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------