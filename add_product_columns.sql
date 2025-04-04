-- Add new columns to Products table for cafe functionality
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'IsAvailable')
BEGIN
    ALTER TABLE Products ADD IsAvailable bit NOT NULL DEFAULT 1;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'CanCustomize')
BEGIN
    ALTER TABLE Products ADD CanCustomize bit NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'HasSizeOptions')
BEGIN
    ALTER TABLE Products ADD HasSizeOptions bit NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'PrepTime')
BEGIN
    ALTER TABLE Products ADD PrepTime nvarchar(max) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = 'IsFeatured')
BEGIN
    ALTER TABLE Products ADD IsFeatured bit NOT NULL DEFAULT 0;
END

PRINT 'Product table columns have been updated successfully!' 