USE [EnvioPaquete]
GO
-- =====================================
-- CREACIÓN DE TABLA DE ROLES (RolID MANUAL)
-- =====================================
CREATE TABLE Roles (
    Id INT NOT NULL PRIMARY KEY,     -- ID se ingresa manualmente
    Nombre VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200)
);

-- Insertar roles con IDs predefinidos
INSERT INTO Roles (Id, Nombre, Descripcion) VALUES
(1, 'Administrador', 'Acceso total al sistema'),
(2, 'Personal de Bodega', 'Gestiona paquetes dentro de bodega'),
(3, 'Repartidor', 'Gestiona entregas y rutas'),
(4, 'Cliente', 'Puede consultar envíos y registrar incidencias');

-- =====================================
-- CREACIÓN DE TABLA DE USUARIOS
-- =====================================
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Correo VARCHAR(150) NOT NULL UNIQUE,
    Clave VARCHAR(255) NOT NULL,
    IdRol INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    EstaActivo BIT DEFAULT 1,

    FOREIGN KEY (IdRol) REFERENCES Roles(Id)
);

INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo) VALUES ('Juan', 'Perez', 'admin', 'admin@gmail.com', '$2a$11$RFxdpApqSo/p.esMK2D5dOWVdw9xd1Adw6x8qdfHXitTBMstOpOhO', 1, 1);
INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo) VALUES ('Manuel', 'Gonzalez', 'bodega', 'bodega@gmail.com', '$2a$11$z0JbCZ4Cd/l16U1qTV3ZuOtCF9TD/y/JTPlFvuznTY0qUYPQyqvJu', 2, 1);
INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo) VALUES ('Maria', 'Fernandez', 'repartidor', 'repartidor@gmail.com', '$2a$11$ZIfM9exLuUe82.sCxgyLk.CT8dvG02T8/TJt0ufWbFGgdF8y9nPiS', 3, 1);
INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo) VALUES ('Karla', 'Fuentes', 'cliente', 'cliente@gmail.com', '$2a$11$jkWbituuLpSZq7QDLIBGve97QEY2Jda2HLNx6NAAG5wse9e0wgI4W', 4, 1);



