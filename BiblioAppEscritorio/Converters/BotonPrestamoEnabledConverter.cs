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
    class BotonPrestamoEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApiService api = new ApiService();
            Libros libro = (Libros)value;
            if (libro != null)
                return api.GetUnidadesDisponiblesLibroPorId(libro.isbn) > 0;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
