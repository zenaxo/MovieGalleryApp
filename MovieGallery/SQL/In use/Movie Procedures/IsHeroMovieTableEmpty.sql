CREATE PROCEDURE IsHeroMovieTableEmpty
AS
BEGIN
    -- Declare a variable to store the count of entries
    DECLARE @RowCount INT;

    -- Get the count of entries in the HeroMovie table
    SELECT @RowCount = COUNT(*)
    FROM HeroMovie;

    -- Return 1 if the table is empty, 0 otherwise
    IF @RowCount = 0
        SELECT 1 AS IsEmpty;
    ELSE
        SELECT 0 AS IsEmpty;
END;