-- This file contains the SQL script to create the database schema and insert some initial data.

-- Drop the default postgres database
DROP DATABASE IF EXISTS postgres;

-- Moves to the project database
\c mangodb

-- Creates our custom enum types
CREATE TYPE "loyalty_rank" AS ENUM('Classic', 'Bronze', 'Silver', 'Gold');

CREATE TYPE "price_category" AS ENUM('Low', 'Medium', 'High');

CREATE TYPE "order_status" AS ENUM(
    'Pending', 'In Progress', 'Completed'
);

-- Creates the database tables
CREATE TABLE "customer" (
    "email" VARCHAR(255) PRIMARY KEY, "first_name" VARCHAR(255), "last_name" VARCHAR(255), "password" VARCHAR(255), "order_count" INT, "loyalty_rank" "loyalty_rank"
);

CREATE TABLE "mango_manager" (
    "email" VARCHAR(255) PRIMARY KEY, "first_name" VARCHAR(255), "last_name" VARCHAR(255), "password" VARCHAR(255), "working_hours" TIME, "salary" DECIMAL(10, 2)
);

CREATE TABLE "mango_chef" (
    "email" VARCHAR(255) PRIMARY KEY, "manager_email" VARCHAR(255) REFERENCES "mango_manager" ("email"), "first_name" VARCHAR(255), "last_name" VARCHAR(255), "password" VARCHAR(255), "working_hours" TIME, "salary" FLOAT
);

CREATE TABLE "supplier_company" (
    "email" VARCHAR(255) PRIMARY KEY, "manager_email" VARCHAR(255) REFERENCES "mango_manager" ("email"), "name" VARCHAR(255), "password" VARCHAR(255), "address" VARCHAR(255), "price_category" "price_category"
);

CREATE TABLE "order" (
    "time" TIMESTAMP PRIMARY KEY, "customer_email" VARCHAR(255) REFERENCES "customer" ("email"), "mango_chef_email" VARCHAR(255) REFERENCES "mango_chef" ("email"), "price" FLOAT, "status" "order_status"
);

CREATE TABLE "deal" (
    "name" VARCHAR(255) PRIMARY KEY, "price" FLOAT, "preparation_time" TIME
);

CREATE TABLE "order_deal" (
    "order_time" TIMESTAMP REFERENCES "order" ("time"), "deal_name" VARCHAR(255) REFERENCES "deal" ("name"), "quantity" INT, PRIMARY KEY ("order_time", "deal_name")
);

CREATE TABLE "recipe" ("name" VARCHAR(255) PRIMARY KEY);

CREATE TABLE "recipe_deal" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"), "deal_name" VARCHAR(255) REFERENCES "deal" ("name"), "quantity" INT, PRIMARY KEY ("recipe_name", "deal_name")
);

CREATE TABLE "step" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"), "description" VARCHAR(255), "time" TIME, PRIMARY KEY ("recipe_name", "description")
);

CREATE TABLE "ingredient" (
    "name" VARCHAR(255) PRIMARY KEY, "supplier_email" VARCHAR(255) REFERENCES "supplier_company" ("email"), "price" FLOAT, "calories" FLOAT, "in_stock" INT, "allergen" VARCHAR(255), "country" VARCHAR(255)
);

CREATE TABLE "tool" (
    "name" VARCHAR(255) PRIMARY KEY, "price" FLOAT, "in_stock" INT
);

CREATE TABLE "recipe_ingredient" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"), "ingredient_name" VARCHAR(255) REFERENCES "ingredient" ("name"), "quantity" INT, PRIMARY KEY ("recipe_name", "ingredient_name")
);

CREATE TABLE "recipe_tool" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"), "tool_name" VARCHAR(255) REFERENCES "tool" ("name"), "quantity" INT, PRIMARY KEY ("recipe_name", "tool_name")
);

-- Inserts some initial data
INSERT INTO
    customer (
        email, first_name, last_name, password, loyalty_rank
    )
VALUES (
        'john.doe@example.com', 'John', 'Doe', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Classic'
    ),
    (
        'jane.smith@example.com', 'Jane', 'Smith', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Bronze'
    ),
    (
        'mike.jones@example.com', 'Mike', 'Jones', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Gold'
    ),
    (
        'sarah.johnson@example.com', 'Sarah', 'Johnson', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Bronze'
    ),
    (
        'paul.williams@example.com', 'Paul', 'Williams', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Silver'
    ),
    (
        'lisa.brown@example.com', 'Lisa', 'Brown', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Bronze'
    ),
    (
        'james.davis@example.com', 'James', 'Davis', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Bronze'
    ),
    (
        'patricia.miller@example.com', 'Patricia', 'Miller', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Silver'
    ),
    (
        'robert.wilson@example.com', 'Robert', 'Wilson', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Silver'
    ),
    (
        'linda.moore@example.com', 'Linda', 'Moore', '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A', 'Gold'
    );

INSERT INTO
    mango_manager (
        email, first_name, last_name, password, working_hours, salary
    )
VALUES (
        'admin', 'John', 'Smith', '2BB80D537B1DA3E38BD30361AA855686BDE0EACD7162FEF6A25FE97BF527A25B', '09:00:00', 5000.00
    );

-- Insert into mango_chef
INSERT INTO "mango_chef" ("email", "manager_email", "first_name", "last_name", "password", "working_hours", "salary")
VALUES 
('chef1@example.com', 'admin', 'Chef', 'One', 'password1', '08:00:00', 3000.00),
('chef2@example.com', 'admin', 'Chef', 'Two', 'password2', '08:00:00', 3500.00);

-- Insert into supplier_company
INSERT INTO "supplier_company" ("email", "manager_email", "name", "password", "address", "price_category")
VALUES 
('supplier1@example.com', 'admin', 'Supplier One', 'password1', '123 Street, City, Country', 'Low'),
('supplier2@example.com', 'admin', 'Supplier Two', 'password2', '456 Avenue, City, Country', 'Medium');

-- Insert into "order"
INSERT INTO "order" ("time", "customer_email", "mango_chef_email", "price", "status")
VALUES 
('2022-01-01 10:00:00', 'john.doe@example.com', 'chef1@example.com', 20.00, 'Completed'),
('2022-01-02 11:00:00', 'jane.smith@example.com', 'chef2@example.com', 25.00, 'In Progress');

-- Insert into deal
INSERT INTO "deal" ("name", "price", "preparation_time")
VALUES 
('Deal One', 10.00, '00:30:00'),
('Deal Two', 15.00, '00:45:00');

-- Insert into order_deal
INSERT INTO "order_deal" ("order_time", "deal_name", "quantity")
VALUES 
('2022-01-01 10:00:00', 'Deal One', 2),
('2022-01-02 11:00:00', 'Deal Two', 1);