CREATE PROCEDURE InsertProducer
    @firstName VARCHAR(50),
    @lastName VARCHAR(50),
    @producerId INT OUTPUT
AS
BEGIN
    DECLARE @InsertedRows TABLE (ProducerID INT);

    INSERT INTO Producers (FirstName, LastName)
    OUTPUT INSERTED.ProducerID INTO @InsertedRows
    VALUES (@firstName, @lastName);

    SELECT @producerId = ProducerID FROM @InsertedRows;
END