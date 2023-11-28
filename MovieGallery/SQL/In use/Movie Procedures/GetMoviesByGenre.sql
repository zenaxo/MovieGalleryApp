-- Create a stored procedure
CREATE PROCEDURE GetMoviesByGenre
    @genre NVARCHAR(255)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Your SELECT query
    SELECT
        MovieID,
        Title,
        Genre,
        MovieImage,
        ReleaseDate,
        MovieDescription
    FROM
        Movies
    WHERE
        Genre = @genre COLLATE SQL_Latin1_General_CP1_CI_AS;
END;