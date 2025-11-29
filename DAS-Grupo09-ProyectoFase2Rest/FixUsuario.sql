USE EnvioPaquete;
GO

-- Actualizar usuario admin con hash BCrypt válido
-- Contraseña: admin123
UPDATE Usuarios 
SET Clave = '$2a$11$g7Sl.wL/nVTZbQrVlbNLmelKYmTY/XQcg7Qe.hUAJHsE/km8AGE82'
WHERE Username = 'admin';

-- Si hay más usuarios, actualizarlos también

GO

PRINT 'Contraseñas actualizadas correctamente';
GO