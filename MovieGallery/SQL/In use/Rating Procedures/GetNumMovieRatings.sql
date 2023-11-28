CREATE PROCEDURE GetNumberOfRatings
    @movieId INT,
    @numberOfRatings INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @numberOfRatings = COUNT(*)
    FROM Ratings
    WHERE MovieID = @movieId;
END;