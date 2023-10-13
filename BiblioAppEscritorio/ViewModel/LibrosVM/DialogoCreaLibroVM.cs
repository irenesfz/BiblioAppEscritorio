using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.ExternalApi;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.ViewModel.LibrosVM
{
    class DialogoCreaLibroVM : ObservableObject
    {
        //VARIABLES
        private readonly ApiService api;
        private readonly ExternalApiService apiExterna;
        private readonly ServicioAzure servicioAzure;
        private readonly ServicioDialogos servicioDialogos;
        private Libros libroACrear;
        private static readonly string filtrosRuta = "JPG (*.jpg)|*.jpg|PNG (*.png*)|*.png*";

        public Libros LibroACrear
        {
            get { return libroACrear; }
            set { SetProperty(ref libroACrear, value); }
        }
        private string imagen;

        public string Imagen
        {
            get { return imagen; }
            set { SetProperty(ref imagen, value); }
        }
        private string titulo;

        public string Titulo
        {
            get { return titulo; }
            set { SetProperty(ref titulo, value); }
        }

        private string isbn;

        public string Isbn
        {
            get { return isbn; }
            set { SetProperty(ref isbn, value); }
        }
        private string autores;

        public string Autores
        {
            get { return autores; }
            set { SetProperty(ref autores, value); }
        }
        private int anyo;

        public int Anyo
        {
            get { return anyo; }
            set { SetProperty(ref anyo, value); }
        }
        private int paginas;

        public int Paginas
        {
            get { return paginas; }
            set { SetProperty(ref paginas, value); }
        }
        private string categorias;

        public string Categorias
        {
            get { return categorias; }
            set { SetProperty(ref categorias, value); }
        }

        private string editorial;

        public string Editorial
        {
            get { return editorial; }
            set { SetProperty(ref editorial, value); }
        }

        private string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { SetProperty(ref descripcion, value); }
        }

        private string idioma;

        public string Idioma
        {
            get { return idioma; }
            set { SetProperty(ref idioma, value); }
        }



        //BUSCAR
        private Item resultadoSeleccionado;

        public Item ResultadoSeleccionado
        {
            get { return resultadoSeleccionado; }
            set { SetProperty(ref resultadoSeleccionado, value); }
        }

        private List<Item> listaResultados;
        public List<Item> ListaResultados
        {
            get { return listaResultados; }
            set { SetProperty(ref listaResultados, value); }
        }

        private string palabraABuscar;

        public string PalabraABuscar
        {
            get { return palabraABuscar; }
            set { SetProperty(ref palabraABuscar, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }

        public DialogoCreaLibroVM()
        {
            servicioDialogos = new ServicioDialogos();
            servicioAzure = new ServicioAzure();
            api = new ApiService();
            ElementoSeguridadApi = api.GetElementoSeguridad();
            apiExterna = new ExternalApiService();
            LibroACrear = new Libros();
            LimpiaFormulario();
        }
        public void LimpiaFormulario()
        {
            Imagen = "https://images-na.ssl-images-amazon.com/images/I/211uDEb4KPL._SY264_BO1,204,203,200_QL40_ML2_.jpg";
            Categorias = String.Empty;
            Isbn = String.Empty;
            Editorial = String.Empty;
            Descripcion = String.Empty;
            Autores = String.Empty;
            Anyo = 0;
            Paginas = 0;
            Titulo = String.Empty;
        }
        public void BuscaLibros()
        {
            LimpiaFormulario();
            if (PalabraABuscar != null && PalabraABuscar != String.Empty)
            {
                // Root r = apiExterna.MuestraTodosLosLibros(PalabraABuscar);
                // ListaResultados = r.items;
                VolumeInfo volumeInfo = apiExterna.MuestraUnLibro(PalabraABuscar);
                if (volumeInfo != null)
                {

                    if (volumeInfo.imageLinks != null)
                    {
                        LibroACrear.imagen = volumeInfo.imageLinks.thumbnail;
                        Imagen = volumeInfo.imageLinks.thumbnail;
                    }
                    if (volumeInfo.title != null)
                    {
                        LibroACrear.titulo = volumeInfo.title.ToString();
                        Titulo = volumeInfo.title.ToString();
                    }
                    if (volumeInfo.industryIdentifiers != null && volumeInfo.industryIdentifiers.Count >= 2)
                    {
                        LibroACrear.isbn = volumeInfo.industryIdentifiers[1].identifier.ToString();
                        Isbn = volumeInfo.industryIdentifiers[1].identifier.ToString();
                    }
                    else if (volumeInfo.industryIdentifiers != null)
                    {
                        if (volumeInfo.industryIdentifiers.Count > 0)
                        {
                            LibroACrear.isbn = volumeInfo.industryIdentifiers[0].identifier.ToString();
                            Isbn = volumeInfo.industryIdentifiers[0].identifier.ToString();
                        }
                    }

                    if (volumeInfo.authors != null)
                    {
                        List<String> autoresLista = volumeInfo.authors;
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i <= autoresLista.Count - 1; i++)
                        {
                            if (i == autoresLista.Count - 1)
                            {
                                sb.Append(autoresLista[i]);
                            }
                            else
                            {
                                sb.Append(autoresLista[i] + ";");
                            }
                        }

                        LibroACrear.autores = sb.ToString();
                        Autores = sb.ToString();
                    }
                    if (volumeInfo.pageCount != null)
                    {
                        LibroACrear.paginas = volumeInfo.pageCount;
                        Paginas = volumeInfo.pageCount;
                    }

                    if (volumeInfo.language != null)
                    {
                        LibroACrear.idioma = volumeInfo.language.ToString();
                        Idioma = volumeInfo.language.ToString();
                    }

                    if (volumeInfo.publisher != null)
                    {
                        LibroACrear.editorial = volumeInfo.publisher;
                        Editorial = volumeInfo.publisher;
                    }

                    if (volumeInfo.categories != null)
                    {
                        List<String> categoriasLista = volumeInfo.categories;
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i <= categoriasLista.Count - 1; i++)
                        {
                            if (i == categoriasLista.Count - 1)
                            {
                                sb.Append(categoriasLista[i]);
                            }
                            else
                            {
                                sb.Append(categoriasLista[i] + ";");
                            }
                        }
                        LibroACrear.categoria = sb.ToString();
                        Categorias = sb.ToString();
                    }

                    if (volumeInfo.description != null)
                    {
                        if (volumeInfo.description.Length > 1500)
                            Descripcion = volumeInfo.description.Substring(0, 1499);
                        else
                        {
                            LibroACrear.descripcion = volumeInfo.description;
                            Descripcion = volumeInfo.description;
                        }
                    }

                    if (volumeInfo.publishedDate != null)
                    {
                        LibroACrear.anyoPublicacion = int.Parse(volumeInfo.publishedDate.Split('-')[0]);
                        Anyo = int.Parse(volumeInfo.publishedDate.Split('-')[0]);
                    }


                }
            }
            else
            {
                servicioDialogos.MensajeInformativo("ERROR", "No se han encontrado coincidencias");
            }

        }

        public void CreaLibro()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            if (CamposCorrectos())
            {
                if (Editorial == null || Editorial == String.Empty)
                    LibroACrear.editorial = "Autoedición";
                else LibroACrear.editorial = Editorial;

                LibroACrear.imagen = Imagen;
                LibroACrear.categoria = Categorias;
                LibroACrear.isbn = Isbn;
                LibroACrear.descripcion = Descripcion;
                LibroACrear.autores = Autores;
                LibroACrear.anyoPublicacion = Anyo;
                LibroACrear.paginas = Paginas;
                LibroACrear.titulo = Titulo;
                string mensaje = api.CreaLibro(ElementoSeguridadApi, LibroACrear);
                if (mensaje != "Libro insertado")
                {
                    servicioDialogos.MensajeError("ERROR", "No se ha podido crear el libro");
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new ActualizarGridLibrosMessage(LibroACrear));
                    servicioDialogos.MensajeInformativo("OK", "Libro creado");
                }
            }
            else
            {
                servicioDialogos.MensajeError("ERROR", "Campos incorrectos");
            }
        }
        public void ExaminarImagen()
        {
            try
            {
                string rutaImagen = servicioDialogos.AbrirArchivoDialogo(filtrosRuta);
                string rutaAzure = servicioAzure.GuardarImagen(rutaImagen);
                LibroACrear.imagen = rutaAzure;
                Imagen = rutaAzure;
            }//System.AggregateException System.ArgumentOutOfRangeException
            catch (AggregateException)
            {
                servicioDialogos.MensajeError("ERROR", "Error al guardar la imagen");
            }
            catch (ArgumentException) { Console.WriteLine("Ha cerrado dialogo"); }
            catch (Azure.RequestFailedException)
            {
                servicioDialogos.MensajeError("ERROR", "Error con la imagen");
            }
            catch (Exception)
            {
                servicioDialogos.MensajeError("ERROR", "Error desconocido");
            }
        }

        private bool CamposCorrectos()
        {
            return Autores != string.Empty &&
                Descripcion != string.Empty &&
                Anyo != 0 &&
                Paginas != 0 &&
                Titulo != string.Empty &&
                Idioma != string.Empty &&
                Imagen != string.Empty &&
                Imagen != "https://images-na.ssl-images-amazon.com/images/I/211uDEb4KPL._SY264_BO1,204,203,200_QL40_ML2_.jpg" &&
                Isbn != string.Empty &&
                Categorias != string.Empty;
        }
    }
}