using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BiblioAppEscritorio.VM
{
    class ListadoLibrosVM : ObservableRecipient
    {
        //VARIABLES
        readonly ServicioDialogos dialogoService;
        readonly ApiService api;

        private string unidadesDisponiblesTextBlock;

        public string UnidadesDisponiblesTextBlock
        {
            get { return unidadesDisponiblesTextBlock; }
            set { SetProperty(ref unidadesDisponiblesTextBlock, value); }
        }

        private string unidadesReservadasTextBlock;

        public string UnidadesReservadasTextBlock
        {
            get { return unidadesReservadasTextBlock; }
            set { SetProperty(ref unidadesReservadasTextBlock, value); }
        }       
        
        private string unidadesTotalesTextBlock;

        public string UnidadesTotalesTextBlock
        {
            get { return unidadesTotalesTextBlock; }
            set { SetProperty(ref unidadesTotalesTextBlock, value); }
        }

        private Libros libroSeleccionado;

        public Libros LibroSeleccionado
        {
            get { return libroSeleccionado; }
            set
            {
                if (value != null)
                {
                    UnidadesLibros u = api.GetUnidadesLibros(value.isbn);
                    UnidadesDisponiblesTextBlock = u.unidadesDisponiblesPrestamo.ToString();
                    UnidadesReservadasTextBlock = u.unidadesReservadas.ToString();
                    UnidadesTotalesTextBlock = u.unidadesTotales.ToString();

                    BotonEliminarLibroEnabled = Int32.Parse(UnidadesTotalesTextBlock) == Int32.Parse(UnidadesDisponiblesTextBlock);
                    BotonEliminarUnidadLibroEnabled = api.DisponibleBorrar(ElementoSeguridadApi, value.isbn);
                    BotonPrestamoEnabled = Int32.Parse(UnidadesDisponiblesTextBlock) > 0;
                    BotonReservaEnabled = Int32.Parse(UnidadesTotalesTextBlock) != Int32.Parse(UnidadesReservadasTextBlock);
                }
                else
                {
                    BotonEliminarLibroEnabled = false;
                    BotonEliminarUnidadLibroEnabled = false;
                    BotonPrestamoEnabled = false;
                    BotonReservaEnabled = false;
                }
                SetProperty(ref libroSeleccionado, value);
            }
        }

        private ObservableCollection<Libros> libros;
        public ObservableCollection<Libros> Libros
        {
            get { return libros; }
            set { SetProperty(ref libros, value); }
        }

        private Boolean botonEliminarUnidadLibroEnabled;

        public Boolean BotonEliminarUnidadLibroEnabled
        {
            get { return botonEliminarUnidadLibroEnabled; }
            set { SetProperty(ref botonEliminarUnidadLibroEnabled, value); }
        }

        private Boolean botonPrestamoEnabled;

        public Boolean BotonPrestamoEnabled
        {
            get { return botonPrestamoEnabled; }
            set { SetProperty(ref botonPrestamoEnabled, value); }
        }

        private Boolean botonReservaEnabled;

        public Boolean BotonReservaEnabled
        {
            get { return botonReservaEnabled; }
            set { SetProperty(ref botonReservaEnabled, value); }
        }

        private Boolean botonEliminarLibroEnabled;

        public Boolean BotonEliminarLibroEnabled
        {
            get { return botonEliminarLibroEnabled; }
            set { SetProperty(ref botonEliminarLibroEnabled, value); }
        }

        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }


        //COMMANDS
        public RelayCommand QuitarSeleccionLibroCommand { get; }
        public RelayCommand RefrescaListaCommand { get; }
        public RelayCommand AddLibroCommand { get; }
        public RelayCommand AddPrestamoCommand { get; }
        public RelayCommand AddReservaCommand { get; }
        public RelayCommand EditarLibroCommand { get; }
        public RelayCommand EliminarLibroCommand { get; }
        public RelayCommand BuscarLibroCommand { get; }
        public RelayCommand RestaUnidadCommand { get; }
        public RelayCommand SumaUnidadCommand { get; }

        //ctor
        public ListadoLibrosVM()
        {
            api = new ApiService();
            //COMMANDS
            RefrescaListaCommand = new RelayCommand(RefrescaLista);
            QuitarSeleccionLibroCommand = new RelayCommand(QuitarSeleccion);
            AddLibroCommand = new RelayCommand(AddLibro);
            AddPrestamoCommand = new RelayCommand(AddPrestamo);
            AddReservaCommand = new RelayCommand(AddReserva);
            EditarLibroCommand = new RelayCommand(EditaLibro);
            EliminarLibroCommand = new RelayCommand(EliminaLibro);
            RestaUnidadCommand = new RelayCommand(RestaUnidad);
            SumaUnidadCommand = new RelayCommand(SumaUnidad);

            //DATOS
            Libros = api.MuestraTodosLosLibros();
            LibroSeleccionado = null;
            dialogoService = new ServicioDialogos();
            BotonEliminarUnidadLibroEnabled = false;
            BotonPrestamoEnabled = false;
            BotonReservaEnabled = false;
            BotonEliminarLibroEnabled = false;
            ElementoSeguridadApi = api.GetElementoSeguridad();

            //SERVICIO DE MENSAJERÍA
            WeakReferenceMessenger.Default.Register<ListadoLibrosVM, LibroSeleccionadoMessage>(this, (r, m) =>
            {
                m.Reply(r.LibroSeleccionado);
            });

            //refresca lista ActualizarGridLibrosMessage
            WeakReferenceMessenger.Default.Register<ActualizarGridLibrosMessage>(this, (r, m) =>
            {
                Libros = api.MuestraTodosLosLibros();
            });
        }



        //FUNCIONES DEL COMMAND
        private void RefrescaLista()
        {
            Libros = api.MuestraTodosLosLibros();
        }
        private void QuitarSeleccion()
        {
            LibroSeleccionado = null;
        }
        private void ActualizaUnidades()
        {
            UnidadesLibros u = api.GetUnidadesLibros(LibroSeleccionado.isbn);
            UnidadesDisponiblesTextBlock = u.unidadesDisponiblesPrestamo.ToString();
            UnidadesReservadasTextBlock = u.unidadesReservadas.ToString();
            UnidadesTotalesTextBlock = u.unidadesTotales.ToString();

            BotonEliminarLibroEnabled = Int32.Parse(UnidadesTotalesTextBlock) == Int32.Parse(UnidadesDisponiblesTextBlock);
            BotonEliminarUnidadLibroEnabled = api.DisponibleBorrar(ElementoSeguridadApi, LibroSeleccionado.isbn);
            BotonPrestamoEnabled = Int32.Parse(UnidadesDisponiblesTextBlock) > 0;
            BotonReservaEnabled = Int32.Parse(UnidadesTotalesTextBlock) != Int32.Parse(UnidadesReservadasTextBlock);
        }
        private void SumaUnidad()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            string mensaje = api.CreaUnidadLibro(ElementoSeguridadApi, LibroSeleccionado.isbn);
            if (mensaje != "Unidad de libro insertada")
            {
                dialogoService.MensajeError("ERROR", "Error al añadir unidad, por favor pruebe más tarde");
            }
            else
            {
                //RefrescaLista();
                int suma = Int32.Parse(UnidadesTotalesTextBlock)+1;
                UnidadesTotalesTextBlock = suma.ToString();
                ActualizaUnidades();
            }
        }

        private void RestaUnidad()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            string mensaje = api.EliminaUnidadDeLibro(ElementoSeguridadApi, LibroSeleccionado.isbn);
            if (mensaje != "Unidad eliminada")
            {
                dialogoService.MensajeError("ERROR", "Error al añadir unidad, por favor pruebe más tarde");
            }
            else
            {
                int resta = Int32.Parse(UnidadesTotalesTextBlock) - 1;
                UnidadesTotalesTextBlock = resta.ToString();
                if (UnidadesTotalesTextBlock == "0")
                    BotonEliminarUnidadLibroEnabled = false;
                ActualizaUnidades();
            }

        }

        private void AddLibro()
        {
            dialogoService.AbrirDialogoAddLibro();
        }
        private void AddPrestamo()
        {
            dialogoService.AbrirDialogoAddPrestamo();
            ActualizaUnidades();
        }

        private void AddReserva()
        {
            dialogoService.AbrirDialogoAddReserva();
            ActualizaUnidades();
        }


        private void EditaLibro()
        {
            dialogoService.AbrirDialogoEditarLibro();
        }

        private void EliminaLibro()
        {
            dialogoService.AbrirDialogoEliminarLibro();
        }
    }
}
