using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace BiblioAppEscritorio.Api
{
    class ElementoSeguridad
    {

        public string Apikey { get; set; }

        public RestResponseCookie Cookie { get; set; }

        public string Session { get; set; }

        public ElementoSeguridad()
        {
        }

        public ElementoSeguridad(string apikey, RestResponseCookie cookie, string session)
        {
            Apikey = apikey;
            Cookie = cookie;
            Session = session;
        }

        public override string ToString()
        {
            var propidades = this.GetType().GetProperties();
            StringBuilder resultado = new StringBuilder();
            foreach (var propiedad in propidades)
            {
                resultado.AppendLine($"{propiedad.Name}: {propiedad.GetValue(this, null)}");
            }
            return resultado.ToString();
        }
    }
}
