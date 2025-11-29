-- =====================================================
-- Script para crear usuarios de prueba con contraseñas hasheadas usando BCrypt
-- =====================================================
-- IMPORTANTE: Ejecutar DESPUÉS de haber ejecutado Script.sql
-- =====================================================

USE [EnvioPaquete]
GO

-- =====================================================
-- Hashes BCrypt VÁLIDOS Generados
-- =====================================================
-- Contraseña: admin123
-- Hash: $2a$11$wpsOcz7UznswVHTaJHlf.uJ5Heas4FXDP8VQQMb/mC9oO9VKe1ogy

-- Contraseña: bodega123  
-- Hash: $2a$11$ddik7/sa/hI36hBAAl.8ae4jcmeicXT874fEuAXUX91CTKi3pjscO

-- Contraseña: repartidor123
-- Hash: $2a$11$6Nxl2/Sm/LwyMm9CWw89tefNnNEgpdnEAM6UOV.9SBE7MnKtfx1g6

-- Contraseña: cliente123
-- Hash: $2a$11$v5qLwri0Z4vZ35aBj1r9nOwzrpsomDJ5CIBo0XkZwRtZFJaM3bOaO

-- =====================================================
-- NOTA: Estos hashes fueron generados usando el endpoint
-- POST /api/seguridad/generar-hash de la API REST
-- =====================================================

PRINT 'Insertando usuarios de prueba...'
GO

-- Usuario Administrador
-- Contraseña: admin123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'admin')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Admin', 'Principal', 'admin', 'admin@sistema.com', '$2a$11$wpsOcz7UznswVHTaJHlf.uJ5Heas4FXDP8VQQMb/mC9oO9VKe1ogy', 1, 1);
    PRINT 'Usuario Administrador creado: admin'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$wpsOcz7UznswVHTaJHlf.uJ5Heas4FXDP8VQQMb/mC9oO9VKe1ogy'
    WHERE Username = 'admin';
    PRINT 'Usuario Administrador actualizado: admin'
END
GO

-- Usuario de Bodega
-- Contraseña: bodega123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'bodega')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Personal', 'Bodega', 'bodega', 'bodega@sistema.com', '$2a$11$ddik7/sa/hI36hBAAl.8ae4jcmeicXT874fEuAXUX91CTKi3pjscO', 2, 1);
    PRINT 'Usuario Bodega creado: bodega'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$ddik7/sa/hI36hBAAl.8ae4jcmeicXT874fEuAXUX91CTKi3pjscO'
    WHERE Username = 'bodega';
    PRINT 'Usuario Bodega actualizado: bodega'
END
GO

-- Usuario Repartidor
-- Contraseña: repartidor123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'repartidor')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Conductor', 'Repartidor', 'repartidor', 'repartidor@sistema.com', '$2a$11$6Nxl2/Sm/LwyMm9CWw89tefNnNEgpdnEAM6UOV.9SBE7MnKtfx1g6', 3, 1);
    PRINT 'Usuario Repartidor creado: repartidor'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$6Nxl2/Sm/LwyMm9CWw89tefNnNEgpdnEAM6UOV.9SBE7MnKtfx1g6'
    WHERE Username = 'repartidor';
    PRINT 'Usuario Repartidor actualizado: repartidor'
END
GO

-- Usuario Cliente
-- Contraseña: cliente123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'cliente')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Cliente', 'Prueba', 'cliente', 'cliente@sistema.com', '$2a$11$v5qLwri0Z4vZ35aBj1r9nOwzrpsomDJ5CIBo0XkZwRtZFJaM3bOaO', 4, 1);
    PRINT 'Usuario Cliente creado: cliente'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$v5qLwri0Z4vZ35aBj1r9nOwzrpsomDJ5CIBo0XkZwRtZFJaM3bOaO'
    WHERE Username = 'cliente';
    PRINT 'Usuario Cliente actualizado: cliente'
END
GO

-- =====================================================
-- Verificar usuarios creados
-- =====================================================
PRINT ''
PRINT '===== USUARIOS CREADOS ====='
SELECT 
    u.Id, 
    u.Nombre, 
    u.Apellido, 
    u.Correo, 
    r.Nombre as Rol, 
    u.EstaActivo,
    u.FechaCreacion
FROM Usuarios u
INNER JOIN Roles r ON u.IdRol = r.Id
ORDER BY u.Id;

PRINT ''
PRINT '===== CREDENCIALES DE PRUEBA ====='
PRINT 'Username / Password:'
PRINT 'Administrador: admin / admin123'
PRINT 'Bodega: bodega / bodega123'
PRINT 'Repartidor: repartidor / repartidor123'
PRINT 'Cliente: cliente / cliente123'
PRINT ''
PRINT 'NOTA: El login se realiza con el USERNAME, no con el correo.'
PRINT ''

GO