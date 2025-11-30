# 📦 Sistema de Gestión de Envíos y Paquetería  
### Proyecto Académico — Desarrollo de Aplicaciones de Software (Fase 2)

Este proyecto implementa un **sistema de gestión de envíos y paquetería** compuesto por dos aplicaciones:

- **API REST (Backend)** desarrollada en ASP.NET Core Web API  
- **Aplicación MVC (Frontend)** desarrollada en ASP.NET Core MVC  

El objetivo es permitir la gestión completa de Clientes, Paquetes, Envíos y Reclamos mediante una arquitectura modular, segura y escalable.

---

## 🚀 Tecnologías Utilizadas

### 🔧 Backend — API REST
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server
- JWT (JSON Web Tokens) para autenticación
- Swagger para pruebas de endpoints
- Inyección de dependencias

### 🎨 Frontend — MVC
- ASP.NET Core MVC (.NET 8)
- Razor Views
- Bootstrap 5
- HttpClient para consumir la API REST
- Sesiones para almacenar token JWT

---

## 📂 Estructura del Proyecto

/DAS-Grupo09-ProyectoFase2 → Proyecto MVC (frontend)
/DAS-Grupo09-ProyectoFase2Rest → API REST (backend)
/README.md → Documentación del proyecto

# 🔐 Sistema de Autenticación (JWT)

1. El usuario inicia sesión desde el MVC.
2. El MVC envía credenciales a **/api/auth/login** de la API.
3. La API valida y devuelve un **JWT token**.
4. El token se guarda en la sesión del MVC.
5. Cada request del MVC hacia la API incluye:

---

Si el token expira, se redirige al login automáticamente.

---

# 📦 Módulos Principales

## 1️⃣ Clientes
- Registrar cliente  
- Editar información  
- Eliminar (soft delete o hard delete según configuración)  
- Validación de campos  
- Listado ordenado  

## 2️⃣ Paquetes
- Crear nuevo paquete  
- Relación con Cliente  
- Código de barra  
- Peso, dimensiones y descripción  
- Listado con filtros básicos  

## 3️⃣ Envíos
- Registrar un envío  
- Relación con Paquete  
- Origen y destino  
- Fechas de salida y entrega  
- Estado del envío  

## 4️⃣ Reclamos
- Crear reclamo  
- Asignación por cliente o por envío  
- Estados del reclamo  
- Fecha de creación y resolución  
- Respuestas y seguimiento  

---

# 🛠️ Configuración de la Base de Datos

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=PaqueteriaDB;Trusted_Connection=True;TrustServerCertificate=True"
}

