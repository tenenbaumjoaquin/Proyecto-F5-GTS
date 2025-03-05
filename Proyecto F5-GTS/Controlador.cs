using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDataReader;
using System.Threading.Tasks;

namespace Proyecto_F5_GTS
{
    //Controlador STATIC para invocar las funciones sin instanciar
    static class Controlador
    {
        //Metodo de busqueda en lista, para corroborar que el elemento a insertar no se encuentra en la lista
        public static CJugador buscar(List<CJugador> listaJugadores, string nombre)
        {
            int indice = listaJugadores.IndexOf(new CJugador(nombre));
            if (indice == -1)
            {
                return null;
            }
            else
            {
                return listaJugadores[indice];
            }
        }
        public static bool crearJugador( int ID, string nombre,  )
    }
}
