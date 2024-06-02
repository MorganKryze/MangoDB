-- This file contains the SQL script to create the database schema and insert some initial data.
-- Creates our custom enum types
CREATE TYPE "loyalty_rank" AS ENUM('Classic', 'Bronze', 'Silver', 'Gold');
CREATE TYPE "price_category" AS ENUM('Low', 'Medium', 'High');
CREATE TYPE "order_status" AS ENUM('Pending', 'In Progress', 'Completed');
-- Creates the database tables
CREATE TABLE "customer" (
    "email" VARCHAR(255) PRIMARY KEY,
    "first_name" VARCHAR(255),
    "last_name" VARCHAR(255),
    "password" VARCHAR(255),
    "loyalty_rank" "loyalty_rank"
);
CREATE TABLE "mango_manager" (
    "email" VARCHAR(255) PRIMARY KEY,
    "first_name" VARCHAR(255),
    "last_name" VARCHAR(255),
    "password" VARCHAR(255),
    "working_hours" TIME,
    "salary" DECIMAL(10, 2)
);
CREATE TABLE "mango_chef" (
    "email" VARCHAR(255) PRIMARY KEY,
    "manager_email" VARCHAR(255) REFERENCES "mango_manager" ("email"),
    "first_name" VARCHAR(255),
    "last_name" VARCHAR(255),
    "password" VARCHAR(255),
    "working_hours" TIME,
    "salary" FLOAT
);
CREATE TABLE "supplier_company" (
    "email" VARCHAR(255) PRIMARY KEY,
    "manager_email" VARCHAR(255) REFERENCES "mango_manager" ("email"),
    "name" VARCHAR(255),
    "password" VARCHAR(255),
    "address" VARCHAR(255),
    "price_category" "price_category"
);
CREATE TABLE "order" (
    "time" TIMESTAMP PRIMARY KEY,
    "customer_email" VARCHAR(255) REFERENCES "customer" ("email"),
    "mango_chef_email" VARCHAR(255) REFERENCES "mango_chef" ("email"),
    "price" FLOAT,
    "status" "order_status"
);
CREATE TABLE "recipe" (
    "name" VARCHAR(255) PRIMARY KEY,
    "price" FLOAT
);
CREATE TABLE "order_recipe" (
    "order_time" TIMESTAMP REFERENCES "order" ("time"),
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"),
    "quantity" INT,
    PRIMARY KEY ("order_time", "recipe_name")
);
CREATE TABLE "step" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"),
    "step_number" INT,
    "description" VARCHAR(255),
    "time" TIME,
    PRIMARY KEY ("recipe_name", "step_number")
);
CREATE TABLE "ingredient" (
    "name" VARCHAR(255) PRIMARY KEY,
    "supplier_email" VARCHAR(255) REFERENCES "supplier_company" ("email"),
    "price" FLOAT,
    "calories" INT,
    "in_stock" INT,
    "allergen" VARCHAR(255),
    "country" VARCHAR(255)
);
CREATE TABLE "tool" (
    "name" VARCHAR(255) PRIMARY KEY,
    "price" FLOAT,
    "in_stock" INT
);
CREATE TABLE "recipe_ingredient" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"),
    "ingredient_name" VARCHAR(255) REFERENCES "ingredient" ("name"),
    "quantity" INT,
    PRIMARY KEY ("recipe_name", "ingredient_name")
);
CREATE TABLE "recipe_tool" (
    "recipe_name" VARCHAR(255) REFERENCES "recipe" ("name"),
    "tool_name" VARCHAR(255) REFERENCES "tool" ("name"),
    "quantity" INT,
    PRIMARY KEY ("recipe_name", "tool_name")
);
-- Inserts some initial data
INSERT INTO "customer" (
        "email",
        "first_name",
        "last_name",
        "password",
        "loyalty_rank"
    )
