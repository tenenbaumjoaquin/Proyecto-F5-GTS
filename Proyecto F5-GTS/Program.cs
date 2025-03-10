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
            List<Jugador> listaJugadores = new List<Jugador>();
            CargarXML(listaJugadores);
            int opcion;
            bool resultadoOperacion = false;
            opcion = Menu.MostrarMenu();
            while (opcion != 3)
            {
                switch (opcion)
                {
                    case 1:
                        if( MostrarListado(listaJugadores) != "")
                        {
                            Console.WriteLine(MostrarListado(listaJugadores));
                            Menu.MostrarMensaje("Lista desplegada.");
                        }
                        else
                        {
                            Menu.MostrarMensaje("Lista vacia.");
                        }
                        break;
                    case 2:
                        resultadoOperacion = CrearJugador(listaJugadores);
                        if (resultadoOperacion)
                        {
                            Menu.MostrarMensaje("Jugador creado con exito.");
                        }
                        break;
                }
                opcion = Menu.MostrarMenu();
            }
            GuardarEnXML(listaJugadores);
            Menu.MostrarMensaje("\tHasta la proxima.");
        }

        public static bool CrearJugador(List<Jugador> listaJugadores)
        {
            string nombre = Menu.LeerString("Ingrese el nombre del jugador: ") ;
            return Controlador.CrearJugador(nombre, listaJugadores);
        }

        public static void CargarXML( List<Jugador> listaJugadores)
        {
            string resultado = Controlador.LeerXML("playerList.xml", listaJugadores);
            if (resultado == "ok")
            {
                Menu.MostrarMensaje("Archivo cargado.");
            }
            else
            {
                Menu.MostrarMensaje("Error al cargar archivo.");
            }
        }

        public static void GuardarEnXML(List<Jugador> listaJugadores)
        {
            string resultado = Controlador.GuardarEnXML("playerList.xml", listaJugadores);
            if (resultado == "ok")
            {
                Menu.MostrarMensaje("\tArchivo guardado con exito.");
            }
            else
            {
                Menu.MostrarMensaje("\tNo se pudo guardar. Error: " + resultado);
            }
        }

        public static string MostrarListado(List<Jugador> listaJugadores)
        {
            string lista = "";
            foreach (Jugador jugador in listaJugadores)
            {
                lista += jugador.DarDatos();
            }
            return lista;
        }
    }
}