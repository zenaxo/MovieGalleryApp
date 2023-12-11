CREATE PROCEDURE InsertCast
    @actorId INT,
    @movieId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Casts (ActorID, MovieID)
        VALUES (@actorId, @movieId);
END