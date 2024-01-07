CREATE PROCEDURE InsertMovie
    @title VARCHAR(50),
    @genre VARCHAR(50),
    @image_url NVARCHAR(1000),
	@image_large NVARCHAR(1000),
    @release_date DATE,
    @movie_description VARCHAR(MAX),
    @movieId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into Movies table
    INSERT INTO Movies (Title, Genre, MovieImage, MovieBackgroundImage, ReleaseDate, MovieDescription)
    VALUES (@title, @genre, @image_url, @image_large, @release_date, @movie_description);

    -- Get the newly inserted MovieId
    SET @movieId = SCOPE_IDENTITY();
END