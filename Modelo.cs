using System;

namespace Gestion_de_biblioteca
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Anio { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual string MostrarInformacion()
        {
            return $"{Id} - {Nombre}";
        }
    }

    public class Usuario : Persona
    {
        public string CorreoElectronico { get; set; }

        public override string MostrarInformacion()
        {
            return $"{Id} - {Nombre} - {CorreoElectronico}";
        }
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