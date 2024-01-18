
using System.Net.Http;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using EjemploWeb1.Models;
using Newtonsoft.Json;
using System.Text;
using System.Web.Services.Description;

namespace EjemploWeb1.ClaseBase
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string response = consumirServicio("VersionSistema");
            // Puedes establecer cualquier valor predeterminado que desees
            ViewBag.TituloLayout = response.Replace("\"", string.Empty);
            ;
        }

        public string consumirServicio(string servicio)
        {
            string apiUrl = "http://localhost:44321/api/" + servicio;

            // Instanciar HttpClient (deberías reutilizar esta instancia si es posible)
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Realizar una solicitud GET al servicio Web API de forma síncrona
                    HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;

                    // Verificar si la solicitud fue exitosa (código de estado 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta como una cadena JSON de forma síncrona
                        string jsonContent = response.Content.ReadAsStringAsync().Result;

                        // Puedes deserializar la cadena JSON a un objeto C# si es necesario
                        // var data = JsonConvert.DeserializeObject<TuTipoDeObjeto>(jsonContent);

                        // Aquí puedes trabajar con los datos obtenidos del servicio
                        // ...

                        return jsonContent;
                    }
                    else
                    {
                        // Manejar el caso en el que la solicitud no fue exitosa de forma síncrona
                        // Puedes examinar response.StatusCode y response.ReasonPhrase para obtener más detalles
                        // ...

                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción que pueda ocurrir durante la solicitud de forma síncrona
                    // ...

                    return "Error";
                }
            }
        }

        public string RequestEliminar(string servicio, int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:44321/api/" + servicio + "/" + id);
            var responseTask = client.SendAsync(request);
            var response = responseTask.Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public string EnviarPost(string servicio, string jsonRequest)
        {
            string apiUrl = "http://localhost:44321/api/" + servicio;
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = response.Content.ReadAsStringAsync().Result;
                        return jsonResponse;
                    }
                    else
                    {
                        return "Error";
                    }
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
        }
        public string RequestService(string servicio, string jsonRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:44321/api/" + servicio);
            var content = new StringContent(jsonRequest, null, "application/json");
            request.Content = content;
            var responseTask = client.SendAsync(request);
            var response = responseTask.Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public string RequestServicePost(string servicio, string jsonRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:44321/api/" + servicio);
            var content = new StringContent(jsonRequest, null, "application/json");
            request.Content = content;
            var responseTask = client.SendAsync(request);
            var response = responseTask.Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }


        public List<ProveedoresModel> ListaConsumirServicio(string servicio)
        {
            string response = consumirServicio(servicio);
            // Deserializar la cadena JSON en una lista de objetos
            List<ProveedoresModel> Proveedores = JsonConvert.DeserializeObject<List<ProveedoresModel>>(response);
            if (Proveedores != null)
                return Proveedores;
            else
                return new List<ProveedoresModel>();
        }

        public List<ProveedoresModel> ListaConsumirServicio(string servicio, string jsonRequest)
        {
            string response = RequestServicePost(servicio, jsonRequest);
            // Deserializar la cadena JSON en una lista de objetos
            List<ProveedoresModel> Proveedores = JsonConvert.DeserializeObject<List<ProveedoresModel>>(response);
            if (Proveedores != null)
                return Proveedores;
            else
                return new List<ProveedoresModel>();
        }

        public ProveedoresModel consumirServicio(string servicio, string jsonRequest)
        {
            string response = RequestService(servicio, jsonRequest);
            // Deserializar la cadena JSON en una lista de objetos
            ProveedoresModel Proveedores = JsonConvert.DeserializeObject<ProveedoresModel>(response);
            if (Proveedores != null)
                return Proveedores;
            else
                return new ProveedoresModel();
        }
    }
}