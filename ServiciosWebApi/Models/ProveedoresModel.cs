using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiciosWebApi.Models
{
    public class ProveedoresModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Giro { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }
    }
}