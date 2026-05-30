CREATE DATABASE IF NOT EXISTS paypals_db;
USE paypals_db;

-- =========================================
-- TABEL: person
-- =========================================
CREATE TABLE person (
    person_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

-- =========================================
-- TABEL: product
-- =========================================
CREATE TABLE product (
    product_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    is_alcoholic BOOLEAN NOT NULL DEFAULT FALSE
);

-- =========================================
-- TABEL: shopping_list
-- =========================================
CREATE TABLE shopping_list (
    shopping_list_id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(150) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    payer_id INT NULL,

    CONSTRAINT fk_shopping_list_payer
        FOREIGN KEY (payer_id)
        REFERENCES person(person_id)
        ON DELETE SET NULL
        ON UPDATE CASCADE
);

-- =========================================
-- TABEL: shopping_list_person
-- =========================================
CREATE TABLE shopping_list_person (
    shopping_list_person_id INT AUTO_INCREMENT PRIMARY KEY,
    shopping_list_id INT NOT NULL,
    person_id INT NOT NULL,
    pays_for_alcohol BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT fk_slp_list
        FOREIGN KEY (shopping_list_id)
        REFERENCES shopping_list(shopping_list_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_slp_person
        FOREIGN KEY (person_id)
        REFERENCES person(person_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT uq_slp UNIQUE (shopping_list_id, person_id)
);

-- =========================================
-- TABEL: shopping_list_product
-- =========================================
CREATE TABLE shopping_list_product (
    shopping_list_product_id INT AUTO_INCREMENT PRIMARY KEY,
    shopping_list_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,

    CONSTRAINT fk_slpr_list
        FOREIGN KEY (shopping_list_id)
        REFERENCES shopping_list(shopping_list_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT fk_slpr_product
        FOREIGN KEY (product_id)
        REFERENCES product(product_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT chk_quantity CHECK (quantity > 0),

    CONSTRAINT uq_slpr UNIQUE (shopping_list_id, product_id)
);

-- =========================================
-- TESTDATA PERSONEN
-- =========================================
INSERT INTO person (name)
VALUES
('Zina'),
('Emma');

-- =========================================
-- TESTDATA PRODUCTEN
-- =========================================
INSERT INTO product (name, price, is_alcoholic)
VALUES
('Cola', 3.50, FALSE),
('Bier 6-pack', 8.99, TRUE);

-- =========================================
-- TESTDATA SHOPPING LIST
-- =========================================
INSERT INTO shopping_list (title, payer_id)
VALUES
('BBQ Avond', 1);

-- =========================================
-- PERSONEN KOPPELEN AAN LIJST
-- =========================================
INSERT INTO shopping_list_person
(shopping_list_id, person_id, pays_for_alcohol)
VALUES
(1, 1, TRUE),
(1, 2, FALSE);

-- =========================================
-- PRODUCTEN KOPPELEN AAN LIJST
-- =========================================
INSERT INTO shopping_list_product
(shopping_list_id, product_id, quantity)
VALUES
(1, 1, 2),
(1, 2, 1);
