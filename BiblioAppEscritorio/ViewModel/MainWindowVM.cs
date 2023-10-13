using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BiblioAppEscritorio.ViewModel
{
    class MainWindowVM : ObservableObject
    {
        readonly NavigationService navigation;
        readonly ApiService api;
        private UserControl contenidoVentana;
        public UserControl ContenidoVentana
        {
            get { return contenidoVentana; }
            set { SetProperty(ref contenidoVentana, value); }
        }

        public RelayCommand AbrirListaLibrosCommand { get; }
        public RelayCommand AbrirListaPrestamosCommand { get; }
        public RelayCommand AbrirListaReservasCommand { get; }
        public RelayCommand AbrirListaSociosCommand { get; }
        public RelayCommand AbrirListaEventosCommand { get; }
        public RelayCommand AbrirAyudaCommand { get; }
        public RelayCommand AbrirInicioCommand { get; }
        public RelayCommand AbrirInterbibliotecarioCommand { get; }
        public RelayCommand AbrirRegistroCommand { get; }

        public MainWindowVM()
        {
            navigation = new NavigationService();
            api = new ApiService();
            //COMMAND
            AbrirListaLibrosCommand = new RelayCommand(AbrirListaLibros);
            AbrirListaPrestamosCommand = new RelayCommand(AbrirListaPrestamos);
            AbrirListaReservasCommand = new RelayCommand(AbrirListaReservas);
            AbrirListaSociosCommand = new RelayCommand(AbrirListaSocios);
            AbrirListaEventosCommand = new RelayCommand(AbrirListaEventos);
            AbrirInicioCommand = new RelayCommand(AbrirInicio);
            AbrirAyudaCommand = new RelayCommand(AbrirAyuda);
            AbrirInterbibliotecarioCommand = new RelayCommand(AbrirInterbibliotecario);
            AbrirRegistroCommand = new RelayCommand(AbrirRegistro);
            //al cargar la app se muestre la pantalla de inicio
            ContenidoVentana = navigation.AbrirInicioUserControl();
        }



        //FUNCIONES DEL COMMAND

        private void AbrirListaLibros()
        {
            ContenidoVentana = navigation.AbrirLibrosUserControl();
        }
        private void AbrirListaPrestamos()
        {
            ContenidoVentana = navigation.AbrirPrestamosUserControl();
        }
        private void AbrirInterbibliotecario()
        {
            ContenidoVentana = navigation.AbrirPrestamoInterbibliotecarioUserControl();
        }
        private void AbrirRegistro()
        {
            ContenidoVentana = navigation.AbrirRegistroPrestamoInterbiblitoecario();
        }

        private void AbrirListaReservas()
        {
            ContenidoVentana = navigation.AbrirReservasUserControl();
        }

        private void AbrirListaSocios()
        {
            ContenidoVentana = navigation.AbrirSociosUserControl();
        }

        private void AbrirListaEventos()
        {
            ContenidoVentana = navigation.AbrirEventosUserControl();
        }

        private void AbrirInicio()
        {
            ContenidoVentana = navigation.AbrirInicioUserControl();
        }

        private void AbrirAyuda()
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.RutaManual);
        }


    }
}
