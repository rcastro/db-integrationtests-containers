CREATE TABLE Movie (
    Id INT PRIMARY KEY,
    [Name] VARCHAR(50),
    Episode INT,
    Director VARCHAR(50)
);
GO

INSERT INTO Movie VALUES (1, 'The Phantom Menace', 1, 'George Lucas');
INSERT INTO Movie VALUES (2, 'Attack of the Clones', 2, 'George Lucas');
INSERT INTO Movie VALUES (3, 'Revenge of the Sith', 3, 'George Lucas');
INSERT INTO Movie VALUES (4, 'A New Hope', 4, 'George Lucas');
INSERT INTO Movie VALUES (5, 'The Empire Strikes Back', 5, 'Irvin Kershner');
INSERT INTO Movie VALUES (6, 'Return of the Jedi', 6, 'Richard Marquand');
INSERT INTO Movie VALUES (7, 'The Force Awakens', 7, 'J.J. Abrams');
INSERT INTO Movie VALUES (8, 'The Last Jedi', 8, 'Rian Johnson');
INSERT INTO Movie VALUES (9, 'The Rise of Skywalker', 9, 'J.J. Abrams');
GO