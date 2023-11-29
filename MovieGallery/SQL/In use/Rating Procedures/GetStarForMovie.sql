CREATE PROCEDURE GetStarForMovie
    @movieId INT,
    @rating INT,
    @total INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @total = COUNT(*)
    FROM Ratings
    WHERE MovieId = @movieId AND Rating = @rating;
END