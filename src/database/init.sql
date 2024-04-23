CREATE TABLE loyalty_ranks (
    name VARCHAR(255) PRIMARY KEY, min_points INT, max_points INT, discount DECIMAL(3, 2)
);

INSERT INTO
    loyalty_ranks (
        name, min_points, max_points, discount
    )
VALUES ('Classic', 0, 9, 0),
    ('Bronze', 10, 49, 0.05),
    ('Silver', 50, 99, 0.1),
    ('Gold', 100, 500, 0.2);

CREATE TABLE customer (
    email VARCHAR(255) PRIMARY KEY, first_name VARCHAR(255), last_name VARCHAR(255), password VARCHAR(255), order_count INT, loyalty_rank VARCHAR(255) REFERENCES loyalty_ranks (name)
);

INSERT INTO
    customer (
        email, first_name, last_name, password, order_count, loyalty_rank
    )
VALUES (
        'john.doe@example.com', 'John', 'Doe', 'jDoe123!', 8, 'Classic'
    ),
    (
        'jane.smith@example.com', 'Jane', 'Smith', 'jSmith456!', 25, 'Bronze'
    ),
    (
        'mike.jones@example.com', 'Mike', 'Jones', 'mJones789!', 120, 'Gold'
    ),
    (
        'sarah.johnson@example.com', 'Sarah', 'Johnson', 'sJohnson321!', 45, 'Bronze'
    ),
    (
        'paul.williams@example.com', 'Paul', 'Williams', 'pWilliams654!', 60, 'Silver'
    ),
    (
        'lisa.brown@example.com', 'Lisa', 'Brown', 'lBrown987!', 30, 'Bronze'
    ),
    (
        'james.davis@example.com', 'James', 'Davis', 'jDavis654!', 40, 'Bronze'
    ),
    (
        'patricia.miller@example.com', 'Patricia', 'Miller', 'pMiller321!', 50, 'Silver'
    ),
    (
        'robert.wilson@example.com', 'Robert', 'Wilson', 'rWilson987!', 60, 'Silver'
    ),
    (
        'linda.moore@example.com', 'Linda', 'Moore', 'lMoore654!', 70, 'Gold'
    );

CREATE TABLE mango_manager (
    email VARCHAR(255) PRIMARY KEY, first_name VARCHAR(255), last_name VARCHAR(255), password VARCHAR(255), working_hours TIME, salary DECIMAL(10, 2)
);

INSERT INTO
    mango_manager (
        email, first_name, last_name, password, working_hours, salary
    )
VALUES (
        'manager@mango.fr', 'Manager', 'Smith', 'mPassword123!', '08:00:00', 5000.00
    );