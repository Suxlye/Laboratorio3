CREATE DATABASE SistemaInventario;
GO

USE SistemaInventario;
GO

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Usuario NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(64) NOT NULL, -- SHA256 genera 64 caracteres
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Codigo NVARCHAR(50) NOT NULL UNIQUE,
    Categoria NVARCHAR(50) NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Cantidad INT NOT NULL,
    StockMinimo INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO Usuarios (Usuario, Contraseña, Activo)
VALUES ('Elmer', '123', 1);

INSERT INTO Usuarios (Usuario, Contraseña, Activo)
VALUES ('Stanley', 'hola', 1);

