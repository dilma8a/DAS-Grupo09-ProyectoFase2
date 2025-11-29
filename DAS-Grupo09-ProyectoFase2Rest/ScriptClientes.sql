USE [EnvioPaquete]
GO

-- =====================================
-- CREACIÓN DE TABLA CLIENTES
-- =====================================
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    DUI VARCHAR(10) NOT NULL UNIQUE,
    Telefono VARCHAR(15) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Direccion VARCHAR(200) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    EstaActivo BIT DEFAULT 1
);
GO

-- =====================================
-- INSERTAR DATOS DE PRUEBA
-- =====================================
INSERT INTO Clientes (Nombre, DUI, Telefono, Email, Direccion)
VALUES 
('Juan Carlos Pérez', '12345678-9', '7890-1234', 'juan.perez@email.com', 'San Salvador, Colonia Escalón'),
('María Elena García', '98765432-1', '7890-5678', 'maria.garcia@email.com', 'Santa Tecla, Colonia San José'),
('Carlos Alberto López', '11223344-5', '7890-9012', 'carlos.lopez@email.com', 'Soyapango, Colonia Santa Lucía'),
('Ana Sofía Martínez', '55667788-9', '7890-3456', 'ana.martinez@email.com', 'Antiguo Cuscatlán, Residencial Los Héroes'),
('Roberto José Hernández', '44332211-0', '7890-7890', 'roberto.hernandez@email.com', 'San Miguel, Barrio El Centro');
GO

-- Verificar datos insertados
SELECT * FROM Clientes;
GO