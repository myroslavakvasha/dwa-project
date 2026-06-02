create table Role(
	Id int primary key identity,
	RoleTitle nvarchar(255) not null
)
go

create table [User](
	Id int primary key identity,
	Username nvarchar(50) not null,
	RoleId int foreign key references Role(Id) not null,
	PwdHash nvarchar(255) not null,
	PwdSalt nvarchar(255) not null,
	FirstName nvarchar(255) not null,
	LastName nvarchar(255) not null,
	Email nvarchar(255) not null,
	Phone nvarchar(255) not null
)
go

create table Category(
	Id int primary key identity,
	Name nvarchar(255) not null
)
go

create table Allergen(
	Id int primary key identity,
	Name nvarchar(255) not null
)
go

create table Food(
	Id int primary key identity,
	Name nvarchar(255) not null,
	CategoryId int foreign key references Category(Id) not null,
	Weight decimal,
	Price money not null,
	Description nvarchar(max),
	ImageUrl nvarchar(255)
)
go

create table FoodAllergen(
	FoodId int foreign key references Food(Id) not null,
	AllergenId int foreign key references Allergen(Id) not null,
	primary key (FoodId, AllergenId)
)
go

create table Payment(
	Id int primary key identity,
	Type nvarchar(255) not null
)
go

create table [Order](
	Id int primary key identity,
	Date datetime not null,
	UserId int foreign key references [User](Id) not null,
	PaymentId int foreign key references Payment(Id) not null,
	Comment nvarchar(max)
)
go

create table OrderItem(
	Id int primary key identity,
	OrderId int foreign key references [Order](Id) not null,
	FoodId int foreign key references Food(Id) not null,
	TotalPrice money not null,
	Quantity int not null
)
go

create table LogLevel(
	Id int primary key identity,
	Title nvarchar(255) not null
)
go

create table Log(
	Id int primary key identity,
	Timestamp datetime not null default getdate(),
	LogLevelId int foreign key references LogLevel(Id) not null,
	Message nvarchar(max) not null
)
go

insert into Role (RoleTitle) values ('Admin')
insert into Role (RoleTitle) values ('User')
go

SET IDENTITY_INSERT [User] ON;
GO

-- Admin user hardcoded
insert into [User] (Id, Username, RoleId, PwdHash, PwdSalt, FirstName, LastName, Email, Phone)
values (
    1,
    'admin',
    (select Id from Role where RoleTitle = 'Admin'),
    'ODNdaGCIkdnlmkXW3q7NVz1t4FTwlPmoRGc2ASpTiNQ=',
    's0TVOl5q3cOAtebj0ET4Nw==',
    'Myroslava',
    'Kvasha',
    'mkvasha@algebra.hr',
    '0953732308'
)
GO

-- Test user hardcoded
insert into [User] (Id, Username, RoleId, PwdHash, PwdSalt, FirstName, LastName, Email, Phone)
values (
    2,
    'test',
    (select Id from Role where RoleTitle = 'User'),
    'yBZ6B6eHA6AFHtGhAJllmru+GRgDjdmhk7smYwv65CQ=',
    'BoAzpi2mTgFyB02G8//LqA==',
    'Test',
    'Test',
    'test@example.com',
    '0503159908'
)
GO

SET IDENTITY_INSERT [User] OFF;
GO

-- Log levels
insert into LogLevel (Title) values ('INFO')
insert into LogLevel (Title) values ('ERROR')
go

--Payments
insert into Payment (Type) values ('Cash')
insert into Payment (Type) values ('Card')
go

-- Categories
insert into Category (Name) values ('Grill')
insert into Category (Name) values ('Pizza')
insert into Category (Name) values ('Burger')
insert into Category (Name) values ('Sides')
insert into Category (Name) values ('Drinks')
go

