CREATE PROCEDURE ProducerExists
    @firstName VARCHAR(255),
    @lastName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProducerCount INT;

    SELECT @ProducerCount = COUNT(*)
    FROM Producers
    WHERE FirstName = @firstName AND LastName = @lastName;

    SELECT @ProducerCount AS ProducerCount;
END;