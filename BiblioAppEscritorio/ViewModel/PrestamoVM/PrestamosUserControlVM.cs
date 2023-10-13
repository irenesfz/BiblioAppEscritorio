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

namespace BiblioAppEscritorio.ViewModel
{
    class PrestamosUserControlVM : ObservableRecipient
    {
        //VARIABLES
        readonly ServicioDialogos dialogoService;
        private readonly ApiService api;

        private GridPrestamos prestamoSeleccionado;

        public GridPrestamos PrestamoSeleccionado
        {
            get { return prestamoSeleccionado; }
            set { SetProperty(ref prestamoSeleccionado, value); }
        }

        private ElementoSeguridad elementoSeguridadApi;

        public ElementoSeguridad ElementoSeguridadApi
        {
            get { return elementoSeguridadApi; }
            set { SetProperty(ref elementoSeguridadApi, value); }
        }

        private ObservableCollection<GridPrestamos> listaPrestamosGrid;

        public ObservableCollection<GridPrestamos> ListaPrestamosGrid
        {
            get { return listaPrestamosGrid; }
            set { SetProperty(ref listaPrestamosGrid, value); }
        }
        


        //COMMANDS
        public RelayCommand QuitarSeleccionPrestamoCommand { get; }
        public RelayCommand RefrescaListaCommand { get; }
        public RelayCommand FinalizarPrestamoCommand { get; }
        public RelayCommand EliminarPrestamoCommand { get; }

        public PrestamosUserControlVM()
        {
            //COMMANDS
            QuitarSeleccionPrestamoCommand = new RelayCommand(QuitarSeleccion);
            RefrescaListaCommand = new RelayCommand(RefrescaLista);
            FinalizarPrestamoCommand = new RelayCommand(FinalizaPrestamo);
            EliminarPrestamoCommand = new RelayCommand(EliminaPrestamo);

            //INICIALIZAR
            dialogoService = new ServicioDialogos();
            api = new ApiService();

            //DATOS
            PrestamoSeleccionado = null;
            ElementoSeguridadApi = api.GetElementoSeguridad();
            ListaPrestamosGrid = api.MuestraTodosLosPrestamosGrid(ElementoSeguridadApi);
            //SERVICIO DE MENSAJERÍA
            WeakReferenceMessenger.Default.Register<PrestamosUserControlVM, PrestamoGridPrestamosSeleccionadoMessage>(this, (r, m) =>
            {
                m.Reply(r.PrestamoSeleccionado);
            });

            //refresca lista ActualizarGridPrestamosMessage
            WeakReferenceMessenger.Default.Register<ActualizarGridPrestamosMessage>(this, (r, m) =>
            {
                if (!api.CheckApiKey(ElementoSeguridadApi).validate)
                {
                    ElementoSeguridadApi = api.GetElementoSeguridad();
                }
                ListaPrestamosGrid = api.MuestraTodosLosPrestamosGrid(ElementoSeguridadApi);
            });
        }

        //FUNCIONES DEL COMMAND
        private void QuitarSeleccion()
        {
            PrestamoSeleccionado = null;
        }

        private void RefrescaLista()
        {
            if (!api.CheckApiKey(ElementoSeguridadApi).validate)
            {
                ElementoSeguridadApi = api.GetElementoSeguridad();
            }
            ListaPrestamosGrid = api.MuestraTodosLosPrestamosGrid(ElementoSeguridadApi);
            PrestamoSeleccionado = null;
        }

        private void FinalizaPrestamo()
        {
            dialogoService.AbrirDialogoFinalizaPrestamo();
            RefrescaLista();
        }

        private void EliminaPrestamo()
        {
            dialogoService.AbrirDialogoEliminarPrestamo();
            RefrescaLista();
        }
    }
}
