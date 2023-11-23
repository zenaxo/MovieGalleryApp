SELECT
    MovieID,
    AVG(CAST(Rating AS FLOAT)) AS AverageRating
FROM
    Ratings
GROUP BY
    MovieID;

SELECT
    MovieID,
    COUNT(*) AS NumberOfRatings
FROM
    Ratings
GROUP BY
    MovieID;