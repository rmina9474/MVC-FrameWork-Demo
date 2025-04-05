-- Add new columns for guest checkout
ALTER TABLE [Orders] ADD [City] nvarchar(max) NULL;
ALTER TABLE [Orders] ADD [Email] nvarchar(max) NULL;
ALTER TABLE [Orders] ADD [FirstName] nvarchar(max) NULL;
ALTER TABLE [Orders] ADD [IsGuestOrder] bit NOT NULL DEFAULT CAST(0 AS bit);
ALTER TABLE [Orders] ADD [LastName] nvarchar(max) NULL;
ALTER TABLE [Orders] ADD [PhoneNumber] nvarchar(max) NULL;
ALTER TABLE [Orders] ADD [State] nvarchar(max) NULL;
ALTER TABLE [Orders] ADD [ZipCode] nvarchar(max) NULL;

-- Add entry to __EFMigrationsHistory to mark this migration as applied
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250404124717_AddGuestCheckoutFields', N'9.0.3');