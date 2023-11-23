-- Assuming you have MovieID values for the movies you inserted
-- Replace the MovieID values with the actual ones you have in your database

-- Inserting 50 to 400 ratings for 'The Shawshank Redemption'
DECLARE @MovieIDShawshank INT = 1; -- Replace with the actual MovieID
DECLARE @Counter INT = 1;

WHILE @Counter <= 400
BEGIN
    INSERT INTO Ratings (MovieID, Rating) VALUES (@MovieIDShawshank, CAST(CEILING(RAND() * 5) AS INT));
    SET @Counter = @Counter + 1;
END

-- Inserting 50 to 400 ratings for 'The Godfather'
DECLARE @MovieIDGodfather INT = 2; -- Replace with the actual MovieID
SET @Counter = 1;

WHILE @Counter <= 400
BEGIN
    INSERT INTO Ratings (MovieID, Rating) VALUES (@MovieIDGodfather, CAST(CEILING(RAND() * 5) AS INT));
    SET @Counter = @Counter + 1;
END

-- Inserting 50 to 400 ratings for 'Pulp Fiction'
DECLARE @MovieIDPulpFiction INT = 3; -- Replace with the actual MovieID
SET @Counter = 1;

WHILE @Counter <= 400
BEGIN
    INSERT INTO Ratings (MovieID, Rating) VALUES (@MovieIDPulpFiction, CAST(CEILING(RAND() * 5) AS INT));
    SET @Counter = @Counter + 1;
END