using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ExcelDataReader;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Proyecto_F5_GTS
{
    class Program
    {
        static void Main(string[] args)
        {
            List<CJugador> listaJugadores = new List<CJugador>();
            cargarXML(listaJugadores);
            int opcion;
            bool resultadoOperacion = false;
            opcion = Menu.mostrarMenu();
            while (opcion != 3)
            {
                switch (opcion)
                {
                    case 1:
                        if( mostrarListado(listaJugadores) != "")
                        {
                            Console.WriteLine(mostrarListado(listaJugadores));
                            Menu.mostrarMensaje("Lista desplegada.");
                        }
                        else
                        {
                            Menu.mostrarMensaje("Lista vacia.");
                        }
                        break;
                    case 2:
                        resultadoOperacion = crearJugador(listaJugadores);
                        if (resultadoOperacion)
                        {
                            Menu.mostrarMensaje("Jugador creado con exito.");
                        }
                        break;
                }
                opcion = Menu.mostrarMenu();
            }
            guardarEnXML(listaJugadores);
            Menu.mostrarMensaje("\tHasta la proxima.");
        }

        public static bool crearJugador(List<CJugador> listaJugadores)
        {
            string nombre = Menu.leerString("Ingrese el nombre del jugador: ") ;
            return Controlador.crearJugador(nombre, listaJugadores);
        }

        public static void cargarXML( List<CJugador> listaJugadores)
        {
            string resultado = Controlador.leerXML("playerList.xml", listaJugadores);
            if (resultado == "ok")
            {
                Menu.mostrarMensaje("Archivo cargado.");
            }
            else
            {
                Menu.mostrarMensaje("Error al cargar archivo.");
            }
        }

        public static void guardarEnXML(List<CJugador> listaJugadores)
        {
            string resultado = Controlador.guardarEnXML("playerList.xml", listaJugadores);
            if (resultado == "ok")
            {
                Menu.mostrarMensaje("\tArchivo guardado con exito.");
            }
            else
            {
                Menu.mostrarMensaje("\tNo se pudo guardar. Error: " + resultado);
            }
        }

        public static string mostrarListado(List<CJugador> listaJugadores)
        {
            string lista = "";
            foreach (CJugador jugador in listaJugadores)
            {
                lista += jugador.darDatos();
            }
            return lista;
        }
    }
}