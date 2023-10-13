using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BiblioAppEscritorio.Converters
{
    class FechaFormatoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != "" && (string)value != null)
            {
                int posicionT = ((string)value).IndexOf("T");
                string[] datos = ((string)value).Substring(0, posicionT).Split('-');
                StringBuilder sb = new StringBuilder();
                for (int i = datos.Length - 1; i >= 0; i--) { if (i > 0) sb.Append(datos[i] + "-"); else sb.Append(datos[i]); }
                return sb.ToString();
            }
            else
            {
                return "No Establecida";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
