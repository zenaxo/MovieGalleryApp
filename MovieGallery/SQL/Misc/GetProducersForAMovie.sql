--CREATE PROCEDURE GetProducersForMovie @MovieID int
--AS
--BEGIN
--SELECT FirstName, LastName
--FROM Producers, Movies, MovieProducers
--WHERE MovieProducers.MovieID = Movies.MovieID AND Producers.ProducerID = MovieProducers.ProducerID AND Movies.MovieID = @MovieID
--END;

EXEC GetProducersForMovie @MovieID = 1