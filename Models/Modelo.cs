using System;

namespace Gestion_de_biblioteca
{
    // Clase base para demostrar herencia y polimorfismo.
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public virtual string MostrarInformacion()
        {
            return $"{Id} - {Nombre}";
        }
    }

    public class Usuario : Persona
    {
        public string CorreoElectronico { get; set; } = string.Empty;

        public override string MostrarInformacion()
        {
            return $"{Id} - {Nombre} - {CorreoElectronico}";
        }
    }

    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int Anio { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class Prestamo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; } = "Prestado";
    }
}
