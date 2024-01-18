using EjemploWeb1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using EjemploWeb1.ClaseBase;
using System.Web.Services.Description;
using Newtonsoft.Json;


namespace EjemploWeb1.Controllers
{

    /// <summary>
    /// El patrón de diseño Modelo-Vista-Controlador (MVC) 
    /// es una arquitectura de software que divide la aplicación en tres componentes principales: 
    /// Modelo, Vista y Controlador. 
    /// El objetivo principal del patrón MVC es separar la lógica de negocio (modelo) de la presentación (vista) 
    /// y la gestión de la interacción del usuario (controlador). 
    /// Esta separación facilita la modularidad, el mantenimiento y la escalabilidad del código.
    /// </summary>
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            string response = consumirServicio("Candidato");
            ViewBag.Candidato = response.Replace("\"", string.Empty);
            return View();
        }
      
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Proveedores(int? pagina)
        {
            // Obtener datos de personas (simulado para el ejemplo)
            List<ProveedoresModel> personas = ListaConsumirServicio("ObtenerProveedores");

            // Página predeterminada en 1 si no se especifica ninguna
            int numeroDePagina = pagina ?? 1;

            // Crear una lista paginada utilizando PagedList
            IPagedList<ProveedoresModel> elementosPaginados = personas.ToPagedList(numeroDePagina, 5);

            return View(elementosPaginados);
        }


        public ActionResult editarRegistro(ProveedoresModel editedData)
        {
            if (editedData.Id == 0)
            {
                // Registro nuevo
                string jsonRequest = JsonConvert.SerializeObject(editedData, Formatting.Indented);
                List<ProveedoresModel> personas = ListaConsumirServicio("CrearProveedor", jsonRequest);
            }
            else
            {
                // Actualizar registro
                string jsonRequest = JsonConvert.SerializeObject(editedData, Formatting.Indented);
                ProveedoresModel personas = consumirServicio("ActualizarProveedor", jsonRequest);
            }

            return RedirectToAction("Proveedores"); // O redirige a la vista que necesites
        }
        public ActionResult eliminarRegistro(ProveedoresModel editedData)
        {
            // Eliminar registro
            string response = RequestEliminar("EliminarProveedor", editedData.Id);
            return RedirectToAction("Proveedores"); // O redirige a la vista que necesites
        }

    }
}