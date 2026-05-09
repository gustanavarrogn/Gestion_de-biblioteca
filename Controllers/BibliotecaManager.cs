using System;
using System.Collections.Generic;
using System.Linq;

namespace Gestion_de_biblioteca
{
    // Controlador principal: contiene las reglas de negocio y separa la lógica de la interfaz.
    public class BibliotecaManager
    {
        private readonly BibliotecaDataStore _dataStore = new();
        private int _contadorLibros = 1;
        private int _contadorUsuarios = 1;
        private int _contadorPrestamos = 1;

        public List<Libro> Libros { get; private set; } = new();
        public List<Usuario> Usuarios { get; private set; } = new();
        public List<Prestamo> Prestamos { get; private set; } = new();

        public BibliotecaManager()
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            BibliotecaData data = _dataStore.Cargar();
            Libros = data.Libros;
            Usuarios = data.Usuarios;
            Prestamos = data.Prestamos;
            _contadorLibros = Math.Max(data.ContadorLibros, Libros.Any() ? Libros.Max(l => l.Id) + 1 : 1);
            _contadorUsuarios = Math.Max(data.ContadorUsuarios, Usuarios.Any() ? Usuarios.Max(u => u.Id) + 1 : 1);
            _contadorPrestamos = Math.Max(data.ContadorPrestamos, Prestamos.Any() ? Prestamos.Max(p => p.Id) + 1 : 1);

            if (!Libros.Any() && !Usuarios.Any())
                CargarDatosIniciales();
        }

        private void GuardarDatos()
        {
            _dataStore.Guardar(new BibliotecaData
            {
                Libros = Libros,
                Usuarios = Usuarios,
                Prestamos = Prestamos,
                ContadorLibros = _contadorLibros,
                ContadorUsuarios = _contadorUsuarios,
                ContadorPrestamos = _contadorPrestamos
            });
        }

        private void CargarDatosIniciales()
        {
            AgregarLibro("El Gran Gatsby", "F. Scott Fitzgerald", 1925);
            AgregarLibro("Matar a un Ruiseñor", "Harper Lee", 1960);
            AgregarLibro("1984", "George Orwell", 1949);
            AgregarLibro("Orgullo y Prejuicio", "Jane Austen", 1813);
            AgregarLibro("Don Quijote de la Mancha", "Miguel de Cervantes", 1605);

            AgregarUsuario("Juan Pérez", "juan@email.com");
            AgregarUsuario("María Gómez", "maria@email.com");
            AgregarUsuario("Ana Torres", "ana@email.com");
        }

        public void AgregarLibro(string titulo, string autor, int anio)
        {
            ValidarLibro(titulo, autor, anio);

            if (Libros.Any(l => Normalizar(l.Titulo) == Normalizar(titulo) && Normalizar(l.Autor) == Normalizar(autor)))
                throw new Exception("Ya existe un libro con ese título y autor.");

            Libros.Add(new Libro { Id = _contadorLibros++, Titulo = titulo.Trim(), Autor = autor.Trim(), Anio = anio, Disponible = true });
            GuardarDatos();
        }

        public void EditarLibro(int id, string titulo, string autor, int anio)
        {
            ValidarLibro(titulo, autor, anio);
            Libro libro = Libros.FirstOrDefault(l => l.Id == id) ?? throw new Exception("Libro no encontrado.");

            if (Libros.Any(l => l.Id != id && Normalizar(l.Titulo) == Normalizar(titulo) && Normalizar(l.Autor) == Normalizar(autor)))
                throw new Exception("Ya existe otro libro con ese título y autor.");

            libro.Titulo = titulo.Trim();
            libro.Autor = autor.Trim();
            libro.Anio = anio;
            GuardarDatos();
        }

        public void EliminarLibro(int id)
        {
            Libro libro = Libros.FirstOrDefault(l => l.Id == id) ?? throw new Exception("Libro no encontrado.");
            if (Prestamos.Any(p => p.LibroId == id && p.Estado == "Prestado"))
                throw new Exception("No se puede eliminar un libro con préstamo activo.");

            Libros.Remove(libro);
            GuardarDatos();
        }

        private void ValidarLibro(string titulo, string autor, int anio)
        {
            if (string.IsNullOrWhiteSpace(titulo)) throw new Exception("El título es obligatorio.");
            if (string.IsNullOrWhiteSpace(autor)) throw new Exception("El autor es obligatorio.");
            if (anio < 1000 || anio > DateTime.Now.Year) throw new Exception("El año no es válido.");
        }

        public void AgregarUsuario(string nombre, string correo)
        {
            ValidarUsuario(nombre, correo);

            if (Usuarios.Any(u => Normalizar(u.CorreoElectronico) == Normalizar(correo)))
                throw new Exception("Ya existe un usuario con ese correo.");

            Usuarios.Add(new Usuario { Id = _contadorUsuarios++, Nombre = nombre.Trim(), CorreoElectronico = correo.Trim() });
            GuardarDatos();
        }

