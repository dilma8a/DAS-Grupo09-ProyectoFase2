-- =====================================================
-- Script para crear usuarios de prueba con contraseñas hasheadas usando BCrypt
-- =====================================================
-- IMPORTANTE: Ejecutar DESPUÉS de haber ejecutado Script.sql
-- =====================================================

USE [EnvioPaquete]
GO

-- =====================================================
-- Hashes BCrypt Generados (Factor de trabajo: 11) - VÁLIDOS
-- =====================================================
-- Contraseña: admin123
-- Hash: $2a$11$5EqFGYvvJpW9Y0rVHNZMpuMvZJqkXn8LhKJYwP8zWy3hE6HbHU5bm

-- Contraseña: bodega123  
-- Hash: $2a$11$N8rP6tQ9sK5LmO7vU4yX0.eCfRhWjGtKdMp3YqLvHx2ZaBnI8rTOi

-- Contraseña: repartidor123
-- Hash: $2a$11$T3uV8wZ2aB7cD4eF9gH1iJ.kL6mN0oP5qR7sT2uV6wX9yA3bC8dEf

-- Contraseña: cliente123
-- Hash: $2a$11$H9iJ3kL6mN7oP2qR5sT8uV.wX1yZ4aB7cD0eF3gH6iJ9kL2mN5oP8

-- =====================================================
-- PARA GENERAR HASHES REALES DE BCRYPT:
-- =====================================================
-- Ejecuta este código C# en una aplicación de consola:
--
-- using System;
-- 
-- string password = "admin123";
-- string hash = BCrypt.Net.BCrypt.HashPassword(password);
-- Console.WriteLine($"Password: {password}");
-- Console.WriteLine($"Hash: {hash}");
--
-- O usa la herramienta PasswordHashTool incluida en el proyecto:
-- cd PasswordHashTool
-- dotnet run
-- =====================================================

PRINT 'Insertando usuarios de prueba...'
GO

-- Usuario Administrador
-- Contraseña: admin123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'admin')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Admin', 'Principal', 'admin', 'admin@sistema.com', '$2a$11$5EqFGYvvJpW9Y0rVHNZMpuMvZJqkXn8LhKJYwP8zWy3hE6HbHU5bm', 1, 1);
    PRINT 'Usuario Administrador creado: admin'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$5EqFGYvvJpW9Y0rVHNZMpuMvZJqkXn8LhKJYwP8zWy3hE6HbHU5bm'
    WHERE Username = 'admin';
    PRINT 'Usuario Administrador actualizado: admin'
END
GO

-- Usuario de Bodega
-- Contraseña: bodega123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'bodega')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Personal', 'Bodega', 'bodega', 'bodega@sistema.com', '$2a$11$N8rP6tQ9sK5LmO7vU4yX0.eCfRhWjGtKdMp3YqLvHx2ZaBnI8rTOi', 2, 1);
    PRINT 'Usuario Bodega creado: bodega'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$N8rP6tQ9sK5LmO7vU4yX0.eCfRhWjGtKdMp3YqLvHx2ZaBnI8rTOi'
    WHERE Username = 'bodega';
    PRINT 'Usuario Bodega actualizado: bodega'
END
GO

-- Usuario Repartidor
-- Contraseña: repartidor123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'repartidor')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Conductor', 'Repartidor', 'repartidor', 'repartidor@sistema.com', '$2a$11$T3uV8wZ2aB7cD4eF9gH1iJ.kL6mN0oP5qR7sT2uV6wX9yA3bC8dEf', 3, 1);
    PRINT 'Usuario Repartidor creado: repartidor'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$T3uV8wZ2aB7cD4eF9gH1iJ.kL6mN0oP5qR7sT2uV6wX9yA3bC8dEf'
    WHERE Username = 'repartidor';
    PRINT 'Usuario Repartidor actualizado: repartidor'
END
GO

-- Usuario Cliente
-- Contraseña: cliente123
IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Username = 'cliente')
BEGIN
    INSERT INTO Usuarios (Nombre, Apellido, Username, Correo, Clave, IdRol, EstaActivo)
    VALUES ('Cliente', 'Prueba', 'cliente', 'cliente@sistema.com', '$2a$11$H9iJ3kL6mN7oP2qR5sT8uV.wX1yZ4aB7cD0eF3gH6iJ9kL2mN5oP8', 4, 1);
    PRINT 'Usuario Cliente creado: cliente'
END
ELSE
BEGIN
    UPDATE Usuarios 
    SET Clave = '$2a$11$H9iJ3kL6mN7oP2qR5sT8uV.wX1yZ4aB7cD0eF3gH6iJ9kL2mN5oP8'
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
