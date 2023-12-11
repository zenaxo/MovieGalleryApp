CREATE PROCEDURE GetHeroMovie
    @MovieID INT OUTPUT
AS
BEGIN
    -- Retrieve the only MovieID in the HeroMovie table
    SELECT TOP 1 @MovieID = MovieID
    FROM HeroMovie;
END;