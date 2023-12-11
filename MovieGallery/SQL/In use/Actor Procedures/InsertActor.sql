CREATE PROCEDURE InsertActor
    @firstName VARCHAR(50),
    @lastName VARCHAR(50),
    @actorId INT OUTPUT
AS
BEGIN
    DECLARE @InsertedRows TABLE (ActorID INT);

    INSERT INTO Actors (FirstName, LastName)
    OUTPUT INSERTED.ActorID INTO @InsertedRows
    VALUES (@firstName, @lastName);

    SELECT @actorId = ActorID FROM @InsertedRows;
END