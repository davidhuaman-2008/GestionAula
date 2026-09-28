-- ============================================
-- CREAR BASE DE DATOS
-- ============================================
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'GestionAula')
BEGIN
    CREATE DATABASE GestionAula;
END
GO
USE GestionAula;
GO

-- ============================================
-- TABLA: Grados
-- ============================================
CREATE TABLE Grados (
    GradoId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);
GO

-- ============================================
-- TABLA: Secciones
-- ============================================
CREATE TABLE Secciones (
    SeccionId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(10) NOT NULL
);
GO

-- ============================================
-- TABLA: Profesores
-- ============================================
CREATE TABLE Profesores (
    ProfesorId INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150),
    Telefono NVARCHAR(20)
);
GO

-- ============================================
-- TABLA: Alumnos
-- ============================================
CREATE TABLE Alumnos (
    AlumnoId INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(15) UNIQUE,
    FechaNacimiento DATE,
    Email NVARCHAR(150)
);
GO

-- ============================================
-- TABLA: Materias
-- ============================================
CREATE TABLE Materias (
    MateriaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);
GO

-- ============================================
-- TABLA: ProfesorMateria
-- ============================================
CREATE TABLE ProfesorMateria (
    ProfesorMateriaId INT IDENTITY(1,1) PRIMARY KEY,
    ProfesorId INT NOT NULL,
    MateriaId INT NOT NULL,
    CONSTRAINT FK_ProfMat_Profesor FOREIGN KEY (ProfesorId) REFERENCES Profesores(ProfesorId),
    CONSTRAINT FK_ProfMat_Materia FOREIGN KEY (MateriaId) REFERENCES Materias(MateriaId),
    CONSTRAINT UQ_Profesor_Materia UNIQUE (ProfesorId, MateriaId)
);
GO

-- ============================================
-- TABLA: Matriculas
-- ============================================
CREATE TABLE Matriculas (
    MatriculaId INT IDENTITY(1,1) PRIMARY KEY,
    AlumnoId INT NOT NULL,
    GradoId INT NOT NULL,
    SeccionId INT NOT NULL,
    AnioEscolar INT NOT NULL DEFAULT 2025,
    CONSTRAINT FK_Matricula_Alumno FOREIGN KEY (AlumnoId) REFERENCES Alumnos(AlumnoId),
    CONSTRAINT FK_Matricula_Grado FOREIGN KEY (GradoId) REFERENCES Grados(GradoId),
    CONSTRAINT FK_Matricula_Seccion FOREIGN KEY (SeccionId) REFERENCES Secciones(SeccionId)
);
GO

-- ============================================
-- TABLA: Notas
-- ============================================
CREATE TABLE Notas (
    NotaId INT IDENTITY(1,1) PRIMARY KEY,
    AlumnoId INT NOT NULL,
    MateriaId INT NOT NULL,
    Parcial1 DECIMAL(4,2) NULL,
    Parcial2 DECIMAL(4,2) NULL,
    Parcial3 DECIMAL(4,2) NULL,
    Parcial4 DECIMAL(4,2) NULL,
    CONSTRAINT FK_Notas_Alumno FOREIGN KEY (AlumnoId) REFERENCES Alumnos(AlumnoId),
    CONSTRAINT FK_Notas_Materia FOREIGN KEY (MateriaId) REFERENCES Materias(MateriaId),
    CONSTRAINT UQ_Alumno_Materia UNIQUE (AlumnoId, MateriaId),
    CONSTRAINT CK_Parcial1 CHECK (Parcial1 BETWEEN 0 AND 20),
    CONSTRAINT CK_Parcial2 CHECK (Parcial2 BETWEEN 0 AND 20),
    CONSTRAINT CK_Parcial3 CHECK (Parcial3 BETWEEN 0 AND 20),
    CONSTRAINT CK_Parcial4 CHECK (Parcial4 BETWEEN 0 AND 20)
);
GO

-- ============================================
-- GRADOS
-- ============================================
INSERT INTO Grados (Nombre) VALUES
('1ro Secundaria'), ('2do Secundaria'), ('3ro Secundaria');
GO

-- ============================================
-- SECCIONES
-- ============================================
INSERT INTO Secciones (Nombre) VALUES ('A'), ('B'), ('C');
GO

