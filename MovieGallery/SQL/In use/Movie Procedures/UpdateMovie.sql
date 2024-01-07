CREATE PROCEDURE UpdateMovie
    @movieId INT,
    @title VARCHAR(50),
    @genre VARCHAR(50),
    @image_url NVARCHAR(1000),
    @image_large NVARCHAR(1000),
    @release_date DATE,
    @movie_description VARCHAR(MAX)
AS
BEGIN
    UPDATE Movies
    SET
        Title = @title,
        Genre = @genre,
        MovieImage = @image_url,
        MovieBackgroundImage = @image_large,
        ReleaseDate = @release_date,
        MovieDescription = @movie_description
    WHERE MovieID = @movieId;
END;