using BiblioAppEscritorio.Clases;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;

namespace BiblioAppEscritorio.Api
{
    /// <summary>
    /// Clase encargada de realizar llamadas a la API para obtener/modificar los datos solicitados.
    /// </summary>
    class ApiService
    {
        /// <summary>
        /// Función que se encarga de obtener el 'ElementoSeguridad' que en este caso son los datos de sesión del trabajador.
        /// </summary>
        /// <returns>ElementoSeguridad con los datos de la sesión.</returns>
        public ElementoSeguridad GetElementoSeguridad()
        {
            ElementoSeguridad elementoSeguridad = CreateSesion();

            elementoSeguridad.Apikey = GetApiKey(elementoSeguridad.Cookie);

            return elementoSeguridad;
        }

        /// <summary>
        /// Función que se encarga de obtener la Key de la sesión del trabajador.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns>String con los datos de la APIKey de esa sesión.</returns>
        public string GetApiKey(RestResponseCookie cookie)
        {
            RestClient cliente = new RestClient(Properties.Settings.Default.BaseURL);

            var request = new RestRequest("/jwt/auth", Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(cookie.Name, cookie.Value);

            request.AddJsonBody("{" +
                                  $"\u0022username\u0022: \u0022{Properties.Settings.Default.DatoAdmin}\u0022," +
                                  $"\u0022password\u0022: \u0022{Properties.Settings.Default.DatoAdmin}\u0022" +
                                "}");

            IRestResponse response = cliente.Execute(request);
            JWTClass jsonReturn = JsonConvert.DeserializeObject<JWTClass>(response.Content);
            return jsonReturn.JWT.ToString();
        }

        /// <summary>
        /// Función que realiza una llamada a la api para comprobar si la sesión ha caducado o no.
        /// </summary>
        /// <param name="elementoSeguridad">Obtenemos la cookie para mandarla y realizar la petición.</param>
        /// <returns>Clase CheckJWT con los datos de la sesión, entre otros, si ha caducado o no.</returns>
        public CheckJWT CheckApiKey(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/jwt/check", Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", Properties.Settings.Default.UrlLocal);

            CheckJWT check = JsonConvert.DeserializeObject<CheckJWT>(cliente.Execute(request).Content);

            return check;
        }

        /// <summary>
        /// Función que realiza una llamada a la API para crear la sesión del trabajador.
        /// </summary>
        /// <returns>ElementoSeguridad con los datos de la sesión creada.</returns>
        public ElementoSeguridad CreateSesion()
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);

            var request = new RestRequest("/jwt/create-session", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            var response = cliente.Execute(request);
            Dictionary<string, string> jsonReturn = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ToString());

            ElementoSeguridad elementoSeguridad = new ElementoSeguridad();
            elementoSeguridad.Cookie = response.Cookies[0];
            elementoSeguridad.Session = jsonReturn["session"].ToString();


