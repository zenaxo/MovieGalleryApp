CREATE PROCEDURE GetProducerId
    @firstName VARCHAR(255),
    @lastName VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProducerID
    FROM Producers
    WHERE FirstName = @firstName AND LastName = @lastName;
END;