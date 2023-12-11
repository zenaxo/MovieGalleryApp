CREATE PROCEDURE SetHeroMovie
    @InputMovieID INT
AS
BEGIN
    -- Delete all entries in the HeroMovie table
    DELETE FROM HeroMovie;

    -- Insert the new entry with the provided MovieID
    INSERT INTO HeroMovie (MovieID)
    VALUES (@InputMovieID);
END;