-- Run after VeterinaryClinic.sql and StoredProcedures.sql

USE master;
GO

CREATE LOGIN clinicUser
WITH PASSWORD = 'YOUR_USER_PASSWORD';
GO

CREATE LOGIN clinicAdmin
WITH PASSWORD = 'YOUR_ADMIN_PASSWORD';
GO


USE VeterinaryClinic;
GO

CREATE USER clinicUser
FOR LOGIN clinicUser;
GO

CREATE USER clinicAdmin
FOR LOGIN clinicAdmin;
GO

USE VeterinaryClinic;
GO

GRANT SELECT, INSERT, UPDATE
TO clinicUser;
GO

DENY DELETE
TO clinicUser;
GO

GRANT SELECT, INSERT, UPDATE, DELETE
TO clinicAdmin;
GO

EXECUTE AS USER = 'clinicUser';

SELECT * FROM Animal;

REVERT;

EXECUTE AS USER = 'clinicUser';

DELETE FROM Animal
WHERE Id = -999;

REVERT;

EXECUTE AS USER = 'clinicAdmin';

DELETE FROM Animal
WHERE Id = -999;

REVERT;

USE VeterinaryClinic;
GO

GRANT EXECUTE ON GetAnimalMedicalHistory TO clinicUser;
GRANT EXECUTE ON GetAnimalMedicalHistory TO clinicAdmin;
GO