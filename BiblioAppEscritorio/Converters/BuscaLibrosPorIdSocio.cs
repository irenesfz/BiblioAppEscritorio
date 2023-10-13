using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiblioAppEscritorio.Converters
{
    class BuscaLibrosPorIdSocio : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApiService api = new ApiService();
            Socio socio = (Socio)value;
            return socio != null ? api.BuscaReservasNoFinalizadasPorSocio(api.CreateSesion(), socio.idSocio) : new ObservableCollection<Reserva>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}