--CREATE PROCEDURE SelectAllProducers @MovieID int
--AS
--SELECT FirstName, LastName
--FROM Producers, MovieProducers, Movies
--WHERE Producers.ProducerID = MovieProducers.MProducerID AND MovieProducers.MovieID = Movies.MovieID AND Movies.MovieID = @MovieID;

--CREATE PROCEDURE GetMovieRatings @MovieID int
--AS
--SELECT AVG(CAST(Ratings.Rating AS FLOAT)) AS AverageRating, COUNT(Ratings.Rating) AS RatingsCount
--FROM Ratings, Movies
--WHERE Movies.MovieID = @MovieID AND Ratings.MovieID = @MovieID
--GROUP BY Ratings.MovieID;

--EXEC SelectAllProducers @MovieID = 3;
--EXEC GetMovieRatings @MovieID = 3;