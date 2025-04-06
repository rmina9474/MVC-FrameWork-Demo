CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "AspNetRoles" (
    "Id" nvarchar(450) NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY,
    "Name" nvarchar(256) NULL,
    "NormalizedName" nvarchar(256) NULL,
    "ConcurrencyStamp" nvarchar(max) NULL
);

CREATE TABLE "AspNetUsers" (
    "Id" nvarchar(450) NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
    "FullName" nvarchar(max) NOT NULL,
    "Address" nvarchar(max) NULL,
    "Age" nvarchar(max) NULL,
    "UserName" nvarchar(256) NULL,
    "NormalizedUserName" nvarchar(256) NULL,
    "Email" nvarchar(256) NULL,
    "NormalizedEmail" nvarchar(256) NULL,
    "EmailConfirmed" bit NOT NULL,
    "PasswordHash" nvarchar(max) NULL,
    "SecurityStamp" nvarchar(max) NULL,
    "ConcurrencyStamp" nvarchar(max) NULL,
    "PhoneNumber" nvarchar(max) NULL,
    "PhoneNumberConfirmed" bit NOT NULL,
    "TwoFactorEnabled" bit NOT NULL,
    "LockoutEnd" datetimeoffset NULL,
    "LockoutEnabled" bit NOT NULL,
    "AccessFailedCount" int NOT NULL
);

CREATE TABLE "Categories" (
    "Id" int NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY,
    "Name" nvarchar(50) NOT NULL
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" int NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY,
    "RoleId" nvarchar(450) NOT NULL,
    "ClaimType" nvarchar(max) NULL,
    "ClaimValue" nvarchar(max) NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" int NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY,
    "UserId" nvarchar(450) NOT NULL,
    "ClaimType" nvarchar(max) NULL,
    "ClaimValue" nvarchar(max) NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" nvarchar(450) NOT NULL,
    "ProviderKey" nvarchar(450) NOT NULL,
    "ProviderDisplayName" nvarchar(max) NULL,
    "UserId" nvarchar(450) NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" nvarchar(450) NOT NULL,
    "RoleId" nvarchar(450) NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" nvarchar(450) NOT NULL,
    "LoginProvider" nvarchar(450) NOT NULL,
    "Name" nvarchar(450) NOT NULL,
    "Value" nvarchar(max) NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Orders" (
    "Id" int NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY,
    "UserId" nvarchar(max) NOT NULL,
    "OrderDate" datetime2 NOT NULL,
    "TotalPrice" decimal(18,2) NOT NULL,
    "ShippingAddress" nvarchar(max) NOT NULL,
    "Notes" nvarchar(max) NOT NULL,
    "ApplicationUserId" nvarchar(450) NULL,
    CONSTRAINT "FK_Orders_AspNetUsers_ApplicationUserId" FOREIGN KEY ("ApplicationUserId") REFERENCES "AspNetUsers" ("Id")
);

