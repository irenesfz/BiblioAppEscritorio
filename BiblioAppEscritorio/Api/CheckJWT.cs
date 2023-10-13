using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BiblioAppEscritorio.Api
{
    class CheckJWT
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string IssuedAt { get; set; }
        public string Expiration { get; set; }
        public string usuario { get; set; }
        public string id_sesion_recibida { get; set; }
        public string id_sesion_actual { get; set; }
        public bool validate_session { get; set; }
        public bool validate_expiration { get; set; }
        public bool validate { get; set; }
        public string resul { get; set; }

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
