SET NAMES utf8mb4;
SET CHARSET utf8mb4;

CREATE TABLE IF NOT EXISTS Users (
    Id int NOT NULL AUTO_INCREMENT,
    Username varchar(50) NOT NULL,
    PasswordHash varchar(255) NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT IGNORE INTO Users (Username, PasswordHash) 
VALUES ('admin', '$2a$11$RXvyZEvBcuPUAXCzXXYKDO1e6n5JkwGFLQhQvgZZV8FDz8mEXpYyO');  -- password: admin

CREATE TABLE IF NOT EXISTS Authors (
    Id int NOT NULL AUTO_INCREMENT,
    Name varchar(100) NOT NULL,
    DateOfBirth datetime NOT NULL,
    Nationality varchar(50) NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS Books (
    Id int NOT NULL AUTO_INCREMENT,
    Title varchar(200) NOT NULL,
    Description text,
    PublicationDate datetime NOT NULL,
    AuthorId int NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (AuthorId) REFERENCES Authors(Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT IGNORE INTO Authors (Name, DateOfBirth, Nationality) VALUES 
('Gabriel García Márquez', '1927-03-06', 'Colombian'),
('Jorge Luis Borges', '1899-08-24', 'Argentine');

INSERT IGNORE INTO Books (Title, Description, PublicationDate, AuthorId) VALUES 
('Cien años de soledad', N'Una saga familiar épica', '1967-05-30', 1),
('El Aleph', N'Colección de cuentos', '1949-06-15', 2); 