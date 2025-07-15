using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_F5_GTS
{
    public static class StringExtensions
    {
        public static string Center(this string texto, int ancho)
        {
            if (string.IsNullOrEmpty(texto)) return new string(' ', ancho);

            int izquierda = (ancho - texto.Length) / 2;
            int derecha = ancho - texto.Length - izquierda;

            return new string(' ', izquierda) + texto + new string(' ', derecha);
        }
    }
}