-- Allergens
insert into Allergen (Name) values ('Celery')
insert into Allergen (Name) values ('Gluten')
insert into Allergen (Name) values ('Crustaceans')
insert into Allergen (Name) values ('Eggs')
insert into Allergen (Name) values ('Fish')
insert into Allergen (Name) values ('Lupin')
insert into Allergen (Name) values ('Milk')
insert into Allergen (Name) values ('Molluscs')
insert into Allergen (Name) values ('Mustard')
insert into Allergen (Name) values ('Nuts')
insert into Allergen (Name) values ('Peanuts')
insert into Allergen (Name) values ('Sesame seeds')
insert into Allergen (Name) values ('Sulphites')
insert into Allergen (Name) values ('Soya')
go

-- Foods
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Ribeye Steak', 1, 350, 24.99, 'Juicy ribeye steak grilled to perfection, served with herb butter', '/images/Ribeye-Steak.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Chicken Wings', 1, 400, 12.99, 'Crispy grilled chicken wings with BBQ glaze', '/images/Chicken-Wings.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Pork Ribs', 1, 500, 18.99, 'Slow-grilled pork ribs with smoky BBQ sauce', '/images/Pork-Ribs.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Grilled Salmon', 1, 300, 19.99, 'Fresh Atlantic salmon fillet grilled with lemon and herbs', '/images/Grilled-Salmon.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Mixed Grill Platter', 1, 600, 28.99, 'Selection of grilled chicken, beef, and pork with side salad', '/images/Mixed-Grill-Platter.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('BBQ Chicken Thighs', 1, 350, 13.99, 'Marinated chicken thighs grilled over open flame', '/images/BBQ-Chicken-Thighs.jpg')
go

insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Margherita', 2, 450, 10.99, 'Classic tomato sauce, fresh mozzarella, and basil', '/images/Margherita.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Pepperoni', 2, 500, 12.99, 'Tomato sauce, mozzarella, and generous pepperoni slices', '/images/Pepperoni.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('BBQ Chicken Pizza', 2, 520, 13.99, 'BBQ sauce base, grilled chicken, red onion, and mozzarella', '/images/BBQ-Chicken-Pizza.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Four Cheese', 2, 480, 13.99, 'Mozzarella, gorgonzola, parmesan, and cheddar on tomato base', '/images/Four-Cheese.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Veggie Supreme', 2, 490, 11.99, 'Tomato sauce, mozzarella, bell peppers, mushrooms, and olives', '/images/Veggie-Supreme.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Diavola', 2, 500, 13.99, 'Spicy salami, tomato sauce, mozzarella, and chili flakes', '/images/Diavola.jpg')
go

insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Classic Beef Burger', 3, 350, 10.99, 'Beef patty, lettuce, tomato, onion, pickles, and house sauce', '/images/Classic-Beef-Burger.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Cheeseburger', 3, 370, 11.99, 'Beef patty with melted cheddar, lettuce, tomato, and mustard', '/images/Cheeseburger.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Crispy Chicken Burger', 3, 340, 10.99, 'Crispy fried chicken breast, coleslaw, pickles, and mayo', '/images/Crispy-Chicken-Burger.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('BBQ Bacon Burger', 3, 420, 13.99, 'Beef patty, bacon, cheddar, BBQ sauce, and crispy onions', '/images/BBQ-Bacon-Burger.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Mushroom Swiss Burger', 3, 390, 12.99, 'Beef patty, sauteed mushrooms, Swiss cheese, and garlic aioli', '/images/Mushroom-Swiss-Burger.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Double Smash Burger', 3, 480, 14.99, 'Two smashed beef patties, American cheese, pickles, and special sauce', '/images/Double-Smash-Burger.jpg')
go

insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('French Fries', 4, 200, 3.99, 'Crispy golden fries seasoned with sea salt', '/images/French-Fries.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Sweet Potato Fries', 4, 200, 4.99, 'Crispy sweet potato fries with chipotle dipping sauce', '/images/Sweet-Potato-Fries.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Onion Rings', 4, 180, 4.49, 'Beer-battered onion rings served with ranch dip', '/images/Onion-Rings.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Caesar Salad', 4, 250, 6.99, 'Romaine lettuce, croutons, parmesan, and Caesar dressing', '/images/Caesar-Salad.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Coleslaw', 4, 150, 2.99, 'Creamy homemade coleslaw with carrot and cabbage', '/images/Coleslaw.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Garlic Bread', 4, 150, 3.49, 'Toasted bread with garlic butter and parsley', '/images/Garlic-Bread.jpg')
go

insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Coca-Cola', 5, 330, 2.49, 'Ice cold Coca-Cola 330ml can', '/images/Coca-Cola.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Lemonade', 5, 400, 3.49, 'Freshly squeezed lemonade with mint', '/images/Lemonade.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Iced Tea', 5, 400, 3.49, 'Homemade iced tea with lemon', '/images/Iced-Tea.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Sparkling Water', 5, 330, 1.99, 'Sparkling mineral water 330ml', '/images/Sparkling-Water.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Orange Juice', 5, 300, 3.99, 'Freshly squeezed orange juice', '/images/Orange-Juice.jpg')
insert into Food (Name, CategoryId, Weight, Price, Description, ImageUrl) values ('Craft Beer', 5, 500, 5.99, 'Local craft lager on tap 500ml', '/images/Craft-Beer.jpg')
go

-- FoodAllergens

-- Grill
insert into FoodAllergen values (1, 7)  -- Milk (butter)
insert into FoodAllergen values (1, 13) -- Sulphites
go

insert into FoodAllergen values (2, 2)  -- Gluten
insert into FoodAllergen values (2, 14) -- Soya
go

insert into FoodAllergen values (3, 2)  -- Gluten
insert into FoodAllergen values (3, 13) -- Sulphites
go

insert into FoodAllergen values (4, 5)  -- Fish
insert into FoodAllergen values (4, 7)  -- Milk
go

insert into FoodAllergen values (5, 2)  -- Gluten
insert into FoodAllergen values (5, 7)  -- Milk
insert into FoodAllergen values (5, 13) -- Sulphites
go

insert into FoodAllergen values (6, 2)  -- Gluten
insert into FoodAllergen values (6, 14) -- Soya
go

-- Pizza
insert into FoodAllergen values (7, 2)  -- Gluten
insert into FoodAllergen values (7, 7)  -- Milk
go

insert into FoodAllergen values (8, 2)  -- Gluten
insert into FoodAllergen values (8, 7)  -- Milk
insert into FoodAllergen values (8, 13) -- Sulphites
go

insert into FoodAllergen values (9, 2)  -- Gluten
insert into FoodAllergen values (9, 7)  -- Milk
insert into FoodAllergen values (9, 14) -- Soya
go

insert into FoodAllergen values (10, 2) -- Gluten
insert into FoodAllergen values (10, 7) -- Milk
go

insert into FoodAllergen values (11, 2) -- Gluten
insert into FoodAllergen values (11, 7) -- Milk
go

insert into FoodAllergen values (12, 2) -- Gluten
insert into FoodAllergen values (12, 7) -- Milk
insert into FoodAllergen values (12, 13)-- Sulphites
go

-- Burger
insert into FoodAllergen values (13, 2) -- Gluten
insert into FoodAllergen values (13, 4) -- Eggs
insert into FoodAllergen values (13, 9) -- Mustard
insert into FoodAllergen values (13, 12)-- Sesame seeds
go

insert into FoodAllergen values (14, 2) -- Gluten
insert into FoodAllergen values (14, 4) -- Eggs
insert into FoodAllergen values (14, 7) -- Milk
insert into FoodAllergen values (14, 9) -- Mustard
insert into FoodAllergen values (14, 12)-- Sesame seeds
go

insert into FoodAllergen values (15, 2) -- Gluten
insert into FoodAllergen values (15, 4) -- Eggs
insert into FoodAllergen values (15, 7) -- Milk
insert into FoodAllergen values (15, 12)-- Sesame seeds
go

insert into FoodAllergen values (16, 2) -- Gluten
insert into FoodAllergen values (16, 4) -- Eggs
insert into FoodAllergen values (16, 7) -- Milk
insert into FoodAllergen values (16, 9) -- Mustard
insert into FoodAllergen values (16, 12)-- Sesame seeds
insert into FoodAllergen values (16, 13)-- Sulphites
go

