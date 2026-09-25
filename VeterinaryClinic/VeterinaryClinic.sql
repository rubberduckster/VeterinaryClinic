CREATE DATABASE VeterinaryClinic;
GO

USE VeterinaryClinic;
GO

CREATE TABLE Owner
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100)
);

CREATE TABLE Animal
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    OwnerId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    Species NVARCHAR(50) NOT NULL,
    Breed NVARCHAR(100),

    FOREIGN KEY (OwnerId) REFERENCES Owner(Id)
);

CREATE TABLE Veterinarian
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE HealthJournal
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    AnimalId INT NOT NULL UNIQUE,

    FOREIGN KEY (AnimalId) REFERENCES Animal(Id)
);

CREATE TABLE Appointment
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    JournalId INT NOT NULL,
    VeterinarianId INT NOT NULL,
    DateTime DATETIME2 NOT NULL,
    Purpose NVARCHAR(255),

    FOREIGN KEY (JournalId) REFERENCES HealthJournal(Id),
    FOREIGN KEY (VeterinarianId) REFERENCES Veterinarian(Id)
);

CREATE TABLE Diagnosis
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    AppointmentId INT NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Notes NVARCHAR(500),

    FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id)
);

CREATE TABLE Treatment
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    AppointmentId INT NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Notes NVARCHAR(500),

    FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id)
);

CREATE TABLE Medication
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    AppointmentId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Dosage NVARCHAR(100),
    Instructions NVARCHAR(500),

    CONSTRAINT CK_Medication_Name
    CHECK (LEN(Name) > 0),

    FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id)
);