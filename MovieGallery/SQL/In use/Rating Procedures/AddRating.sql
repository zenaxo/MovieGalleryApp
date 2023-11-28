CREATE PROCEDURE AddRating
    @movieId INT,
    @rating INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Ratings (MovieID, Rating)
    VALUES (@movieId, @rating);
END;