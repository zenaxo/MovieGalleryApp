CREATE PROCEDURE GetProducersForMovie
    @MovieID INT
AS
BEGIN
    SELECT Producers.ProducerID, Producers.FirstName, Producers.LastName
    FROM Producers
    JOIN MovieProducers ON Producers.ProducerID = MovieProducers.ProducerID
    JOIN Movies ON MovieProducers.MovieID = Movies.MovieID
    WHERE Movies.MovieID = @MovieID;
END;