            return elementoSeguridad;
        }

        #region Libros
        /// <summary>
        /// Función que realiza llamada a la API para obtener todos los libros de la biblioteca.
        /// </summary>
        /// <returns>Lista con todos los libros de la biblioteca</returns>
        public ObservableCollection<Libros> MuestraTodosLosLibros()
        {
            var cliente = new RestClient(BiblioAppEscritorio.Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/libros/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = cliente.Execute(request);
            System.Console.WriteLine(JsonConvert.DeserializeObject<List<Libros>>(cliente.Execute(request).Content));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Libros>>(response.Content); }
            catch (Exception) { return new ObservableCollection<Libros>(); }
        }

        /// <summary>
        /// Función que obtiene los datos del libro según el id del libro.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y que la API nos identifique.</param>
        /// <param name="idLibro">El id del libro que desea buscar.</param>
        /// <returns></returns>
        public Libros GetLibroPorId(ElementoSeguridad elementoSeguridad, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libros/id/{idLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Libros>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new Libros();
            }

        }

        /// <summary>
        /// Función que realiza una petición a la API para insertar un libro en la bd.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="libro">El libro a insertar.</param>
        /// <returns>String con un mensaje para asegurar si se ha insertado correctamente el libro en la bd.</returns>
        public string CreaLibro(ElementoSeguridad elementoSeguridad, Libros libro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libros", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(libro);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para editar un libro existente.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="libro">Libro a editar.</param>
        /// <returns>String con un mensaje para asegurar si se ha editado correctamente el libro.</returns>
        public string EditaLibro(ElementoSeguridad elementoSeguridad, Libros libro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libros", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(libro);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una petición a la API para eliminar un libro existente.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="isbn">ISBN del libro a eliminar.</param>
        /// <returns>String con un mensaje para asegurar si se ha editado correctamente el libro.</returns>
        public string EliminaLibros(ElementoSeguridad elementoSeguridad, string isbn)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libros/{isbn}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        #endregion

        #region Libro
        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con los libros reservados y así obtener sus unidades.
        /// </summary>
        /// <param name="isbnLibro">ISBN del libro a buscar.</param>
        /// <returns>Número de unidades reservadas de un libro.</returns>
        public int GetUnidadesReservadasLibroPorId(string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/nreservas/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            int unidades = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(cliente.Execute(request).Content).Count;
            try { return unidades; }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con los libros disponibles y así obtener sus unidades.
        /// </summary>
        /// <param name="isbnLibro">ISBN del libro a buscar.</param>
        /// <returns>Número de unidades disponibles de un libro.</returns>
        public int GetUnidadesDisponiblesLibroPorId(string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/ndisponibles/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            int unidades = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(cliente.Execute(request).Content).Count;
            try { return unidades; }
            catch (Exception) { return 0; }
        }

        /// <summary>
        /// Función que reliza una llamada a la API para obtener una lista de libros disponibles según su ISBN.
        /// </summary>
        /// <param name="isbnLibro">ISBN del libro a obtener.</param>
        /// <returns>Lista de unidades de libros disponibles.</returns>
        public ObservableCollection<Libro> GetLibroDisponiblePorIsbn(string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/ndisponibles/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            try { return JsonConvert.DeserializeObject<ObservableCollection<Libro>>(cliente.Execute(request).Content); }
            catch (Exception) { return new ObservableCollection<Libro>(); }
        }
        /// <summary>
        /// Función que reliza una llamada a la API para obtener una lista de libros reservados según su ISBN.
        /// </summary>
        /// <param name="isbnLibro">ISBN del libro reservado a obtener.</param>
        /// <returns>Lista de unidades de libros reservados.</returns>
        public Libro GetLibroDisponibleReservaPorIsbn(string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/reserva/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            try { return JsonConvert.DeserializeObject<Libro>(cliente.Execute(request).Content); }
            catch (Exception) { return null; }
        }
        /// <summary>
        /// Función que reliza una llamada a la API para obtener las unidades de los libros a elegir.
        /// </summary>
        /// <param name="isbnLibro">ISBN del libro a consultar.</param>
        /// <returns>Clase unidades libros que devuelve las unidades del libro solicitado.</returns>
        public UnidadesLibros GetUnidadesLibros(string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/ud/isbn/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            try { return JsonConvert.DeserializeObject<UnidadesLibros>(cliente.Execute(request).Content); }
            catch (Exception) { return null; }
        }

        /// <summary>
        /// Función que reliza una llamada a la API para obtener el número de unidades totales de ese libro.
        /// </summary>
        /// <param name="isbnLibro">ISBN del libro a elegir.</param>
        /// <returns>Número de las unidades totales de un libro.</returns>
        public int GetUnidadesTotalesLibroPorId(string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            int unidades = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(cliente.Execute(request).Content).Count;
            try { return unidades; }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Función que realiza a una llamada a la API para consultar si ese libre está disponible para borrar porque no tiene reservas ni préstamos pendientes.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="isbnLibro">ISBN del libro a elegir.</param>
        /// <returns>Booleano que determina si está disponible para eliminar o no.</returns>
        public Boolean DisponibleBorrar(ElementoSeguridad elementoSeguridad, string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/disponibleBorrar/{isbnLibro}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            string mensaje = JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje;
            try { return mensaje == "Disponible";  }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para marcar como no disponible una reserva en concreto.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idLibro">Id del libro para marcar como no disponible.</param>
        /// <returns>Mensaje para saber si se ha modificado correctamente el apartado de reserva del libro.</returns>
        public string MarcaUnidadComoNoDisponibleReserva(ElementoSeguridad elementoSeguridad, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/noDisponibleReserva/{idLibro}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para marcar la unidad del libro como unidad disponible para reserva.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idLibro">Id del libro a modificar.</param>
        /// <returns>Mensaje para saber si se ha modificado correctamente el apartado de disponibilidad de reserva del libro.</returns>
        public string MarcaUnidadComoDisponibleReserva(ElementoSeguridad elementoSeguridad, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/disponibleReserva/{idLibro}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para marcar la unidad del libro como unidad no disponible.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idLibro">Id del libro a modificar.</param>
        /// <returns>Mensaje para saber si se ha modificado correctamente el apartado de reserva del libro.</returns>
        public string MarcaUnidadComoNoDisponible(ElementoSeguridad elementoSeguridad, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/noDisponible/{idLibro}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para marcar la unidad del libro como unidad disponible.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idLibro">Id del libro a modificar.</param>
        /// <returns>Mensaje para saber si se ha modificado correctamente el apartado de disponibilidad del libro.</returns>
        public string MarcaUnidadComoDisponible(ElementoSeguridad elementoSeguridad, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/disponible/{idLibro}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para insertar una unidad de un libro existente.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="isbnLibro">ISBN del libro a añadir unidad</param>
        /// <returns>Mensaje para saber si se ha insertado correctamente la unidad del libro.</returns>
        public string CreaUnidadLibro(ElementoSeguridad elementoSeguridad, String isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/{isbnLibro}", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para eliminar una unidad de un libro existente.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="isbnLibro">ISBN del libro a eliminar unidad.</param>
        /// <returns>Mensaje para saber si se ha eliminado correctamente la unidad del libro.</returns>
        public string EliminaUnidadDeLibro(ElementoSeguridad elementoSeguridad, string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/unidad/isbn/{isbnLibro}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "ERROR";
            }

        }

        /// <summary>
        /// Función que realiza una llamada a la API para eliminar todas las unidades de ese libro.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="isbnLibro">ISBN del libro a eliminar unidades.</param>
        /// <returns>Mensaje para saber si se han eliminadon correctamente todas las unidades del libro.</returns>
        public string EliminaTodasLasUnidadesDeLibro(ElementoSeguridad elementoSeguridad, string isbnLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/libro/isbn/{isbnLibro}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "ERROR";
            }

        }

        #endregion

        #region Eventos
        /// <summary>
        /// Función que realiza una llamada a la API para obtener todos los eventos existentes de la biblioteca.
        /// </summary>
        /// <returns>Lista de todos los eventos.</returns>
        public ObservableCollection<Evento> MuestraTodosLosEventos()
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/eventos/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = cliente.Execute(request);
            System.Console.WriteLine(JsonConvert.DeserializeObject<List<Libros>>(cliente.Execute(request).Content));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Evento>>(response.Content); }
            catch (Exception) { return new ObservableCollection<Evento>(); }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener los eventos de una fecha específica.
        /// </summary>
        /// <param name="fecha">Fecha específica a buscar.</param>
        /// <returns>Lista de eventos según la fecha específicada.</returns>
        public ObservableCollection<Evento> BuscaEventoPorFecha(string fecha)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/eventos/fecha/{fecha}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = cliente.Execute(request);
            System.Console.WriteLine(JsonConvert.DeserializeObject<List<Libros>>(cliente.Execute(request).Content));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Evento>>(response.Content); }
            catch (Exception) { return new ObservableCollection<Evento>(); }

        }

        /// <summary>
        /// Función que realiza una llamada a la API para insertar un evento en la BD.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="evento">Evento a insertar.</param>
        /// <returns>Mensaje para confirmar que se ha insertado correctamente el evento.</returns>
        public string CreaEventos(ElementoSeguridad elementoSeguridad, Evento evento)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/eventos", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(evento);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para editar un evento de la BD.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="evento">Evento a editar.</param>
        /// <returns>Mensaje para confirmar que se ha editado correctamente el evento.</returns>
        public string EditaEventos(ElementoSeguridad elementoSeguridad, Evento evento)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/eventos", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(evento);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }
        /// <summary>
        /// Función que realiza una llamada a la API para eliminar un evento de la BD.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idEvento">Id del evento a eliminar.</param>
        /// <returns>Mensaje para confirmar que se ha eliminado correctamente el evento.</returns>
        public string EliminaEvento(ElementoSeguridad elementoSeguridad, int idEvento)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/eventos/{idEvento}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }

        }
        #endregion

        #region Socios
        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con los socios registrados en la bd de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <returns>Lista de socios.</returns>
        public ObservableCollection<Socio> MuestraTodosLosSocios(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/socio/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Socio>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<Socio>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con los socios registrados en la bd de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <returns>Lista de socios adaptado al grid que se muestra en la aplicación.</returns>
        public ObservableCollection<GridSocios> MuestraTodosLosSociosGrid(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/socio/grid", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<GridSocios>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<GridSocios>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener los datos de un socio en concreto.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <param name="idSocio">Id del socio a obtener.</param>
        /// <returns>Socio deseado según la id.</returns>
        public Socio GetSocioPorId(ElementoSeguridad elementoSeguridad, int idSocio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/socio/id/{idSocio}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Socio>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new Socio();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener los datos de un socio en concreto según su DNI.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <param name="dni">DNI del socio a obtener.</param>
        /// <returns>Socio deseado según el DNI.</returns>
        public Socio GetSocioPorDni(ElementoSeguridad elementoSeguridad, string dni)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/socio/dni/{dni}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Socio>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener restablecer la contraseña del socio.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idSocio">Socio deseado según la id</param>
        /// <returns>Mensaje para confirmar que se ha reestablecido correctamente la contraseña.</returns>
        public string RestablecePass(ElementoSeguridad elementoSeguridad, int idSocio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/socio/resetPassword/{idSocio}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para insertar un socio en la bd de biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="socio">Socio a insertar</param>
        /// <returns>Mensaje para confirmar que se ha creado correctamente el socio.</returns>
        public string CreaSocio(ElementoSeguridad elementoSeguridad, Socio socio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/socio", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(socio);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para editar un socio de la bd de biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="socio">Socio a editar</param>
        /// <returns>Mensaje para confirmar que se ha editado correctamente el socio.</returns>
        public string EditaSocio(ElementoSeguridad elementoSeguridad, Socio socio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/socio", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(socio);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para eliminar un socio de la bd de biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="socio">Socio a eliminar</param>
        /// <returns>Mensaje para confirmar que se ha eliminado correctamente el socio.</returns>
        public string EliminaSocios(ElementoSeguridad elementoSeguridad, int idSocio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/socio/{idSocio}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }

        }
        #endregion

        #region Prestamos
        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con todos los préstamos de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <returns>Lista con todos los préstamos</returns>
        public ObservableCollection<Prestamo> MuestraTodosLosPrestamos(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/prestamos/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Prestamo>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<Prestamo>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con todos los préstamos de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <returns>Lista con todos los préstamos adaptados al Grid que se muestra en la aplicación de escritorio.</returns>
        public ObservableCollection<GridPrestamos> MuestraTodosLosPrestamosGrid(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/prestamos/grid/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<GridPrestamos>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<GridPrestamos>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para insertar un préstamo en la bd de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="prestamo">Préstamo a insertar.</param>
        /// <returns>Mensaje de confirmación para asegurar que se ha insertado correctamente.</returns>
        public string CreaPrestamo(ElementoSeguridad elementoSeguridad, Prestamo prestamo)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/prestamos/{prestamo.idLibro}/{prestamo.idSocio}", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(prestamo);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener las unidades de los préstamos sin finalizar de un socio en concreto.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idSocio">Id del socio a obtener.</param>
        /// <returns>Unidades del préstamo sin finalizar.</returns>
        public int GetUnidadesDePrestamosSinFinalizarSocio(ElementoSeguridad elementoSeguridad, int idSocio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/prestamos/noFinalizadoSocio/{idSocio}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            string a = cliente.Execute(request).Content;
            int unidades = 0;
            if (a != "")
                unidades = JsonConvert.DeserializeObject<ObservableCollection<Prestamo>>(a).Count;
            else
                return 0;
            try { return unidades; }
            catch (Exception) { return 0; }
        }
        /// <summary>
        /// Función que realiza una llamada a la API para obtener los préstamos de un socio en concreto.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idSocio">Id del socio a obtener.</param>
        /// <returns>Lista de préstamos según el socio.</returns>
        public ObservableCollection<Prestamo> BuscaPrestamosNoFinalizadosPorSocio(ElementoSeguridad elementoSeguridad, int idSocio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/prestamos/noFinalizadoSocio/{idSocio}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Prestamo>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<Prestamo>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para finalizar un préstamo. 
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idPrestamo">id del préstamo a finalizar.</param>
        /// <returns>Mensaje para confirmar que se ha finalizado el préstamo.</returns>
        public string FinalizaPrestamo(ElementoSeguridad elementoSeguridad, int idPrestamo)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/prestamos/{idPrestamo}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "Error genérico";
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para elimimnar un préstamo. 
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idPrestamo">id del préstamo a eliminar.</param>
        /// <returns>Mensaje para confirmar que se ha eliminado el préstamo.</returns>
        public string EliminaPrestamo(ElementoSeguridad elementoSeguridad, int idPrestamo)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/prestamos/{idPrestamo}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "Error genérico";
            }
        }

        #endregion

        #region Registro préstamos interbibliotecarios
        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista con todos los préstamos interbibliotecarios registrados.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <returns>Lista de préstamos interbibliotecarios registrados.</returns>
        public ObservableCollection<PrestamoInterbibliotecario> MuestraTodosLosRegistrosInterbiblitecarios(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/interbibliotecario/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<PrestamoInterbibliotecario>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<PrestamoInterbibliotecario>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para insertar un préstamo interbibliotecario.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <returns>Mensaje para confirmar que se creado el préstamo interbibliotecario.</returns>
        public string CreaRegistroInterbiblitecario(ElementoSeguridad elementoSeguridad, PrestamoInterbibliotecario prestamo)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/interbibliotecario", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(prestamo);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para editar un préstamo interbibliotecario.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <returns>Mensaje para confirmar que se ha editado el préstamo.</returns>
        public string EditaRegistroInterbiblitecario(ElementoSeguridad elementoSeguridad, PrestamoInterbibliotecario prestamo)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/interbibliotecario", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            string data = JsonConvert.SerializeObject(prestamo);
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para eliminar un préstamo interbibliotecario.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <returns>Mensaje para confirmar que se ha eliminar el préstamo.</returns>
        public string EliminaRegistroInterbiblitecario(ElementoSeguridad elementoSeguridad, int idRegistro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/interbibliotecario/{idRegistro}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }
        #endregion

        #region Reservas
        /// <summary>
        /// Función que realiza una llamada a la API para obtener todas las reservas de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <returns>Lista con todas las reservas.</returns>
        public ObservableCollection<Reserva> MuestraTodasLasReservas(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/reservas/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Reserva>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<Reserva>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener todas las reservas de la biblioteca.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <returns>Lista con todas las reservas adaptado al grid que se muestra en la aplicación de escritorio.</returns>
        public ObservableCollection<GridReservas> MuestraTodosLosReservasGrid(ElementoSeguridad elementoSeguridad)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest("/reservas/grid/all", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<GridReservas>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<GridReservas>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para finalizar una reserva.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <param name="idReserva">id de la reserva a finalizar.</param>
        /// <param name="idLibro">id del libro que tiene la reserva a finalizar.</param>
        /// <returns></returns>
        public string FinalizaReserva(ElementoSeguridad elementoSeguridad, int idReserva, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/reservas/{idReserva}/{idLibro}", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception)
            {
                return "Error genérico";
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para obtener una lista de reservas no finalizadas por un socio en concreto.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a obtener datos.</param>
        /// <param name="idSocio">id del socio a buscar la reserva.</param>
        /// <returns>Lista con las reservas no finalizadas por socio en concreto.</returns>
        public ObservableCollection<Reserva> BuscaReservasNoFinalizadasPorSocio(ElementoSeguridad elementoSeguridad, int idSocio)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/reservas/noFinalizadaSocio/{idSocio}", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<ObservableCollection<Reserva>>(cliente.Execute(request).Content); }
            catch (Exception)
            {
                return new ObservableCollection<Reserva>();
            }
        }

        /// <summary>
        /// Función que realiza una llamada a la API para crear una reserva.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idSocio">id del socio que solicita la reserva.</param>
        /// <param name="idLibro">id del libro a reservar.</param>
        /// <returns>Mensaje para confirmar que se ha creado correctamente la reserva.</returns>
        public string CreaReserva(ElementoSeguridad elementoSeguridad, int idSocio, int idLibro)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/reservas/add/{idSocio}/{idLibro}", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }


        /// <summary>
        /// Función que realiza una llamada a la API para eliminar una reserva.
        /// </summary>
        /// <param name="elementoSeguridad">Clase para poder mandar la cookie y APIKey par que la API nos identifique y tengamos acceso a modificar datos.</param>
        /// <param name="idReserva">id de la reserva a eliminar</param>
        /// <returns>Mensaje para confirmar que se ha eliminado correctamente la reserva.</returns>
        public string EliminaReservas(ElementoSeguridad elementoSeguridad, int idReserva)
        {
            var cliente = new RestClient(Properties.Settings.Default.BaseURL);
            var request = new RestRequest($"/reservas/{idReserva}", Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddCookie(elementoSeguridad.Cookie.Name, elementoSeguridad.Cookie.Value);
            request.AddQueryParameter("apikey", GetApiKey(elementoSeguridad.Cookie));
            try { return JsonConvert.DeserializeObject<Mensaje>(cliente.Execute(request).Content).mensaje; }
            catch (Exception) { return "Error genérico"; }
        }

        #endregion


    }
}