CREATE TABLE "Products" (
    "Id" int NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY,
    "Name" nvarchar(100) NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "Description" nvarchar(max) NOT NULL,
    "ImageUrl" nvarchar(max) NULL,
    "CategoryId" int NOT NULL,
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OrderDetails" (
    "Id" int NOT NULL CONSTRAINT "PK_OrderDetails" PRIMARY KEY,
    "OrderId" int NOT NULL,
    "ProductId" int NOT NULL,
    "Quantity" int NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    CONSTRAINT "FK_OrderDetails_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderDetails_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ProductImages" (
    "Id" int NOT NULL CONSTRAINT "PK_ProductImages" PRIMARY KEY,
    "ImageUrl" nvarchar(max) NOT NULL,
    "ProductId" int NOT NULL,
    CONSTRAINT "FK_ProductImages_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ProductReviews" (
    "Id" int NOT NULL CONSTRAINT "PK_ProductReviews" PRIMARY KEY,
    "Rating" int NOT NULL,
    "Comment" nvarchar(500) NOT NULL,
    "CreatedAt" datetime2 NOT NULL,
    "UserId" nvarchar(450) NOT NULL,
    "ProductId" int NOT NULL,
    CONSTRAINT "FK_ProductReviews_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ProductReviews_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName") WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName") WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX "IX_OrderDetails_OrderId" ON "OrderDetails" ("OrderId");

CREATE INDEX "IX_OrderDetails_ProductId" ON "OrderDetails" ("ProductId");

CREATE INDEX "IX_Orders_ApplicationUserId" ON "Orders" ("ApplicationUserId");

CREATE INDEX "IX_ProductImages_ProductId" ON "ProductImages" ("ProductId");

CREATE INDEX "IX_ProductReviews_ProductId" ON "ProductReviews" ("ProductId");

CREATE INDEX "IX_ProductReviews_UserId" ON "ProductReviews" ("UserId");

CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250330090953_InitialCreate', '9.0.3');

CREATE TABLE "ef_temp_Orders" (
    "Id" int NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "ApplicationUserId" nvarchar(450) NOT NULL,
    "Notes" nvarchar(max) NOT NULL,
    "OrderDate" datetime2 NOT NULL,
    "ShippingAddress" nvarchar(max) NOT NULL,
    "TotalPrice" decimal(18,2) NOT NULL,
    "UserId" nvarchar(max) NOT NULL,
    CONSTRAINT "FK_Orders_AspNetUsers_ApplicationUserId" FOREIGN KEY ("ApplicationUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Orders" ("Id", "ApplicationUserId", "Notes", "OrderDate", "ShippingAddress", "TotalPrice", "UserId")
SELECT "Id", IFNULL("ApplicationUserId", ''), "Notes", "OrderDate", "ShippingAddress", "TotalPrice", "UserId"
FROM "Orders";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Orders";

ALTER TABLE "ef_temp_Orders" RENAME TO "Orders";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_Orders_ApplicationUserId" ON "Orders" ("ApplicationUserId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250330092149_AddApplicationUserIdToOrder', '9.0.3');

BEGIN TRANSACTION;
DROP INDEX "IX_Orders_ApplicationUserId";

CREATE INDEX "IX_Orders_UserId" ON "Orders" ("UserId");

CREATE TABLE "ef_temp_Orders" (
    "Id" int NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "Notes" nvarchar(max) NOT NULL,
    "OrderDate" datetime2 NOT NULL,
    "ShippingAddress" nvarchar(max) NOT NULL,
    "TotalPrice" decimal(18,2) NOT NULL,
    "UserId" nvarchar(450) NOT NULL,
    CONSTRAINT "FK_Orders_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Orders" ("Id", "Notes", "OrderDate", "ShippingAddress", "TotalPrice", "UserId")
SELECT "Id", "Notes", "OrderDate", "ShippingAddress", "TotalPrice", "UserId"
FROM "Orders";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Orders";

ALTER TABLE "ef_temp_Orders" RENAME TO "Orders";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_Orders_UserId" ON "Orders" ("UserId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404123706_UpdateModels', '9.0.3');

BEGIN TRANSACTION;
ALTER TABLE "Orders" ADD "City" nvarchar(max) NOT NULL DEFAULT '';

ALTER TABLE "Orders" ADD "Email" nvarchar(max) NOT NULL DEFAULT '';

ALTER TABLE "Orders" ADD "FirstName" nvarchar(max) NOT NULL DEFAULT '';

ALTER TABLE "Orders" ADD "IsGuestOrder" bit NOT NULL DEFAULT 0;

ALTER TABLE "Orders" ADD "LastName" nvarchar(max) NOT NULL DEFAULT '';

ALTER TABLE "Orders" ADD "PhoneNumber" nvarchar(max) NOT NULL DEFAULT '';

ALTER TABLE "Orders" ADD "State" nvarchar(max) NOT NULL DEFAULT '';

ALTER TABLE "Orders" ADD "ZipCode" nvarchar(max) NOT NULL DEFAULT '';

CREATE TABLE "ef_temp_Orders" (
    "Id" int NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "City" nvarchar(max) NOT NULL,
    "Email" nvarchar(max) NOT NULL,
    "FirstName" nvarchar(max) NOT NULL,
    "IsGuestOrder" bit NOT NULL,
    "LastName" nvarchar(max) NOT NULL,
    "Notes" nvarchar(max) NOT NULL,
    "OrderDate" datetime2 NOT NULL,
    "PhoneNumber" nvarchar(max) NOT NULL,
    "ShippingAddress" nvarchar(max) NOT NULL,
    "State" nvarchar(max) NOT NULL,
    "TotalPrice" decimal(18,2) NOT NULL,
    "UserId" nvarchar(450) NULL,
    "ZipCode" nvarchar(max) NOT NULL,
    CONSTRAINT "FK_Orders_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
);

INSERT INTO "ef_temp_Orders" ("Id", "City", "Email", "FirstName", "IsGuestOrder", "LastName", "Notes", "OrderDate", "PhoneNumber", "ShippingAddress", "State", "TotalPrice", "UserId", "ZipCode")
SELECT "Id", "City", "Email", "FirstName", "IsGuestOrder", "LastName", "Notes", "OrderDate", "PhoneNumber", "ShippingAddress", "State", "TotalPrice", "UserId", "ZipCode"
FROM "Orders";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Orders";

ALTER TABLE "ef_temp_Orders" RENAME TO "Orders";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_Orders_UserId" ON "Orders" ("UserId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404124717_AddGuestCheckoutFields', '9.0.3');

BEGIN TRANSACTION;
ALTER TABLE "Products" ADD "Brand" nvarchar(max) NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404134839_AddBrandToProduct', '9.0.3');

ALTER TABLE "AspNetUsers" ADD "AvatarUrl" nvarchar(max) NULL;

CREATE TABLE "ef_temp_Products" (
    "Id" int NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "CategoryId" int NOT NULL,
    "Description" nvarchar(max) NOT NULL,
    "ImageUrl" nvarchar(max) NULL,
    "Name" nvarchar(100) NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Products" ("Id", "CategoryId", "Description", "ImageUrl", "Name", "Price")
SELECT "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price"
FROM "Products";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;
DROP TABLE "Products";

ALTER TABLE "ef_temp_Products" RENAME TO "Products";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;
CREATE INDEX "IX_Products_CategoryId" ON "Products" ("CategoryId");

COMMIT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404144617_AddAvatarUrlToUser', '9.0.3');

BEGIN TRANSACTION;
ALTER TABLE "Products" ADD "CanCustomize" bit NOT NULL DEFAULT 0;

ALTER TABLE "Products" ADD "HasSizeOptions" bit NOT NULL DEFAULT 0;

ALTER TABLE "Products" ADD "IsAvailable" bit NOT NULL DEFAULT 0;

ALTER TABLE "Products" ADD "IsFeatured" bit NOT NULL DEFAULT 0;

ALTER TABLE "Products" ADD "PrepTime" nvarchar(max) NULL;

CREATE TABLE "ProductOptions" (
    "Id" int NOT NULL CONSTRAINT "PK_ProductOptions" PRIMARY KEY,
    "Name" nvarchar(max) NOT NULL,
    "AdditionalPrice" decimal(18,2) NOT NULL,
    "Description" nvarchar(max) NULL,
    "ProductId" int NOT NULL,
    "OptionType" nvarchar(max) NOT NULL,
    "IsDefault" bit NOT NULL,
    CONSTRAINT "FK_ProductOptions_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ProductOptions_ProductId" ON "ProductOptions" ("ProductId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250404173940_CafeMenuSystem', '9.0.3');

COMMIT;

