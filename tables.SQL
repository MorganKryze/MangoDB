CREATE DATABASE MangoDB;

CREATE TABLE Ingredients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    country VARCHAR(50),
    allergen VARCHAR(50),
);

CREATE TABLE Material(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    price FLOAT,
    availability_status BOOLEAN,
);

CREATE TABLE Menus(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    allergen VARCHAR(50),
    quantity INT,
    price FLOAT,
    recipe VARCHAR(50),
    calories INT,
);

CREATE TABLE Recipes(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    allergen VARCHAR(50),
    quantity INT,
    price FLOAT,
    ingredients VARCHAR(50),
    time FLOAT,
);

CREATE TABLE Mango_master(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
);

CREATE TABLE Mango_chiefs(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    salary FLOAT,
    regular_hours FLOAT,
    sup_hours FLOAT
    status VARCHAR(50),
);

CREATE TABLE Supplier(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    country VARCHAR(50),
    cost FLOAT,
);

CREATE TABLE Customer(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) AUTO_INCREMENT PRIMARY KEY,
    loyaulty BOOLEAN,
    purchase FLOAT,
    point INT,
    allergy VARCHAR(50)
);