        public void EditarUsuario(int id, string nombre, string correo)
        {
            ValidarUsuario(nombre, correo);
            Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == id) ?? throw new Exception("Usuario no encontrado.");

            if (Usuarios.Any(u => u.Id != id && Normalizar(u.CorreoElectronico) == Normalizar(correo)))
                throw new Exception("Ya existe otro usuario con ese correo.");

            usuario.Nombre = nombre.Trim();
            usuario.CorreoElectronico = correo.Trim();
            GuardarDatos();
        }

        public void EliminarUsuario(int id)
        {
            Usuario usuario = Usuarios.FirstOrDefault(u => u.Id == id) ?? throw new Exception("Usuario no encontrado.");
            if (Prestamos.Any(p => p.UsuarioId == id && p.Estado == "Prestado"))
                throw new Exception("No se puede eliminar un usuario con préstamo activo.");

            Usuarios.Remove(usuario);
            GuardarDatos();
        }

        private void ValidarUsuario(string nombre, string correo)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(correo)) throw new Exception("El correo es obligatorio.");
            if (!correo.Contains('@') || !correo.Contains('.')) throw new Exception("El correo no tiene un formato válido.");
        }

        public void RegistrarPrestamo(int usuarioId, int libroId, DateTime fechaPrestamo)
        {
            if (!Usuarios.Any(u => u.Id == usuarioId)) throw new Exception("Usuario no encontrado.");
            Libro libro = Libros.FirstOrDefault(l => l.Id == libroId) ?? throw new Exception("Libro no encontrado.");
            if (!libro.Disponible) throw new Exception("El libro no está disponible.");

            Prestamos.Add(new Prestamo { Id = _contadorPrestamos++, UsuarioId = usuarioId, LibroId = libroId, FechaPrestamo = fechaPrestamo, Estado = "Prestado" });
            libro.Disponible = false;
            GuardarDatos();
        }

        public void RegistrarDevolucion(int prestamoId, DateTime fechaDevolucion)
        {
            Prestamo prestamo = Prestamos.FirstOrDefault(p => p.Id == prestamoId) ?? throw new Exception("Préstamo no encontrado.");
            if (prestamo.Estado == "Devuelto") throw new Exception("Ese préstamo ya fue devuelto.");

            prestamo.FechaDevolucion = fechaDevolucion;
            prestamo.Estado = "Devuelto";

            Libro? libro = Libros.FirstOrDefault(l => l.Id == prestamo.LibroId);
            if (libro != null) libro.Disponible = true;
            GuardarDatos();
        }

        public void EliminarPrestamo(int prestamoId)
        {
            Prestamo prestamo = Prestamos.FirstOrDefault(p => p.Id == prestamoId) ?? throw new Exception("Préstamo no encontrado.");
            if (prestamo.Estado == "Prestado")
            {
                Libro? libro = Libros.FirstOrDefault(l => l.Id == prestamo.LibroId);
                if (libro != null) libro.Disponible = true;
            }

            Prestamos.Remove(prestamo);
            GuardarDatos();
        }

        public Dictionary<string, int> ObtenerLibrosMasPrestados()
        {
            return Prestamos.GroupBy(p => p.LibroId)
                .Select(g => new { Libro = Libros.FirstOrDefault(l => l.Id == g.Key)?.Titulo ?? "Sin título", Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToDictionary(x => x.Libro, x => x.Total);
        }

        public Dictionary<string, int> ObtenerUsuariosMasActivos()
        {
            return Prestamos.GroupBy(p => p.UsuarioId)
                .Select(g => new { Usuario = Usuarios.FirstOrDefault(u => u.Id == g.Key)?.Nombre ?? "Sin usuario", Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToDictionary(x => x.Usuario, x => x.Total);
        }

        public Dictionary<string, int> ObtenerDisponibilidadInventario()
        {
            return new Dictionary<string, int>
            {
                ["Disponibles"] = Libros.Count(l => l.Disponible),
                ["Prestados"] = Libros.Count(l => !l.Disponible)
            };
        }

        public string[,] GenerarMatrizPrestamos()
        {
            string[,] matriz = new string[Prestamos.Count + 1, 5];
            matriz[0, 0] = "ID";
            matriz[0, 1] = "Usuario";
            matriz[0, 2] = "Libro";
            matriz[0, 3] = "Fecha Préstamo";
            matriz[0, 4] = "Estado";

            for (int i = 0; i < Prestamos.Count; i++)
            {
                Prestamo p = Prestamos[i];
                matriz[i + 1, 0] = p.Id.ToString();
                matriz[i + 1, 1] = Usuarios.FirstOrDefault(x => x.Id == p.UsuarioId)?.Nombre ?? string.Empty;
                matriz[i + 1, 2] = Libros.FirstOrDefault(x => x.Id == p.LibroId)?.Titulo ?? string.Empty;
                matriz[i + 1, 3] = p.FechaPrestamo.ToShortDateString();
                matriz[i + 1, 4] = p.Estado;
            }

            return matriz;
        }

        private static string Normalizar(string valor) => valor.Trim().ToLowerInvariant();
    }
}
