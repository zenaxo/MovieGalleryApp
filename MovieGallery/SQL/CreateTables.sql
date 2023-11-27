CREATE TABLE Producers (
	ProducerID INT IDENTITY (1,1) PRIMARY KEY,
	FirstName VARCHAR(50),
	LastName VARCHAR(50)
);

CREATE TABLE Actors (
	ActorID INT IDENTITY (1,1) PRIMARY KEY,
	FirstName VARCHAR(50),
	LastName VARCHAR(50)
);

CREATE TABLE Movies (
	MovieID INT IDENTITY (1,1) PRIMARY KEY,
	Title VARCHAR(255) NOT NULL,
	Genre VARCHAR(50) NOT NULL,
	MovieImage VARCHAR(50) DEFAULT ('image-not-found.jpg'),
	[MovieDescription] VARCHAR(1000) NOT NULL DEFAULT ('Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'), 
	ReleaseDate DATE
);

CREATE TABLE MovieProducers (
	MProducerID INT IDENTITY (1,1) PRIMARY KEY,
	ProducerID INT,
	MovieID INT,
	FOREIGN KEY (ProducerID) REFERENCES Producers(ProducerID) ON DELETE CASCADE,
	FOREIGN KEY (MovieID) REFERENCES Movies(MovieID) ON DELETE CASCADE
);

CREATE TABLE Casts (
	CastID INT IDENTITY (1,1) PRIMARY KEY,
	ActorID INT,
	MovieID INT,
	FOREIGN KEY (ActorID) REFERENCES Actors(ActorID) ON DELETE CASCADE,
	FOREIGN KEY (MovieID) REFERENCES Movies(MovieID) ON DELETE CASCADE
);
CREATE TABLE Ratings (
    RatingID INT IDENTITY (1,1) PRIMARY KEY,
    MovieID INT,
    Rating INT,
    FOREIGN KEY (MovieID) REFERENCES Movies(MovieID) ON DELETE CASCADE
);
