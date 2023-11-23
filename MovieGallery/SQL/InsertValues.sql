-- Insert data into the Movies table
INSERT INTO Movies (Title, Genre, ReleaseDate)
VALUES
    ('The Shawshank Redemption', 'Drama', '1994-09-10'),
    ('The Godfather', 'Crime', '1972-03-15'),
    ('Pulp Fiction', 'Crime', '1994-10-14');

-- Insert data into the Actors table
INSERT INTO Actors (FirstName, LastName)
VALUES
    ('Morgan', 'Freeman'),
    ('Marlon', 'Brando'),
    ('John', 'Travolta');



-- Insert data into the Producers table
INSERT INTO Producers (FirstName, LastName)
VALUES
    ('Frank', 'Darabont'),
    ('Francis', 'Ford Coppola'),
    ('Quentin', 'Tarantino');

-- Insert data into the Casts table
INSERT INTO Casts (ActorID, MovieID)
VALUES
    (1, 1),
    (2, 2),
    (3, 3);

-- Insert data into the MovieProducers table
INSERT INTO MovieProducers (ProducerID, MovieID)
VALUES
    (1, 1),
    (2, 2),
    (3, 3);

UPDATE Movies
SET MovieImage = 'the_shawshank_redemption_.jpg'
WHERE Title = 'The Shawshank Redemption'