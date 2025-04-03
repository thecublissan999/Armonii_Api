using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationApiArmonii.DTO
{
    public class DataTransferObjectMusico
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasenya { get; set; }
        public string Telefono { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? Estado { get; set; }
        public double? Valoracion { get; set; }
        public string Tipo { get; set; }

        // Musico
        public string Apodo { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public int? Edad { get; set; }
        public string Biografia { get; set; }
        public string Imagen { get; set; }
        public int IdUsuario { get; set; }

        // Local
        public string Direccion { get; set; }
        public string TipoLocal { get; set; }
        public TimeSpan? HorarioApertura { get; set; }
        public TimeSpan? HorarioCierre { get; set; }
        public string Descripcion { get; set; }
    }
}