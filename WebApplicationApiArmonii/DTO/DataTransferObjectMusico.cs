using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationApiArmonii.DTO
{
    public class DataTransferObjectMusico
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string contrasenya { get; set; }
        public string telefono { get; set; }
        public double? latitud { get; set; }
        public double? longitud { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public bool? estado { get; set; }
        public double? valoracion { get; set; }
        public string tipo { get; set; }

        // Musico
        public string apodo { get; set; }
        public string apellido { get; set; }
        public string genero { get; set; }
        public int? edad { get; set; }
        public string biografia { get; set; }
        public string imagen { get; set; }
        public int idUsuario { get; set; }
        public List<String> generosMusicales { get; set; }

        // Local
        public string direccion { get; set; }
        public string tipoLocal { get; set; }
        public TimeSpan? HorarioApertura { get; set; }
        public TimeSpan? HorarioCierre { get; set; }
        public string descripcion { get; set; }
    }
}