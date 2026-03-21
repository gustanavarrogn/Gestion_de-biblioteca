using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion_de_biblioteca
{
    public class BibliotecaManager
    {
        public List<Libro> Libros { get; private set; }
        public List<Usuario> Usuarios { get; private set; }
        public List<Prestamo> Prestamos { get; private set; }

        private int _contadorLibros = 1;
        private int _contadorUsuarios = 1;
        private int _contadorPrestamos = 1;

        public BibliotecaManager()
        {
            Libros = new List<Libro>();
            Usuarios = new List<Usuario>();
            Prestamos = new List<Prestamo>();

            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            AgregarLibro("El Gran Gatsby", "F. Scott Fitzgerald", 1925);
            AgregarLibro("Matar a un Ruiseñor", "Harper Lee", 1960);
            AgregarLibro("1984", "George Orwell", 1949);
            AgregarLibro("Orgullo y Prejuicio", "Jane Austen", 1813);

            AgregarUsuario("Juan Pérez", "juan@email.com");
            AgregarUsuario("María Gómez", "maria@email.com");
            AgregarUsuario("Ana Torres", "ana@email.com");
        }

        // =========================
        // CRUD LIBROS
        // =========================
        public void AgregarLibro(string titulo, string autor, int anio)
        {
            ValidarLibro(titulo, autor, anio);

            bool duplicado = Libros.Any(l =>
                l.Titulo.Trim().ToLower() == titulo.Trim().ToLower() &&
                l.Autor.Trim().ToLower() == autor.Trim().ToLower());

            if (duplicado)
                throw new Exception("Ya existe un libro con ese título y autor.");

            Libros.Add(new Libro
            {
                Id = _contadorLibros++,
                Titulo = titulo.Trim(),
                Autor = autor.Trim(),
                Anio = anio,
                Disponible = true
            });
        }

        public void EditarLibro(int id, string titulo, string autor, int anio)
        {
            ValidarLibro(titulo, autor, anio);

            Libro libro = Libros.FirstOrDefault(l => l.Id == id);
            if (libro == null)
                throw new Exception("Libro no encontrado.");

            bool duplicado = Libros.Any(l =>
                l.Id != id &&
                l.Titulo.Trim().ToLower() == titulo.Trim().ToLower() &&
                l.Autor.Trim().ToLower() == autor.Trim().ToLower());

            if (duplicado)
                throw new Exception("Ya existe otro libro con ese título y autor.");

            libro.Titulo = titulo.Trim();
            libro.Autor = autor.Trim();
            libro.Anio = anio;
        }

        public void EliminarLibro(int id)
        {
            Libro libro = Libros.FirstOrDefault(l => l.Id == id);
            if (libro == null)
                throw new Exception("Libro no encontrado.");

            bool tienePrestamoActivo = Prestamos.Any(p => p.LibroId == id && p.Estado == "Prestado");
            if (tienePrestamoActivo)
                throw new Exception("No se puede eliminar un libro con préstamo activo.");

            Libros.Remove(libro);
        }

        private void ValidarLibro(string titulo, string autor, int anio)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new Exception("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(autor))
                throw new Exception("El autor es obligatorio.");

            if (anio < 1000 || anio > DateTime.Now.Year)
                throw new Exception("El año no es válido.");
        }

        // =========================
        // CRUD USUARIOS
        // =========================
        public void AgregarUsuario(string nombre, string correo)
        {
            ValidarUsuario(nombre, correo);

            bool duplicado = Usuarios.Any(u =>
                u.CorreoElectronico.Trim().ToLower() == correo.Trim().ToLower());

            if (duplicado)
                throw new Exception("Ya existe un usuario con ese correo.");

            Usuarios.Add(new Usuario
            {
                Id = _contadorUsuarios++,
                Nombre = nombre.Trim(),
                CorreoElectronico = correo.Trim()
            });
        }

        public void EditarUsuario(int id, string nombre, string correo)
        {
            ValidarUsuario(nombre, correo);

            Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            bool duplicado = Usuarios.Any(u =>
                u.Id != id &&
                u.CorreoElectronico.Trim().ToLower() == correo.Trim().ToLower());

            if (duplicado)
                throw new Exception("Ya existe otro usuario con ese correo.");

            usuario.Nombre = nombre.Trim();
            usuario.CorreoElectronico = correo.Trim();
        }

        public void EliminarUsuario(int id)
        {
            Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            bool tienePrestamoActivo = Prestamos.Any(p => p.UsuarioId == id && p.Estado == "Prestado");
            if (tienePrestamoActivo)
                throw new Exception("No se puede eliminar un usuario con préstamo activo.");

            Usuarios.Remove(usuario);
        }

        private void ValidarUsuario(string nombre, string correo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(correo))
                throw new Exception("El correo es obligatorio.");

            if (!correo.Contains("@") || !correo.Contains("."))
                throw new Exception("El correo no tiene un formato válido.");
        }

        // =========================
        // PRESTAMOS
        // =========================
        public void RegistrarPrestamo(int usuarioId, int libroId, DateTime fechaPrestamo)
        {
            Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            Libro libro = Libros.FirstOrDefault(l => l.Id == libroId);
            if (libro == null)
                throw new Exception("Libro no encontrado.");

            if (!libro.Disponible)
                throw new Exception("El libro no está disponible.");

            Prestamos.Add(new Prestamo
            {
                Id = _contadorPrestamos++,
                UsuarioId = usuarioId,
                LibroId = libroId,
                FechaPrestamo = fechaPrestamo,
                FechaDevolucion = null,
                Estado = "Prestado"
            });

            libro.Disponible = false;
        }

        public void RegistrarDevolucion(int prestamoId, DateTime fechaDevolucion)
        {
            Prestamo prestamo = Prestamos.FirstOrDefault(p => p.Id == prestamoId);
            if (prestamo == null)
                throw new Exception("Préstamo no encontrado.");

            if (prestamo.Estado == "Devuelto")
                throw new Exception("Ese préstamo ya fue devuelto.");

            prestamo.FechaDevolucion = fechaDevolucion;
            prestamo.Estado = "Devuelto";

            Libro libro = Libros.FirstOrDefault(l => l.Id == prestamo.LibroId);
            if (libro != null)
                libro.Disponible = true;
        }

        public void EliminarPrestamo(int prestamoId)
        {
            Prestamo prestamo = Prestamos.FirstOrDefault(p => p.Id == prestamoId);
            if (prestamo == null)
                throw new Exception("Préstamo no encontrado.");

            if (prestamo.Estado == "Prestado")
            {
                Libro libro = Libros.FirstOrDefault(l => l.Id == prestamo.LibroId);
                if (libro != null)
                    libro.Disponible = true;
            }

            Prestamos.Remove(prestamo);
        }

        // =========================
        // ESTADISTICAS
        // =========================
        public Dictionary<string, int> ObtenerLibrosMasPrestados()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();

            foreach (Prestamo prestamo in Prestamos)
            {
                Libro libro = Libros.FirstOrDefault(l => l.Id == prestamo.LibroId);
                if (libro == null) continue;

                if (!resultado.ContainsKey(libro.Titulo))
                    resultado[libro.Titulo] = 0;

                resultado[libro.Titulo]++;
            }

            return resultado.OrderByDescending(x => x.Value)
                            .Take(5)
                            .ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, int> ObtenerUsuariosMasActivos()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();

            foreach (Prestamo prestamo in Prestamos)
            {
                Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == prestamo.UsuarioId);
                if (usuario == null) continue;

                if (!resultado.ContainsKey(usuario.Nombre))
                    resultado[usuario.Nombre] = 0;

                resultado[usuario.Nombre]++;
            }

            return resultado.OrderByDescending(x => x.Value)
                            .Take(5)
                            .ToDictionary(x => x.Key, x => x.Value);
        }

        // =========================
        // MATRIZ (para cumplir requisito)
        // =========================
        public string[,] GenerarMatrizPrestamos()
        {
            int filas = Prestamos.Count + 1;
            string[,] matriz = new string[filas, 5];

            matriz[0, 0] = "ID";
            matriz[0, 1] = "Usuario";
            matriz[0, 2] = "Libro";
            matriz[0, 3] = "Fecha Préstamo";
            matriz[0, 4] = "Estado";

            for (int i = 0; i < Prestamos.Count; i++)
            {
                Prestamo p = Prestamos[i];
                Usuario u = Usuarios.FirstOrDefault(x => x.Id == p.UsuarioId);
                Libro l = Libros.FirstOrDefault(x => x.Id == p.LibroId);

                matriz[i + 1, 0] = p.Id.ToString();
                matriz[i + 1, 1] = u != null ? u.Nombre : "";
                matriz[i + 1, 2] = l != null ? l.Titulo : "";
                matriz[i + 1, 3] = p.FechaPrestamo.ToShortDateString();
                matriz[i + 1, 4] = p.Estado;
            }

            return matriz;
        }
    }
}