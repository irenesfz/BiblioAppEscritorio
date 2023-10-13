using BiblioAppEscritorio.ExternalApi;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Servicios
{
    /// <summary>
    /// Clase que realiza llamadas a la API pública de google para buscar información de libros.
    /// </summary>
    class ExternalApiService
    {
        /// <summary>
        /// Función que realiza una llamada a la API de google y obtiene una lista con las coincidencias según la cadena a buscar.
        /// </summary>
        /// <param name="cadenaABuscar">String por el que se buscan libros.</param>
        /// <returns>Root con los datos de las coincidencias encontradas.</returns>
        public Root MuestraTodosLosLibros(String cadenaABuscar)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("https://www.googleapis.com/books/v1/volumes?q="+ cadenaABuscar, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = cliente.Execute(request);
            try { return JsonConvert.DeserializeObject<Root>(response.Content); }
            catch (Exception) { return new Root (); }
        }

        /// <summary>
        /// Función que realiza una llamada a la API de google y obtiene el primer libro que aparezca según las coincidencias con la palabra a buscar.
        /// </summary>
        /// <param name="cadenaABuscar">String por el que se buscan libros.</param>
        /// <returns>VolumeInfo con los datos del libro encontrado.</returns>
        public VolumeInfo MuestraUnLibro(String cadenaABuscar)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("https://www.googleapis.com/books/v1/volumes?q=" + cadenaABuscar, Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = cliente.Execute(request);
            Root r = JsonConvert.DeserializeObject<Root>(response.Content);
            try { if (r.items != null) return r.items[0].volumeInfo; else return null; }
            catch (Exception) { return null; }
        }
    }
}
