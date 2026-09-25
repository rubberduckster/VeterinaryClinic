-- Performance testing script.
-- Run on a freshly created/seeded database.
-- The script inserts 10,000 test records and creates IX_Animal_Name.

USE VeterinaryClinic;
GO

DECLARE @Counter INT = 1;

WHILE @Counter <= 10000
BEGIN
    INSERT INTO Owner (Name, Phone, Email)
    VALUES (
        'Performance Owner ' + CAST(@Counter AS NVARCHAR),
        '12345678',
        'performance' + CAST(@Counter AS NVARCHAR) + '@test.com'
    );

    DECLARE @OwnerId INT = SCOPE_IDENTITY();

    INSERT INTO Animal (OwnerId, Name, DateOfBirth, Species, Breed)
    VALUES (
        @OwnerId,
        'Performance Animal ' + CAST(@Counter AS NVARCHAR),
        '2020-01-01',
        'Dog',
        'Test Breed'
    );

    SET @Counter = @Counter + 1;
END;
GO

SELECT COUNT(*) AS AnimalCount
FROM Animal;

SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

SELECT
    Animal.Name,
    Animal.Species,
    Animal.Breed,
    Owner.Name AS OwnerName
FROM Animal
INNER JOIN Owner
    ON Animal.OwnerId = Owner.Id
WHERE Animal.Name = 'Performance Animal 5000';
GO

CREATE NONCLUSTERED INDEX IX_Animal_Name
ON Animal(Name);
GO

-- Run the same query again after creating the index
SELECT
    Animal.Name,
    Animal.Species,
    Animal.Breed,
    Owner.Name AS OwnerName
FROM Animal
INNER JOIN Owner
    ON Animal.OwnerId = Owner.Id
WHERE Animal.Name = 'Performance Animal 5000';
GO

SET SHOWPLAN_TEXT ON;
GO

SELECT
    Animal.Name,
    Animal.Species,
    Animal.Breed,
    Owner.Name AS OwnerName
FROM Animal
INNER JOIN Owner
    ON Animal.OwnerId = Owner.Id
WHERE Animal.Name = 'Performance Animal 5000';
GO

SET SHOWPLAN_TEXT OFF;
GO

EXEC sp_helpindex 'Animal';
GO