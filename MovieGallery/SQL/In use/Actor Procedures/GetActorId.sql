CREATE PROCEDURE GetActorId
    @firstName VARCHAR(255),
    @lastName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ActorID
    FROM Actors
    WHERE FirstName = @firstName AND LastName = @lastName;
END;