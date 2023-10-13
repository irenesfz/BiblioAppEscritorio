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
    class VisibilidadBotonEliminarLibroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApiService api = new ApiService();
            Libros libro = (Libros)value;
            if (libro != null)
                //si están las mismas unidades totales que disponibles,
                //significaría que no hay ninguno en préstamo y se podría eliminar el libro
                return api.GetUnidadesTotalesLibroPorId(libro.isbn) == api.GetUnidadesDisponiblesLibroPorId(libro.isbn);
            else return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
