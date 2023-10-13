using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace BiblioAppEscritorio.ViewModel
{
    class SociosUserControlVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ApiService api;
        readonly ServicioDialogos dialogoService;


        private GridSocios socioGridSeleccionado;

        public GridSocios SocioGridSeleccionado
        {
            get { return socioGridSeleccionado; }
            set
            {
                if (TitulosPrestamos != null)
                    TitulosPrestamos.Clear();
                if (TitulosPrestamos != null)
                    TitulosReservas.Clear();
                if (value != null)
                {
                    foreach (String reserva in value.reservasSocio)
                        TitulosReservas.Add(new Mensaje(reserva));
                    foreach (String prestamo in value.prestamosSocio)
                        TitulosPrestamos.Add(new Mensaje(prestamo));
                }
                else
                {
                    if (TitulosPrestamos != null)
                        TitulosPrestamos.Clear();
                    if (TitulosPrestamos != null)
                        TitulosReservas.Clear();
                }
                SetProperty(ref socioGridSeleccionado, value);
            }
        }

        private ObservableCollection<GridSocios> listaSociosGrid;
        public ObservableCollection<GridSocios> ListaSociosGrid
        {
            get { return listaSociosGrid; }
            set { SetProperty(ref listaSociosGrid, value); }
        }
        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }
        private ObservableCollection<Mensaje> titulosReservas;

        public ObservableCollection<Mensaje> TitulosReservas
        {
            get { return titulosReservas; }
            set { SetProperty(ref titulosReservas, value); }
        }

        private ObservableCollection<Mensaje> titulosPrestamos;

        public ObservableCollection<Mensaje> TitulosPrestamos
        {
            get { return titulosPrestamos; }
            set { SetProperty(ref titulosPrestamos, value); }
        }


        //COMMANDS
        public RelayCommand RefrescaListaCommand { get; }
        public RelayCommand AddSocioCommand { get; }
        public RelayCommand EditarSocioCommand { get; }
        public RelayCommand EliminarSocioCommand { get; }


        public SociosUserControlVM()
        {
            api = new ApiService();
            dialogoService = new ServicioDialogos();
            SocioGridSeleccionado = null;
            TitulosReservas = new ObservableCollection<Mensaje>();
            TitulosPrestamos = new ObservableCollection<Mensaje>();
            ElementoSeguridadApi = api.GetElementoSeguridad();
            //COMMANDS
            RefrescaListaCommand = new RelayCommand(RefrescaLista);
            AddSocioCommand = new RelayCommand(AddSocio);
            EditarSocioCommand = new RelayCommand(EditaSocio);
            EliminarSocioCommand = new RelayCommand(EliminaSocio);

            //DATOS
            ListaSociosGrid = api.MuestraTodosLosSociosGrid(ElementoSeguridadApi);
            ListaSociosGrid.RemoveAt(0); //elimina al admin

            //SERVICIO DE MENSAJERÍA
            //manda socio seleccionado
            WeakReferenceMessenger.Default.Register<SociosUserControlVM, SocioSeleccionadoMessage>(this, (r, m) =>
            {
                m.Reply(r.SocioGridSeleccionado.socio);
            });

            //actualiza lista
            WeakReferenceMessenger.Default.Register<ActualizarGridSociosMessage>(this, (r, m) =>
            {

                if (!api.CheckApiKey(ElementoSeguridadApi).validate)
                {
                    ElementoSeguridadApi = api.GetElementoSeguridad();
                }
                ListaSociosGrid = api.MuestraTodosLosSociosGrid(ElementoSeguridadApi);
                ListaSociosGrid.RemoveAt(0); //elimina al admin
            });
        }

        //FUNCIONES DEL COMMAND
        private void RefrescaLista()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            //DATOS
            ListaSociosGrid = api.MuestraTodosLosSociosGrid(ElementoSeguridadApi);
            ListaSociosGrid.RemoveAt(0); //elimina al admin
        }

        private void AddSocio()
        {
            dialogoService.CargarDialogoAddSocio();
        }

        private void EditaSocio()
        {
            dialogoService.CargarDialogoEditaSocio();
        }

        private void EliminaSocio()
        {
            dialogoService.AbrirDialogoEliminarSocio();
            RefrescaLista();
        }
    }
}
