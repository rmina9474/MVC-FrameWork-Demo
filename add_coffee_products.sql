-- First, clear existing data and create coffee categories
DELETE FROM Products;
DELETE FROM Categories;

-- Reset identity columns
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);

-- Add Coffee Categories
INSERT INTO Categories (Name) VALUES ('Hot Coffee');
INSERT INTO Categories (Name) VALUES ('Cold Coffee');
INSERT INTO Categories (Name) VALUES ('Specialty Drinks');
INSERT INTO Categories (Name) VALUES ('Food & Pastries');

-- Add Coffee Products
-- Hot Coffee
INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Classic Espresso', 'A rich and intense shot of our signature espresso blend.', 35000, 1, '/images/coffee/espresso.jpg', 1, 0, 0, '1-2 min', 1);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Creamy Cappuccino', 'Espresso with steamed milk and a thick layer of foam.', 45000, 1, '/images/coffee/cappuccino.jpg', 1, 1, 1, '3-4 min', 1);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Vanilla Latte', 'Espresso with steamed milk and a hint of vanilla.', 50000, 1, '/images/coffee/latte.jpg', 1, 1, 1, '3-4 min', 1);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Caramel Macchiato', 'Espresso marked with a dollop of foamed milk and caramel drizzle.', 55000, 1, '/images/coffee/espresso.jpg', 1, 1, 1, '3-4 min', 0);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Mocha', 'Espresso with steamed milk, chocolate syrup, and whipped cream.', 52000, 1, '/images/coffee/latte.jpg', 1, 1, 1, '3-4 min', 0);

-- Cold Coffee
INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Iced Americano', 'Chilled espresso with cold water and ice.', 40000, 2, '/images/coffee/espresso.jpg', 1, 0, 1, '2-3 min', 1);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Cold Brew', 'Coffee brewed with cold water for 12+ hours, resulting in a smooth, less acidic taste.', 48000, 2, '/images/coffee/latte.jpg', 1, 0, 1, '1 min', 1);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Vietnamese Iced Coffee', 'Traditional Vietnamese coffee with condensed milk and ice.', 45000, 2, '/images/coffee/cappuccino.jpg', 1, 0, 1, '3-4 min', 1);

-- Specialty Drinks
INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Caramel Frappuccino', 'Blended coffee with caramel, milk, ice, and topped with whipped cream.', 60000, 3, '/images/coffee/latte.jpg', 1, 1, 1, '4-5 min', 0);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Matcha Green Tea Latte', 'Premium matcha green tea with steamed milk.', 55000, 3, '/images/coffee/cappuccino.jpg', 1, 1, 1, '3-4 min', 0);

-- Food & Pastries
INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Butter Croissant', 'Flaky, buttery croissant, baked fresh daily.', 25000, 4, '/images/coffee/espresso.jpg', 1, 0, 0, '1 min', 0);

INSERT INTO Products (Name, Description, Price, CategoryId, ImageUrl, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES ('Chocolate Chip Cookie', 'Freshly baked cookie with chunks of chocolate.', 20000, 4, '/images/coffee/latte.jpg', 1, 0, 0, '1 min', 0); 