VALUES (
        'john.doe@example.com',
        'John',
        'Doe',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'jane.smith@example.com',
        'Jane',
        'Smith',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'mike.jones@example.com',
        'Mike',
        'Jones',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'sarah.johnson@example.com',
        'Sarah',
        'Johnson',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'paul.williams@example.com',
        'Paul',
        'Williams',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'lisa.brown@example.com',
        'Lisa',
        'Brown',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'james.davis@example.com',
        'James',
        'Davis',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'patricia.miller@example.com',
        'Patricia',
        'Miller',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'robert.wilson@example.com',
        'Robert',
        'Wilson',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    ),
    (
        'linda.moore@example.com',
        'Linda',
        'Moore',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        'Classic'
    );
INSERT INTO "mango_manager" (
        "email",
        "first_name",
        "last_name",
        "password",
        "working_hours",
        "salary"
    )
VALUES (
        'admin',
        'John',
        'Smith',
        '2BB80D537B1DA3E38BD30361AA855686BDE0EACD7162FEF6A25FE97BF527A25B',
        '09:00:00',
        5000.00
    );
INSERT INTO "mango_chef" (
        "email",
        "manager_email",
        "first_name",
        "last_name",
        "password",
        "working_hours",
        "salary"
    )
VALUES (
        'chef1@example.com',
        'admin',
        'Chef',
        'One',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        '08:00:00',
        3000.00
    ),
    (
        'chef2@example.com',
        'admin',
        'Chef',
        'Two',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        '08:00:00',
        3500.00
    );
INSERT INTO "supplier_company" (
        "email",
        "manager_email",
        "name",
        "password",
        "address",
        "price_category"
    )
VALUES (
        'minifoodcmpny@food.com',
        'admin',
        'MiniFood',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        '123 Bellevue Street, Dublin, Ireland',
        'Low'
    ),
    (
        'maxifood@superfood.com',
        'admin',
        'MaxiFood',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        '456 Avenue Road, Dublin, Ireland',
        'Medium'
    ),
    (
        'alloutorder@market.com',
        'admin',
        'AllOutOrder',
        '96D9632F363564CC3032521409CF22A852F2032EEC099ED5967C0D000CEC607A',
        '789 Crescent Lane, Dublin, Ireland',
        'High'
    );
INSERT INTO "recipe" ("name", "price")
VALUES ('Bananas party', 4.5),
    ('Cherry delight', 5.8),
    ('Date night', 7.2),
    ('Mango mix', 5.5),
    ('Watermelon wonder', 4.8),
    ('Kiwi kiss', 6.5),
    ('Peach pleasure', 5.2),
    ('Sex apple', 5.7);
INSERT INTO "order" (
        "time",
        "customer_email",
        "mango_chef_email",
        "price",
        "status"
    )
VALUES (
        '2024-05-25 10:00:00',
        'john.doe@example.com',
        'chef1@example.com',
        9.0,
        'Completed'
    ),
    (
        '2024-05-25 11:00:00',
        'jane.smith@example.com',
        'chef1@example.com',
        7.2,
        'Completed'
    ),
    (
        '2024-05-25 12:00:00',
        'sarah.johnson@example.com',
        'chef2@example.com',
        20.4,
        'Completed'
    );
INSERT INTO "order_recipe" (
        "order_time",
        "recipe_name",
        "quantity"
    )
VALUES (
        '2024-05-25 10:00:00',
        'Bananas party',
        2
    ),
    (
        '2024-05-25 11:00:00',
        'Date night',
        1
    ),
    (
        '2024-05-25 12:00:00',
        'Peach pleasure',
        3
    ),
    (
        '2024-05-25 12:00:00',
        'Watermelon wonder',
        1
    );
INSERT INTO "step" (
        "recipe_name",
        "step_number",
        "description",
        "time"
    )
