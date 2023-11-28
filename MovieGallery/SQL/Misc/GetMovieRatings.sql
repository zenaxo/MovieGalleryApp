-- Create a stored procedure to get average rating and number of ratings for each movie
CREATE PROCEDURE GetMovieRatings
AS
BEGIN
    -- Query to get average rating for each movie
    SELECT
        MovieID,
        AVG(CAST(Rating AS FLOAT)) AS AverageRating
    INTO #AvgRatings
    FROM
        Ratings
    GROUP BY
        MovieID;

    -- Query to get the number of ratings for each movie
    SELECT
        MovieID,
        COUNT(*) AS NumberOfRatings
    INTO #NumRatings
    FROM
        Ratings
    GROUP BY
        MovieID;

    -- Combine the results using a final SELECT statement
    SELECT
        A.MovieID,
        A.AverageRating,
        B.NumberOfRatings
    FROM
        #AvgRatings A
    JOIN
        #NumRatings B ON A.MovieID = B.MovieID;

    -- Drop temporary tables
    DROP TABLE #AvgRatings;
    DROP TABLE #NumRatings;
END;