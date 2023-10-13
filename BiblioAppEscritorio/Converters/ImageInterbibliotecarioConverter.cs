using BiblioAppEscritorio.ExternalApi;
using BiblioAppEscritorio.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiblioAppEscritorio.Converters
{
    class ImageInterbibliotecarioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VolumeInfo r = (VolumeInfo)value;
            return r.imageLinks != null ? r.imageLinks.thumbnail : "https://images-na.ssl-images-amazon.com/images/I/211uDEb4KPL._SY264_BO1,204,203,200_QL40_ML2_.jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