VALUES (
        'Bananas party',
        1,
        'Peel and slice the bananas',
        '00:01:00'
    ),
    (
        'Bananas party',
        2,
        'Put the bananas in a mixer',
        '00:01:00'
    ),
    (
        'Bananas party',
        3,
        'Add sugar and milk to the mixer',
        '00:01:00'
    ),
    (
        'Bananas party',
        4,
        'Blend the mixture',
        '00:02:00'
    ),
    (
        'Bananas party',
        5,
        'Serve the banana shake',
        '00:01:00'
    ),
    (
        'Cherry delight',
        1,
        'Pit the cherries',
        '00:01:00'
    ),
    (
        'Cherry delight',
        2,
        'Mix the cherries with sugar by hand',
        '00:3:00'
    ),
    (
        'Cherry delight',
        3,
        'Add the cherry mixture to a bowl',
        '00:01:00'
    ),
    (
        'Cherry delight',
        4,
        'Chill the cherry mixture with cold milk',
        '00:10:00'
    ),
    (
        'Cherry delight',
        5,
        'Serve the cherry delight',
        '00:01:00'
    ),
    (
        'Date night',
        1,
        'Pit the dates',
        '00:20:00'
    ),
    (
        'Date night',
        2,
        'Stuff the dates with almonds',
        '00:15:00'
    ),
    (
        'Date night',
        3,
        'Put the dates in the mixer',
        '00:01:00'
    ),
    (
        'Date night',
        4,
        'Add milk & Cinnamon to the mixer',
        '00:01:30'
    ),
    (
        'Date night',
        5,
        'Blend the dates',
        '00:02:00'
    ),
    (
        'Date night',
        6,
        'Serve the date shake',
        '00:01:00'
    ),
    (
        'Mango mix',
        1,
        'Peel and dice the mangoes',
        '00:10:00'
    ),
    (
        'Mango mix',
        2,
        'Mix the mangoes with yogurt',
        '00:05:00'
    ),
    (
        'Mango mix',
        3,
        'Chill the mango mixture',
        '00:10:00'
    ),
    (
        'Mango mix',
        4,
        'Serve the mango mix',
        '00:01:00'
    ),
    (
        'Watermelon wonder',
        1,
        'Cut the watermelon into cubes',
        '00:10:00'
    ),
    (
        'Watermelon wonder',
        2,
        'Chill the watermelon cubes',
        '00:05:00'
    ),
    (
        'Watermelon wonder',
        3,
        'Pour honey over the watermelon',
        '00:01:00'
    ),
    (
        'Watermelon wonder',
        4,
        'Add a portion of smashed watermelon cut with water',
        '00:01:00'
    ),
    (
        'Watermelon wonder',
        5,
        'Serve the watermelon wonder',
        '00:01:00'
    ),
    (
        'Kiwi kiss',
        1,
        'Peel and slice the kiwis',
        '00:10:00'
    ),
    (
        'Kiwi kiss',
        2,
        'Mix the kiwis with sugar',
        '00:05:00'
    ),
    (
        'Kiwi kiss',
        3,
        'Add the kiwi mixture to a bowl',
        '00:01:00'
    ),
    (
        'Kiwi kiss',
        4,
        'Chill the kiwi mixture',
        '00:10:00'
    ),
    (
        'Kiwi kiss',
        5,
        'Serve the kiwi kiss',
        '00:01:00'
    ),
    (
        'Peach pleasure',
        1,
        'Peel and dice the peaches',
        '00:10:00'
    ),
    (
        'Peach pleasure',
        2,
        'Mix the peaches with sugar',
        '00:05:00'
    ),
    (
        'Peach pleasure',
        3,
        'Add the peach mixture to a bowl',
        '00:01:00'
    ),
    (
        'Peach pleasure',
        4,
        'Chill the peach mixture',
        '00:10:00'
    ),
    (
        'Peach pleasure',
        5,
        'Serve the peach pleasure',
        '00:01:00'
    ),
    (
        'Sex apple',
        1,
        'Peel and dice the apples',
        '00:10:00'
    ),
    (
        'Sex apple',
        2,
        'Mix the apples with sugar',
        '00:05:00'
    ),
    (
        'Sex apple',
        3,
        'Add the apple mixture to a bowl',
        '00:01:00'
    ),
    (
        'Sex apple',
        4,
        'Chill the apple mixture',
        '00:10:00'
    ),
    (
        'Sex apple',
        5,
        'Serve the apple pleasure',
        '00:01:00'
    );
