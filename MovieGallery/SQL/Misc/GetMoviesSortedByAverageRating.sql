CREATE PROCEDURE GetMoviesSortedByAverageRating
AS
BEGIN
    SELECT m.*
    FROM Movies m
    LEFT JOIN (
        SELECT MovieID, AVG(Rating) AS AvgRating
        FROM Ratings
        GROUP BY MovieID
    ) r ON m.MovieID = r.MovieID
    ORDER BY r.AvgRating DESC
END;