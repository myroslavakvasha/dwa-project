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
	Description nvarchar(max)
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

-- Amdin user hardcoded
insert into [User](Username, RoleId, PwdHash, PwdSalt, FirstName, LastName, Email, Phone)
values (
	'admin',
	(select Id from Role where RoleTitle = 'Admin'),
	'ODNdaGCIkdnlmkXW3q7NVz1t4FTwlPmoRGc2ASpTiNQ=',
	's0TVOl5q3cOAtebj0ET4Nw==',
	'Myroslava',
	'Kvasha',
	'mkvasha@algebra.hr',
	'0953732308')
go

-- Test user hardcoded
insert into [User](Username, RoleId, PwdHash, PwdSalt, FirstName, LastName, Email, Phone)
values (
	'test',
	(select Id from Role where RoleTitle = 'User'),
	'yBZ6B6eHA6AFHtGhAJllmru+GRgDjdmhk7smYwv65CQ=',
	'BoAzpi2mTgFyB02G8//LqA==',
	'Test',
	'Test',
	'test@example.com',
	'0503159908')
go

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
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Ribeye Steak', 1, 350, 24.99, 'Juicy ribeye steak grilled to perfection, served with herb butter')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Chicken Wings', 1, 400, 12.99, 'Crispy grilled chicken wings with BBQ glaze')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Pork Ribs', 1, 500, 18.99, 'Slow-grilled pork ribs with smoky BBQ sauce')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Grilled Salmon', 1, 300, 19.99, 'Fresh Atlantic salmon fillet grilled with lemon and herbs')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Mixed Grill Platter', 1, 600, 28.99, 'Selection of grilled chicken, beef, and pork with side salad')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('BBQ Chicken Thighs', 1, 350, 13.99, 'Marinated chicken thighs grilled over open flame')
go

insert into Food (Name, CategoryId, Weight, Price, Description) values ('Margherita', 2, 450, 10.99, 'Classic tomato sauce, fresh mozzarella, and basil')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Pepperoni', 2, 500, 12.99, 'Tomato sauce, mozzarella, and generous pepperoni slices')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('BBQ Chicken Pizza', 2, 520, 13.99, 'BBQ sauce base, grilled chicken, red onion, and mozzarella')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Four Cheese', 2, 480, 13.99, 'Mozzarella, gorgonzola, parmesan, and cheddar on tomato base')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Veggie Supreme', 2, 490, 11.99, 'Tomato sauce, mozzarella, bell peppers, mushrooms, and olives')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Diavola', 2, 500, 13.99, 'Spicy salami, tomato sauce, mozzarella, and chili flakes')
go

insert into Food (Name, CategoryId, Weight, Price, Description) values ('Classic Beef Burger', 3, 350, 10.99, 'Beef patty, lettuce, tomato, onion, pickles, and house sauce')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Cheeseburger', 3, 370, 11.99, 'Beef patty with melted cheddar, lettuce, tomato, and mustard')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Crispy Chicken Burger', 3, 340, 10.99, 'Crispy fried chicken breast, coleslaw, pickles, and mayo')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('BBQ Bacon Burger', 3, 420, 13.99, 'Beef patty, bacon, cheddar, BBQ sauce, and crispy onions')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Mushroom Swiss Burger', 3, 390, 12.99, 'Beef patty, sauteed mushrooms, Swiss cheese, and garlic aioli')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Double Smash Burger', 3, 480, 14.99, 'Two smashed beef patties, American cheese, pickles, and special sauce')
go

insert into Food (Name, CategoryId, Weight, Price, Description) values ('French Fries', 4, 200, 3.99, 'Crispy golden fries seasoned with sea salt')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Sweet Potato Fries', 4, 200, 4.99, 'Crispy sweet potato fries with chipotle dipping sauce')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Onion Rings', 4, 180, 4.49, 'Beer-battered onion rings served with ranch dip')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Caesar Salad', 4, 250, 6.99, 'Romaine lettuce, croutons, parmesan, and Caesar dressing')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Coleslaw', 4, 150, 2.99, 'Creamy homemade coleslaw with carrot and cabbage')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Garlic Bread', 4, 150, 3.49, 'Toasted bread with garlic butter and parsley')
go

insert into Food (Name, CategoryId, Weight, Price, Description) values ('Coca-Cola', 5, 330, 2.49, 'Ice cold Coca-Cola 330ml can')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Lemonade', 5, 400, 3.49, 'Freshly squeezed lemonade with mint')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Iced Tea', 5, 400, 3.49, 'Homemade iced tea with lemon')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Sparkling Water', 5, 330, 1.99, 'Sparkling mineral water 330ml')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Orange Juice', 5, 300, 3.99, 'Freshly squeezed orange juice')
insert into Food (Name, CategoryId, Weight, Price, Description) values ('Craft Beer', 5, 500, 5.99, 'Local craft lager on tap 500ml')
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