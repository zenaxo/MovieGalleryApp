--CREATE PROCEDURE SelectAllProducers @MovieID int
--AS
--SELECT FirstName, LastName
--FROM Producers, MovieProducers, Movies
--WHERE Producers.ProducerID = MovieProducers.MProducerID AND MovieProducers.MovieID = Movies.MovieID AND Movies.MovieID = @MovieID;

CREATE PROCEDURE CalculateAverageRating @MovieID int
AS
SELECT Ratings.MovieID, AVG(CAST(Ratings.Rating AS FLOAT)) AS AverageRating
WHERE Movies.MovieID = @MovieID;

--EXEC CalculateAverageRating @MovieID = 3;