-- ============================================
-- PROFESORES
-- ============================================
INSERT INTO Profesores (Nombres, Apellidos, Email, Telefono) VALUES
('Carlos',   'Ramírez Soto',     'cramirez@colegio.edu',  '987654321'),
('María',    'Gonzales Pérez',   'mgonzales@colegio.edu', '987654322'),
('José',     'Huamán Quispe',    'jhuaman@colegio.edu',   '987654323'),
('Ana',      'Torres Villalobos','atorres@colegio.edu',   '987654324'),
('Luis',     'Fernández Ríos',   'lfernandez@colegio.edu', '987654325'),
('Rosa',     'Castillo Mendoza', 'rcastillo@colegio.edu', '987654326'),
('Pedro',    'Salazar Núñez',    'psalazar@colegio.edu',  '987654327'),
('Elena',    'Vargas Loayza',    'evargas@colegio.edu',   '987654328'),
('Jorge',    'Ramos Ccahuana',   'jramos@colegio.edu',    '987654329'),
('Lucía',    'Mendoza Apaza',    'lmendoza@colegio.edu',  '987654330');
GO

-- ============================================
-- MATERIAS
-- ============================================
INSERT INTO Materias (Nombre, Descripcion) VALUES
('Matemática',        'Álgebra, Aritmética y Geometría'),
('Comunicación',      'Lenguaje y Literatura'),
('Ciencias Sociales', 'Historia, Geografía y Economía'),
('Ciencia y Tecnología','Biología, Física y Química'),
('Inglés',            'Idioma extranjero'),
('Educación Física',  'Deporte y salud'),
('Arte y Cultura',    'Danzas, música y pintura'),
('Religión',          'Educación religiosa'),
('Computación',       'Ofimática y programación básica'),
('Filosofía',         'Ética y lógica');
GO

-- ============================================
-- PROFESOR - MATERIA
-- ============================================
INSERT INTO ProfesorMateria (ProfesorId, MateriaId) VALUES
(1, 1), (1, 9), (1, 10),
(2, 2), (2, 5),
(3, 3), (3, 8),
(4, 4),
(5, 6),
(6, 7),
(7, 1),
(8, 2),
(9, 3),
(10, 4);
GO

-- ============================================
-- ALUMNOS (30)
-- ============================================
INSERT INTO Alumnos (Nombres, Apellidos, DNI, FechaNacimiento, Email) VALUES
('Juan','Pérez López','70000001','2008-03-12','jperez@mail.com'),
('María','García Ruiz','70000002','2008-05-22','mgarcia@mail.com'),
('Luis','Rodríguez Díaz','70000003','2008-07-10','lrodriguez@mail.com'),
('Ana','Martínez Cruz','70000004','2008-01-15','amartinez@mail.com'),
('Pedro','Sánchez Vargas','70000005','2008-09-03','psanchez@mail.com'),
('Lucía','Flores Ramos','70000006','2008-11-28','lflores@mail.com'),
('Diego','Torres Quispe','70000007','2008-02-19','dtorres@mail.com'),
('Sofía','Rojas Mendoza','70000008','2008-06-07','srojas@mail.com'),
('Carlos','Vargas Núñez','70000009','2008-04-25','cvargas@mail.com'),
('Elena','Chávez Soto','70000010','2008-08-14','echavez@mail.com'),
('Jorge','Ríos Huamán','70000011','2008-10-30','jrios@mail.com'),
('Paola','Castro Reyes','70000012','2008-12-05','pcastro@mail.com'),
('Andrés','Silva Paredes','70000013','2008-03-08','asilva@mail.com'),
('Camila','Muñoz Tello','70000014','2008-07-17','cmunoz@mail.com'),
('Ricardo','Ortiz Lozano','70000015','2008-05-11','rortiz@mail.com'),
('Valeria','Cabrera Pinto','70000016','2008-09-21','vcabrera@mail.com'),
('Fernando','Ramos Aguirre','70000017','2008-02-02','framos@mail.com'),
('Isabella','Suárez Campos','70000018','2008-06-26','isuarez@mail.com'),
('Gabriel','Molina Herrera','70000019','2008-11-13','gmolina@mail.com'),
('Daniela','Peña Ríos','70000020','2008-04-09','dpena@mail.com'),
('Sebastián','Vega Castro','70000021','2008-08-01','svega@mail.com'),
('Antonella','Navarro León','70000022','2008-12-19','anavarro@mail.com'),
('Mateo','Bravo Salas','70000023','2008-03-27','mbravo@mail.com'),
('Renata','Cordero Luna','70000024','2008-07-06','rcordero@mail.com'),
('Emilio','Ponce Guzmán','70000025','2008-05-30','eponce@mail.com'),
('Alessia','Tapia Rojas','70000026','2008-10-08','atapia@mail.com'),
('Thiago','Reyes Bravo','70000027','2008-01-22','treyes@mail.com'),
('Micaela','Salas Prado','70000028','2008-09-15','msalas@mail.com'),
('Bruno','Fuentes Ríos','70000029','2008-06-18','bfuentes@mail.com'),
('Ariana','Luna Vega','70000030','2008-11-04','aluna@mail.com');
GO

-- ============================================
-- MATRICULAS
-- ============================================
DECLARE @i INT = 1;
WHILE @i <= 30
BEGIN
    INSERT INTO Matriculas (AlumnoId, GradoId, SeccionId, AnioEscolar)
    VALUES (@i, 1, 1, 2025);
    SET @i = @i + 1;