INSERT INTO "ingredient" (
        "name",
        "supplier_email",
        "price",
        "calories",
        "in_stock",
        "allergen",
        "country"
    )
VALUES (
        'Bananas',
        'alloutorder@market.com',
        1.0,
        100,
        70,
        'None',
        'Spain'
    ),
    (
        'Cherries',
        'maxifood@superfood.com',
        0.3,
        50,
        50,
        'None',
        'Ireland'
    ),
    (
        'Dates',
        'minifoodcmpny@food.com',
        0.5,
        200,
        30,
        'None',
        'Morocco'
    ),
    (
        'Mangoes',
        'alloutorder@market.com',
        1.5,
        150,
        40,
        'None',
        'Cuba'
    ),
    (
        'Sugar',
        'minifoodcmpny@food.com',
        0.2,
        5,
        250,
        'None',
        'Ireland'
    ),
    (
        'Milk',
        'maxifood@superfood.com',
        0.5,
        50,
        250,
        'Lactose',
        'Ireland'
    ),
    (
        'Cinnamon',
        'alloutorder@market.com',
        0.1,
        10,
        100,
        'None',
        'Sri Lanka'
    ),
    (
        'Ice',
        'minifoodcmpny@food.com',
        0.1,
        0,
        200,
        'None',
        'Ireland'
    ),
    (
        'Almonds',
        'maxifood@superfood.com',
        0.5,
        100,
        50,
        'None',
        'Spain'
    ),
    (
        'Yogurt',
        'minifoodcmpny@food.com',
        0.3,
        50,
        100,
        'Lactose',
        'Ireland'
    ),
    (
        'Watermelon',
        'maxifood@superfood.com',
        1.0,
        100,
        60,
        'None',
        'Ireland'
    ),
    (
        'Honey',
        'alloutorder@market.com',
        0.2,
        10,
        100,
        'None',
        'Ireland'
    ),
    (
        'Kiwi',
        'alloutorder@market.com',
        0.5,
        50,
        50,
        'None',
        'Caribbean'
    ),
    (
        'Peaches',
        'maxifood@superfood.com',
        0.5,
        50,
        50,
        'None',
        'Spain'
    ),
    (
        'Apples',
        'minifoodcmpny@food.com',
        0.5,
        50,
        50,
        'None',
        'Ireland'
    );
INSERT INTO "tool" (
        "name",
        "price",
        "in_stock"
    )
VALUES ('Knife', 5.0, 10),
    ('Blender', 50.0, 5),
    ('Bowl', 10.0, 20),
    ('Mixer', 100.0, 2),
    ('Spoon', 2.0, 50),
    ('Cup', 1.0, 100);
INSERT INTO "recipe_ingredient" (
        "recipe_name",
        "ingredient_name",
        "quantity"
    )
VALUES (
        'Bananas party',
        'Bananas',
        2
    ),
    (
        'Bananas party',
        'Sugar',
        2
    ),
    (
        'Bananas party',
        'Milk',
        3
    ),
    (
        'Cherry delight',
        'Cherries',
        2
    ),
    (
        'Cherry delight',
        'Sugar',
        2
    ),
    (
        'Cherry delight',
        'Milk',
        3
    ),
    (
        'Cherry delight',
        'Ice',
        1
    ),
    (
        'Date night',
        'Dates',
        2
    ),
    (
        'Date night',
        'Almonds',
        2
    ),
    (
        'Date night',
        'Milk',
        3
    ),
    (
        'Date night',
        'Cinnamon',
        1
    ),
    (
        'Mango mix',
        'Mangoes',
        2
    ),
    (
        'Mango mix',
        'Yogurt',
        2
    ),
    (
        'Mango mix',
        'Ice',
        1
    ),
    (
        'Watermelon wonder',
        'Watermelon',
        1
    ),
    (
        'Watermelon wonder',
        'Honey',
        1
    ),
    (
        'Watermelon wonder',
        'Ice',
        1
    ),
    (
        'Kiwi kiss',
        'Kiwi',
        2
    ),
    (
        'Kiwi kiss',
        'Sugar',
        1
    ),
    (
        'Kiwi kiss',
        'Ice',
        2
    ),
    (
        'Peach pleasure',
        'Peaches',
        2
    ),
    (
        'Peach pleasure',
        'Sugar',
        1
    ),
    (
        'Peach pleasure',
        'Ice',
        1
    ),
    (
        'Sex apple',
        'Apples',
        2
    ),
    (
        'Sex apple',
        'Sugar',
        4
    ),
    (
        'Sex apple',
        'Ice',
        1
    );
