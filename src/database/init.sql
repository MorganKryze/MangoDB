CREATE TYPE loyalty_rank AS ENUM(
    'Classic', 'Bronze', 'Silver', 'Gold'
);

CREATE TABLE customer (
    email VARCHAR(255) PRIMARY KEY, first_name VARCHAR(255), last_name VARCHAR(255), password VARCHAR(255), order_count INT, loyalty_rank loyalty_rank
);

INSERT INTO
    customer (
        email, first_name, last_name, password, order_count, loyalty_rank
    )
VALUES (
        'user1@example.com', 'User', 'One', 'password1', 8, 'Classic'
    ),
    (
        'user2@example.com', 'User', 'Two', 'password2', 20, 'Bronze'
    ),
    (
        'user3@example.com', 'User', 'Three', 'password3', 110, 'Gold'
    ),
    (
        'user4@example.com', 'User', 'Four', 'password4', 40, 'Bronze'
    ),
    (
        'user5@example.com', 'User', 'Five', 'password5', 52, 'Silver'
    );