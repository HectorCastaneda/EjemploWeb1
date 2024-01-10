using Newtonsoft.Json;
using ServiciosWebApi.ArchivoJson;
using ServiciosWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;



namespace ServiciosWebApi.Controllers
{
    public class GapsiApiServiceController : ApiController
    {
        private static List<ProveedoresModel> Proveedores;
        private ITextWriter simpleWriter;
        private ITextWriter loggingWriter;

        private GapsiApiServiceController()
        {
            // Obteniendo directorio del pryecto
            string directorioProyecto = AppDomain.CurrentDomain.BaseDirectory;
            // Crear una instancia de escritura simple
            simpleWriter = new SimpleTextWriter(directorioProyecto + "\\ArchivoJson\\archivoProveedores.json");
            // Decorar la instancia con funcionalidad de registro
            loggingWriter = new LoggingTextWriterDecorator(simpleWriter);
            // Creando el archivo si no existe
            loggingWriter.Inicializa();


            // Obtener el texto del archivo
            string obtenerTextoArchivo = loggingWriter.ToStringText();
            
            // Deserializar la cadena JSON en una lista de objetos
            Proveedores = JsonConvert.DeserializeObject<List<ProveedoresModel>>(obtenerTextoArchivo);
        }


        [HttpGet]
        [Route("api/ObtenerProveedores")]
        public IHttpActionResult ObtenerProveedores()
        {
            return Ok(Proveedores);
        }

        [HttpGet]
        [Route("api/ObtenerProveedores/{id}")]
        public IHttpActionResult ObtenerProveedores(int id)
        {
            var producto = Proveedores.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        [Route("api/CrearProveedor")]
        public IHttpActionResult CrearProveedor(ProveedoresModel producto)
        {
            if (Proveedores != null || Proveedores.Count == 0)
                producto.Id = Proveedores.Max(Proveedores => Proveedores.Id) + 1;
            else
            {
                producto.Id = 1;
                Proveedores = new List<ProveedoresModel>();
            }
 

            bool nombreNoDuplicado = !Proveedores.Any(p => p.Nombre == producto.Nombre && p.Giro == producto.Giro);

            if (nombreNoDuplicado)
            {
                // Agrega el nuevo producto a la lista
                Proveedores.Add(producto);
                // Convertir la lista a JSON
                string jsonLista = JsonConvert.SerializeObject(Proveedores, Formatting.Indented);
                // Utilizar la instancia decorada para escribir en el archivo
                loggingWriter.FlushText();
                loggingWriter.WriteText(jsonLista);
                return Ok(Proveedores);
            }
            else
            {
                // El nombre y giro estan duplicados
                return Conflict();
            }
            
        }

        [HttpPut]
        [Route("api/ActualizarProveedor")]
        public IHttpActionResult ActualizarProveedor(ProveedoresModel producto)
        {
            int id = producto.Id;
            var existingProducto = Proveedores.FirstOrDefault(p => p.Id == id);
            if (existingProducto == null)
            {
                return NotFound();
            }

            existingProducto.Nombre = producto.Nombre;
            existingProducto.Giro = producto.Giro;
            existingProducto.Direccion = producto.Direccion;
            existingProducto.Descripcion = producto.Descripcion;
            existingProducto.Precio = producto.Precio;

            // Convertir la lista a JSON
            string jsonLista = JsonConvert.SerializeObject(Proveedores, Formatting.Indented);
            // Utilizar la instancia decorada para escribir en el archivo
            loggingWriter.FlushText();
            loggingWriter.WriteText(jsonLista);

            return Ok(existingProducto);
        }

        [HttpDelete]
        [Route("api/EliminarProveedor/{id}")]
        public IHttpActionResult EliminarProveedor(int id)
        {
            var producto = Proveedores.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            Proveedores.Remove(producto);
            // Convertir la lista a JSON
            string jsonLista = JsonConvert.SerializeObject(Proveedores, Formatting.Indented);
            // Utilizar la instancia decorada para escribir en el archivo
            loggingWriter.FlushText();
            loggingWriter.WriteText(jsonLista);
            return Ok(producto);
        }

        [HttpGet]
        [Route("api/VersionSistema")]
        public IHttpActionResult VersionSistema()
        {
            return Ok("Version 1.0");
        }

        [HttpGet]
        [Route("api/Candidato")]
        public IHttpActionResult Candidato()
        {
            return Ok("Bienvenido Candidato 01");
        }


        private static List<ProveedoresModel> ListObtenerProveedores()
        {
            // Aquí simulamos algunos datos de ejemplo en lo que hacemos el servicio.
            return new List<ProveedoresModel>
            {
                new ProveedoresModel { Id = 1, Nombre = "Juan", Giro = "Nominas", Direccion = "Ciudad A", Descripcion = "Proveedor nominas", Precio = "1000" },
                new ProveedoresModel { Id = 2, Nombre = "María", Giro = "Transportes", Direccion = "Ciudad B", Descripcion = "Proveedor transporte", Precio = "2000" },
                new ProveedoresModel { Id = 3, Nombre = "Carlos", Giro = "Publicidad", Direccion = "Ciudad C", Descripcion = "Proveedor comercial", Precio = "3000" }
            };

        }
    }

    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
    }
    
}