insert into FoodAllergen values (17, 2) -- Gluten
insert into FoodAllergen values (17, 4) -- Eggs
insert into FoodAllergen values (17, 7) -- Milk
insert into FoodAllergen values (17, 12)-- Sesame seeds
go

insert into FoodAllergen values (18, 2) -- Gluten
insert into FoodAllergen values (18, 4) -- Eggs
insert into FoodAllergen values (18, 7) -- Milk
insert into FoodAllergen values (18, 9) -- Mustard
insert into FoodAllergen values (18, 12)-- Sesame seeds
go

-- Sides
insert into FoodAllergen values (19, 2) -- Gluten

insert into FoodAllergen values (20, 2) -- Gluten
insert into FoodAllergen values (20, 7) -- Milk
go

insert into FoodAllergen values (21, 2) -- Gluten
insert into FoodAllergen values (21, 4) -- Eggs
insert into FoodAllergen values (21, 7) -- Milk
go

insert into FoodAllergen values (22, 2) -- Gluten
insert into FoodAllergen values (22, 4) -- Eggs
insert into FoodAllergen values (22, 5) -- Fish (anchovies in dressing)
insert into FoodAllergen values (22, 7) -- Milk
go

insert into FoodAllergen values (23, 4) -- Eggs (mayo)
insert into FoodAllergen values (23, 9) -- Mustard
go

insert into FoodAllergen values (24, 2) -- Gluten
insert into FoodAllergen values (24, 7) -- Milk
go

-- Drinks
insert into FoodAllergen values (30, 2) -- Gluten
insert into FoodAllergen values (30, 13)-- Sulphites
go

-- Orders
insert into [Order] (Date, UserId, PaymentId, Comment) values (
    '2025-01-10 19:30:00',
    (select Id from [User] where Username = 'test'),
    (select Id from Payment where Type = 'Card'),
    'Extra napkins please')
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (1, 7, 10.99, 1)  -- Margherita
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (1, 8, 12.99, 1)  -- Pepperoni
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (1, 25, 2.49, 2)  -- Coca-Cola x2
go

insert into [Order] (Date, UserId, PaymentId, Comment) values (
    '2025-01-12 13:00:00',
    (select Id from [User] where Username = 'test'),
    (select Id from Payment where Type = 'Cash'),
    null)
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (2, 14, 11.99, 1) -- Cheeseburger
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (2, 19, 3.99, 1)  -- French Fries
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (2, 26, 3.49, 1)  -- Lemonade
go

insert into [Order] (Date, UserId, PaymentId, Comment) values (
    '2025-01-15 20:00:00',
    (select Id from [User] where Username = 'test'),
    (select Id from Payment where Type = 'Card'),
    'Well done steak please')
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (3, 1, 24.99, 1)  -- Ribeye Steak
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (3, 22, 6.99, 1)  -- Caesar Salad
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (3, 30, 5.99, 2)  -- Craft Beer x2
go

insert into [Order] (Date, UserId, PaymentId, Comment) values (
    '2025-01-18 12:30:00',
    (select Id from [User] where Username = 'test'),
    (select Id from Payment where Type = 'Cash'),
    null)
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (4, 16, 13.99, 2) -- BBQ Bacon Burger x2
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (4, 20, 4.99, 2)  -- Sweet Potato Fries x2
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (4, 28, 1.99, 2)  -- Sparkling Water x2
go

insert into [Order] (Date, UserId, PaymentId, Comment) values (
    '2025-01-20 18:00:00',
    (select Id from [User] where Username = 'test'),
    (select Id from Payment where Type = 'Card'),
    'Cutlery included please')
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (5, 5, 28.99, 1)  -- Mixed Grill Platter
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (5, 10, 13.99, 1) -- Four Cheese Pizza
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (5, 21, 4.49, 2)  -- Onion Rings x2
insert into OrderItem (OrderId, FoodId, TotalPrice, Quantity) values (5, 30, 5.99, 2)  -- Craft Beer x2
go