END
GO

-- ============================================
-- NOTAS: 30 alumnos x 10 materias = 300 filas
-- ============================================
DECLARE @alumno INT = 1;
DECLARE @materia INT;
DECLARE @p1 DECIMAL(4,2), @p2 DECIMAL(4,2), @p3 DECIMAL(4,2), @p4 DECIMAL(4,2);

WHILE @alumno <= 30
BEGIN
    SET @materia = 1;
    WHILE @materia <= 10
    BEGIN
        SET @p1 = CAST((ABS(CHECKSUM(NEWID())) % 21) AS DECIMAL(4,2));
        SET @p2 = CAST((ABS(CHECKSUM(NEWID())) % 21) AS DECIMAL(4,2));
        SET @p3 = CAST((ABS(CHECKSUM(NEWID())) % 21) AS DECIMAL(4,2));
        SET @p4 = CAST((ABS(CHECKSUM(NEWID())) % 21) AS DECIMAL(4,2));

        INSERT INTO Notas (AlumnoId, MateriaId, Parcial1, Parcial2, Parcial3, Parcial4)
        VALUES (@alumno, @materia, @p1, @p2, @p3, @p4);

        SET @materia = @materia + 1;
    END
    SET @alumno = @alumno + 1;
END
GO

----------------------
USE GestionAula;
GO
SELECT 'Grados' AS Tabla, COUNT(*) AS Registros FROM Grados
UNION ALL SELECT 'Secciones', COUNT(*) FROM Secciones
UNION ALL SELECT 'Profesores', COUNT(*) FROM Profesores
UNION ALL SELECT 'Alumnos', COUNT(*) FROM Alumnos
UNION ALL SELECT 'Materias', COUNT(*) FROM Materias
UNION ALL SELECT 'ProfesorMateria', COUNT(*) FROM ProfesorMateria
UNION ALL SELECT 'Matriculas', COUNT(*) FROM Matriculas
UNION ALL SELECT 'Notas', COUNT(*) FROM Notas;
GO

USE GestionAula;
GO

-- Borrar notas actuales
DELETE FROM Notas;
DBCC CHECKIDENT('Notas', RESEED, 0);
GO

-- Regenerar notas con distribución realista (promedio ~13)
DECLARE @alumno INT = 1;
DECLARE @materia INT;
DECLARE @p1 DECIMAL(4,2), @p2 DECIMAL(4,2), @p3 DECIMAL(4,2), @p4 DECIMAL(4,2);
DECLARE @base INT;

WHILE @alumno <= 30
BEGIN
    -- Cada alumno tiene un "nivel base" aleatorio entre 8 y 17
    SET @base = 8 + (ABS(CHECKSUM(NEWID())) % 10);

    SET @materia = 1;
    WHILE @materia <= 10
    BEGIN
        -- Variación de -3 a +3 respecto al nivel base
        SET @p1 = @base + (ABS(CHECKSUM(NEWID())) % 7) - 3;
        SET @p2 = @base + (ABS(CHECKSUM(NEWID())) % 7) - 3;
        SET @p3 = @base + (ABS(CHECKSUM(NEWID())) % 7) - 3;
        SET @p4 = @base + (ABS(CHECKSUM(NEWID())) % 7) - 3;

        -- Limitar entre 0 y 20
        IF @p1 < 0 SET @p1 = 0;
        IF @p1 > 20 SET @p1 = 20;
        IF @p2 < 0 SET @p2 = 0;
        IF @p2 > 20 SET @p2 = 20;
        IF @p3 < 0 SET @p3 = 0;
        IF @p3 > 20 SET @p3 = 20;
        IF @p4 < 0 SET @p4 = 0;
        IF @p4 > 20 SET @p4 = 20;

        INSERT INTO Notas (AlumnoId, MateriaId, Parcial1, Parcial2, Parcial3, Parcial4)
        VALUES (@alumno, @materia, @p1, @p2, @p3, @p4);

        SET @materia = @materia + 1;
    END
    SET @alumno = @alumno + 1;
END
GO

-- Verificar
SELECT 
    COUNT(*) AS TotalNotas,
    CAST(AVG((Parcial1 + Parcial2 + Parcial3 + Parcial4) / 4.0) AS DECIMAL(4,2)) AS PromedioGeneral,
    SUM(CASE WHEN (Parcial1 + Parcial2 + Parcial3 + Parcial4) / 4.0 >= 11 THEN 1 ELSE 0 END) AS Aprobados,
    SUM(CASE WHEN (Parcial1 + Parcial2 + Parcial3 + Parcial4) / 4.0 < 11 THEN 1 ELSE 0 END) AS Desaprobados
FROM Notas;
GO