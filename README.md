# Gestión de Biblioteca

Aplicación de escritorio desarrollada en **C# Windows Forms** para administrar libros, usuarios y préstamos de una biblioteca.

## Tecnologías utilizadas

- C#
- Windows Forms
- .NET 8
- Visual Studio Community
- Persistencia local con JSON

## Funcionalidades principales

### Libros
- Agregar libros
- Editar libros
- Eliminar libros
- Buscar libros por ID, título o autor
- Control de disponibilidad

### Usuarios
- Registrar usuarios
- Editar usuarios
- Eliminar usuarios
- Buscar usuarios por ID, nombre o correo

### Préstamos
- Registrar préstamos
- Marcar devolución de libros
- Eliminar préstamos
- Validar disponibilidad de libros

### Estadísticas
- Gráfico de libros más prestados
- Gráfico de usuarios más activos
- Gráfico de disponibilidad del inventario
- Tarjetas de resumen general

## Arquitectura MVC aplicada

El proyecto está organizado para demostrar separación de responsabilidades:

```text
Models/       Clases del sistema: Libro, Usuario, Prestamo y Persona
Controllers/  BibliotecaManager: lógica de negocio, validaciones y operaciones CRUD
Views/        MainForm: interfaz gráfica de Windows Forms
Data/         BibliotecaDataStore: guardado y carga de datos en JSON
Controls/     BarChartControl: control gráfico reutilizable
```

## Persistencia de datos

Los datos se guardan automáticamente en el archivo:

```text
datos_biblioteca.json
```

Este archivo se crea dentro de la carpeta de salida del programa al ejecutarlo.

## Cómo ejecutar el proyecto

1. Abrir Visual Studio Community.
2. Seleccionar **Open a project or solution**.
3. Abrir el archivo:

```text
Gestion de biblioteca.csproj
```

4. Presionar **F5** o el botón **Iniciar**.

## Buenas prácticas incluidas

- Validaciones de campos obligatorios
- Validación de año del libro
- Validación básica de correo electrónico
- Control de duplicados
- Confirmación antes de eliminar registros
- Separación de modelo, vista, controlador y datos
- Uso de listas, diccionarios y matriz de préstamos
- Interfaz organizada y funcional

## Autor

Gustavo Navarro
