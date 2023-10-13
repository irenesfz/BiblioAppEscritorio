using BiblioAppEscritorio.ExternalApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiblioAppEscritorio.Converters
{
    class IsbnInterbibliotecarioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VolumeInfo r = (VolumeInfo)value;
            return r.industryIdentifiers != null ? r.industryIdentifiers[0].identifier : "ISBN no disponible";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}