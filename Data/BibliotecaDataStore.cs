using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Gestion_de_biblioteca
{
    public class BibliotecaData
    {
        public List<Libro> Libros { get; set; } = new();
        public List<Usuario> Usuarios { get; set; } = new();
        public List<Prestamo> Prestamos { get; set; } = new();
        public int ContadorLibros { get; set; } = 1;
        public int ContadorUsuarios { get; set; } = 1;
        public int ContadorPrestamos { get; set; } = 1;
    }

    // Persistencia simple en JSON para que los datos no se pierdan al cerrar la app.
    public class BibliotecaDataStore
    {
        private readonly string _rutaArchivo;
        private readonly JsonSerializerOptions _opciones = new() { WriteIndented = true };

        public BibliotecaDataStore()
        {
            _rutaArchivo = Path.Combine(AppContext.BaseDirectory, "datos_biblioteca.json");
        }

        public BibliotecaData Cargar()
        {
            if (!File.Exists(_rutaArchivo))
                return new BibliotecaData();

            string json = File.ReadAllText(_rutaArchivo);
            return JsonSerializer.Deserialize<BibliotecaData>(json) ?? new BibliotecaData();
        }

        public void Guardar(BibliotecaData data)
        {
            string json = JsonSerializer.Serialize(data, _opciones);
            File.WriteAllText(_rutaArchivo, json);
        }
    }
}
