--CREATE PROCEDURE GetUsers
--	@userName VARCHAR(255),
--	@password VARCHAR(255)
--AS
--BEGIN
--	SELECT UserName
--	FROM Users
--	WHERE UserName = @userName AND Pass = @password
--END;

EXEC GetUsers @userName = 'admin', @password = 'admin';