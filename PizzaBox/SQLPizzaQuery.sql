
--Create Schema Pizza_Schema;

create table Pizza_Schema.Stores(
	storeID int IDENTITY(1,1) NOT NULL PRIMARY Key,
	storeName varchar(50) NOT NULL UNIQUE,
	storeCode varchar(50) NOT NULL,
);

create table Pizza_Schema.Users(
	userID int IDENTITY(1,1) NOT NULL PRIMARY Key,
	userName varchar(50) NOT NULL UNIQUE,
	userCode varchar(50) NOT NULL,
	storeID int FOREIGN KEY REFERENCES Pizza_Schema.Stores(storeID),
	storeTime DateTime
);

create table Pizza_Schema.Orders(
	orderID int IDENTITY(1,1) NOT NULL PRIMARY Key,
	storeID int FOREIGN KEY REFERENCES Pizza_Schema.Stores(storeID) NOT NULL,
	userID int FOREIGN KEY REFERENCES Pizza_Schema.Users(userID) NOT NULL,
	pizzaAmount int NOT NULL,
	cost MONEY NOT NULL,
	orderTime DATETIME NOT NULL
);

create table Pizza_Schema.Pizzas(
	pizzaID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	orderID int FOREIGN KEY REFERENCES Pizza_Schema.Orders(orderID) NOT NULL,
	crust varchar(20) NOT NULL,
	size varchar(20) NOT NULL,
	pizzaCost MONEY NOT NULL
);

create table Pizza_Schema.Toppings(
	toppingID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	pizzaID int FOREIGN KEY REFERENCES Pizza_Schema.Pizzas(pizzaID) NOT NULL,
	topping varchar(20)
);

Select * from Pizza_Schema.Stores;
Select * from Pizza_Schema.Users;
Select * from Pizza_Schema.Orders;
Select * from Pizza_Schema.Pizzas;
Select * from Pizza_Schema.Toppings;
delete from Pizza_Schema.Toppings where Pizza_Schema.Toppings.PizzaID=19;
delete from Pizza_Schema.Pizzas where Pizza_Schema.Pizzas.orderID=20;
delete from Pizza_Schema.Orders where Pizza_Schema.Orders.orderID=19;


