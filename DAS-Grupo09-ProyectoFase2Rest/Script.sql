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

-- =====================================
-- EJEMPLO: INSERTAR USUARIO ADMINISTRADOR
-- =====================================
INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol)
VALUES ('Admin', 'Principal', 'admin', 'admin@sistema.com', 'HASH_AQUI', 1);