INSERT INTO "recipe_tool" (
        "recipe_name",
        "tool_name",
        "quantity"
    )
VALUES (
        'Bananas party',
        'Knife',
        1
    ),
    (
        'Bananas party',
        'Blender',
        1
    ),
    (
        'Bananas party',
        'Bowl',
        1
    ),
    (
        'Bananas party',
        'Mixer',
        1
    ),
    (
        'Bananas party',
        'Spoon',
        1
    ),
    (
        'Bananas party',
        'Cup',
        1
    ),
    (
        'Cherry delight',
        'Knife',
        1
    ),
    (
        'Cherry delight',
        'Blender',
        1
    ),
    (
        'Cherry delight',
        'Bowl',
        1
    ),
    (
        'Cherry delight',
        'Mixer',
        1
    ),
    (
        'Cherry delight',
        'Spoon',
        1
    ),
    (
        'Cherry delight',
        'Cup',
        1
    ),
    (
        'Date night',
        'Knife',
        1
    ),
    (
        'Date night',
        'Blender',
        1
    ),
    (
        'Date night',
        'Bowl',
        1
    ),
    (
        'Date night',
        'Mixer',
        1
    ),
    (
        'Date night',
        'Spoon',
        1
    ),
    (
        'Date night',
        'Cup',
        1
    ),
    (
        'Mango mix',
        'Knife',
        1
    ),
    (
        'Mango mix',
        'Blender',
        1
    ),
    (
        'Mango mix',
        'Bowl',
        1
    ),
    (
        'Mango mix',
        'Mixer',
        1
    ),
    (
        'Mango mix',
        'Spoon',
        1
    ),
    (
        'Mango mix',
        'Cup',
        1
    ),
    (
        'Watermelon wonder',
        'Knife',
        1
    ),
    (
        'Watermelon wonder',
        'Blender',
        1
    ),
    (
        'Watermelon wonder',
        'Bowl',
        1
    ),
    (
        'Kiwi kiss',
        'Knife',
        1
    ),
    (
        'Kiwi kiss',
        'Blender',
        1
    ),
    (
        'Kiwi kiss',
        'Bowl',
        1
    ),
    (
        'Kiwi kiss',
        'Mixer',
        1
    ),
    (
        'Kiwi kiss',
        'Spoon',
        1
    ),
    (
        'Kiwi kiss',
        'Cup',
        1
    ),
    (
        'Peach pleasure',
        'Knife',
        1
    ),
    (
        'Peach pleasure',
        'Blender',
        1
    ),
    (
        'Peach pleasure',
        'Bowl',
        1
    ),
    (
        'Peach pleasure',
        'Mixer',
        1
    ),
    (
        'Peach pleasure',
        'Spoon',
        1
    ),
    (
        'Peach pleasure',
        'Cup',
        1
    ),
    (
        'Sex apple',
        'Knife',
        1
    ),
    (
        'Sex apple',
        'Blender',
        1
    ),
    (
        'Sex apple',
        'Bowl',
        1
    ),
    (
        'Sex apple',
        'Spoon',
        1
    );