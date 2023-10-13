using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiblioAppEscritorio.Converters
{
    class VisibilidadBotonEliminarSocioPresamoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApiService api = new ApiService();
            Socio socio = (Socio)value;
            if (socio != null)
                //si no es 0, tiene libros en préstamo y no puede eliminarse el socio
                return api.GetUnidadesDePrestamosSinFinalizarSocio(api.CreateSesion(), socio.idSocio) == 0;
            else return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
