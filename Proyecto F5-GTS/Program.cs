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
            int opcion;
            string dni, nombre, apellido;
            bool resultadoOperacion = false;
            opcion = Menu.mostrarMenu();
            while (opcion != 9)
            {
                switch (opcion)
                {
                    case 1:
                        cargarDesdeArchivoXML(listaJugadores);
                        break;
                    case 2:
                        if (mostrarListado(listaJugadores) != "")
                        {
                            Console.WriteLine(mostrarListado(listaJugadores));
                            Menu.mostrarMensaje("Lista de empleados desplegada.");
                        }
                        else
                        {
                            Menu.mostrarMensaje("Lista vacia.");
                        }
                        break;
                }
                opcion = Menu.mostrarMenu();
            }
            Menu.mostrarMensaje("\tHasta la proxima.");
        }
    }

    public static void cargarDesdeArchivoXML(List<CJugador> listaJugadores)
    {
        string resultado = Controlador.leerXML(Menu.leerString("\tIngrese el nombre del archivo:"), listaJugadores);
        if (resultado == "ok")
        {
            Menu.mostrarMensaje("\tArchivo leido con exito.");
        }
        else
        {
            Menu.mostrarMensaje($"\tFallo lectura del archivo. Error: {resultado} ");
        }
    }

    public static void guardarEnXML(List<CJugador> listaJugadores)
    {
        string resultado = Controlador.guardarEnXML(Menu.leerString("\tIngrese el nombre del archivo a guardar"), listaJugadores);
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