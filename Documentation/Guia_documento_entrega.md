# Guía para el documento de entrega

## Portada
- Nombre de la institución
- Nombre de la materia
- Nombre del estudiante
- Nombre del proyecto: Gestión de Biblioteca
- Docente
- Fecha de entrega

## Introducción
El proyecto consiste en una aplicación de escritorio desarrollada en C# con Windows Forms para administrar el inventario de una biblioteca. El sistema permite registrar libros, usuarios y préstamos, aplicando validaciones y mostrando estadísticas del inventario.

## Objetivo
Desarrollar una aplicación funcional que permita gestionar libros, usuarios y préstamos mediante una interfaz gráfica clara, organizada y fácil de utilizar.

## Funcionalidades implementadas
- Registro, edición, eliminación y búsqueda de libros.
- Registro, edición, eliminación y búsqueda de usuarios.
- Registro y devolución de préstamos.
- Control automático de disponibilidad de libros.
- Estadísticas gráficas del inventario.
- Guardado automático de datos en archivo JSON.

## Arquitectura MVC
- Modelo: clases Libro, Usuario, Persona y Prestamo.
- Vista: formulario principal MainForm.
- Controlador: clase BibliotecaManager con la lógica del sistema.
- Datos: clase BibliotecaDataStore para persistencia local.

## Capturas recomendadas
1. Pantalla principal de la aplicación.
2. Registro de un libro nuevo.
3. Registro de un usuario nuevo.
4. Registro de un préstamo.
5. Devolución de un préstamo.
6. Estadísticas actualizadas.
7. Estructura del proyecto en Visual Studio.
8. Repositorio en GitHub con commits.

## Conclusión
La aplicación cumple con los requerimientos principales del proyecto, ya que permite administrar una biblioteca mediante operaciones CRUD, validaciones, interfaz gráfica, separación básica MVC, estadísticas y persistencia de datos.
