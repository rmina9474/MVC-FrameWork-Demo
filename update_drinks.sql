-- Update existing drinks
UPDATE Products 
SET 
    Name = 'Vietnamese Black Coffee',
    Description = 'Strong and bold traditional Vietnamese black coffee, served hot',
    Price = 39000,
    IsAvailable = 1,
    CanCustomize = 1,
    HasSizeOptions = 1,
    PrepTime = '3-4 min',
    IsFeatured = 1
WHERE Id = 1;

UPDATE Products 
SET 
    Name = 'Black Tea',
    Description = 'Fragrant black tea with a rich aroma and smooth taste',
    Price = 45000,
    IsAvailable = 1,
    CanCustomize = 1,
    HasSizeOptions = 1,
    PrepTime = '2-3 min',
    IsFeatured = 0
WHERE Id = 2;

UPDATE Products 
SET 
    Name = 'Iced Caramel Macchiato',
    Description = 'Espresso combined with vanilla syrup, milk and caramel sauce, served over ice',
    Price = 52000,
    IsAvailable = 1,
    CanCustomize = 1,
    HasSizeOptions = 1,
    PrepTime = '3-4 min',
    IsFeatured = 1
WHERE Id = 3;

-- Add new drinks using available images
INSERT INTO Products (Name, Price, Description, ImageUrl, CategoryId, IsAvailable, CanCustomize, HasSizeOptions, PrepTime, IsFeatured)
VALUES 
('Cappuccino', 48000, 'Italian coffee drink made with espresso and steamed milk foam', '/images/coffee/Cappuchino.jpg', 1, 1, 1, 1, '3-4 min', 1),
('Vietnamese Coffee with Condensed Milk', 42000, 'Traditional Vietnamese coffee with sweet condensed milk', '/images/coffee/Cafe-Y.jpg', 1, 1, 1, 1, '3-4 min', 1),
('Lemon Tea', 40000, 'Refreshing tea served with fresh lemon and a hint of honey', '/images/coffee/TraChanh.jpg', 3, 1, 1, 1, '2-3 min', 0),
('Nespresso Specialty', 55000, 'Premium coffee made with Nespresso capsules', '/images/coffee/Nespresso.jpg', 1, 1, 1, 1, '1-2 min', 1),
('Cocktail Special', 65000, 'Non-alcoholic coffee cocktail with a blend of flavors', '/images/coffee/Cocktail.jpg', 3, 1, 1, 1, '4-5 min', 1),
('Signature Cocktail Collection', 72000, 'Assortment of our signature non-alcoholic cocktails', '/images/coffee/Cocktails.jpg', 3, 1, 1, 1, '5-6 min', 0),
('Capuchino Classic', 50000, 'Classic capuchino with perfect milk foam ratio', '/images/coffee/Capuchino.jpg', 1, 1, 1, 1, '3-4 min', 1);

-- Add product options for new drinks
INSERT INTO ProductOptions (Name, AdditionalPrice, OptionType, ProductId, IsDefault)
VALUES 
-- Cappuccino size options
('Small', 0, 'Size', 4, 1),
('Medium', 10000, 'Size', 4, 0),
('Large', 15000, 'Size', 4, 0),

-- Vietnamese Coffee with Condensed Milk options
('Regular', 0, 'Size', 5, 1),
('Large', 10000, 'Size', 5, 0),
('Extra Condensed Milk', 5000, 'Extra', 5, 0),

-- Lemon Tea options
('Small', 0, 'Size', 6, 1),
('Medium', 8000, 'Size', 6, 0),
('Large', 12000, 'Size', 6, 0),
('Extra Lemon', 5000, 'Extra', 6, 0),
('With Honey', 5000, 'Extra', 6, 0),

-- Nespresso options
('Ristretto', 0, 'Type', 7, 1),
('Espresso', 0, 'Type', 7, 0),
('Lungo', 5000, 'Type', 7, 0),

-- Cocktail options
('Regular', 0, 'Size', 8, 1),
('Large', 15000, 'Size', 8, 0),
('Extra Sweet', 5000, 'Customize', 8, 0),
('Less Sweet', 0, 'Customize', 8, 0),

-- Cocktail Collection options
('Sampler (3 types)', 0, 'Size', 9, 1),
('Full Collection (5 types)', 25000, 'Size', 9, 0),

-- Capuchino Classic options
('Small', 0, 'Size', 10, 1),
('Medium', 10000, 'Size', 10, 0),
('Large', 15000, 'Size', 10, 0),
('Extra Shot', 8000, 'Extra', 10, 0),
('With Caramel', 5000, 'Flavor', 10, 0),
('With Chocolate', 5000, 'Flavor', 10, 0);

-- Update existing drink options (assuming they exist)
-- For Vietnamese Black Coffee (ID 1)
INSERT INTO ProductOptions (Name, AdditionalPrice, OptionType, ProductId, IsDefault)
VALUES 
('Small', 0, 'Size', 1, 1),
('Medium', 8000, 'Size', 1, 0),
('Large', 12000, 'Size', 1, 0),
('Extra Strong', 5000, 'Strength', 1, 0);

-- For Black Tea (ID 2)
INSERT INTO ProductOptions (Name, AdditionalPrice, OptionType, ProductId, IsDefault)
VALUES 
('Small', 0, 'Size', 2, 1),
('Medium', 8000, 'Size', 2, 0),
('Large', 12000, 'Size', 2, 0),
('With Honey', 5000, 'Extra', 2, 0);

-- For Iced Caramel Macchiato (ID 3)
INSERT INTO ProductOptions (Name, AdditionalPrice, OptionType, ProductId, IsDefault)
VALUES 
('Regular', 0, 'Size', 3, 1),
('Large', 12000, 'Size', 3, 0),
('Extra Caramel', 5000, 'Extra', 3, 0),
('Extra Shot', 8000, 'Extra', 3, 0); 