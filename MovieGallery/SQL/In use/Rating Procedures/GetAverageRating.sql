CREATE PROCEDURE GetAverageRating
    @movieId INT,
    @averageRating FLOAT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @averageRating = AVG(CAST(Rating AS FLOAT))
    FROM Ratings
    WHERE MovieID = @movieId;
END;