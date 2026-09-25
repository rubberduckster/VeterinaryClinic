USE VeterinaryClinic;
GO

CREATE OR ALTER PROCEDURE GetAnimalMedicalHistory
    @AnimalName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM Animal
        WHERE Name = @AnimalName
    )
    BEGIN
        RAISERROR('Animal not found.', 16, 1);
        RETURN;
    END;

    SELECT
        Animal.Name AS AnimalName,
        Appointment.DateTime,
        Appointment.Purpose,
        Veterinarian.Name AS VeterinarianName
    FROM Animal
    INNER JOIN HealthJournal
        ON Animal.Id = HealthJournal.AnimalId
    INNER JOIN Appointment
        ON HealthJournal.Id = Appointment.JournalId
    INNER JOIN Veterinarian
        ON Appointment.VeterinarianId = Veterinarian.Id
    WHERE Animal.Name = @AnimalName
    ORDER BY Appointment.DateTime DESC;
END;
GO