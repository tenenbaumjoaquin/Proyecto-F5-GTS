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
            Dictionary<int, Jugador> dicJugadores = listaJugadores.ToDictionary(j => j.ID);
            List<Grupo> listaGrupos = new List<Grupo>();
            CargarJugadoresXML(listaJugadores);
            CargarGruposXML(listaGrupos);
            int opcion;
            bool resultadoOperacion = false;
            opcion = Menu.MostrarMenu();
            while (opcion != 0)
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
            GuardarJugadoresXML(listaJugadores);
            GuardarGruposXML(listaGrupos);
            Menu.MostrarMensaje("\tHasta la proxima.");
        }

        public static bool CrearJugador(List<Jugador> listaJugadores)
        {
            string nombre = Menu.LeerString("Ingrese el nombre del jugador: ") ;
            return Controlador.CrearJugador(nombre, listaJugadores);
        }
        public static bool CrearGrupo(List<Grupo> listaGrupos)
        {
            string nombre = Menu.LeerString("Ingrese el nombre del grupo: ");
            return Controlador.CrearGrupo(nombre, listaGrupos);
        }

        public static bool AgregarJugador(List<Jugador> listaJugadores, List<Grupo> listaGrupos)
        {
            Console.WriteLine("\n\tSe ")
        }

        public static void CargarJugadoresXML( List<Jugador> listaJugadores)
        {
            string resultado = Controlador.LeerJugadorXML("playerList.xml", listaJugadores);
            if (resultado == "ok")
            {
                Menu.MostrarMensaje("Archivo cargado.");
            }
            else
            {
                Menu.MostrarMensaje("Error al cargar jugadores");
            }
        }

        public static void GuardarJugadoresXML(List<Jugador> listaJugadores)
        {
            string resultado = Controlador.GuardarJugadorXML("playerList.xml", listaJugadores);
            if (resultado == "ok")
            {
                Menu.MostrarMensaje("\tArchivo guardado con exito.");
            }
            else
            {
                Menu.MostrarMensaje("\tNo se pudo guardar. Error: " + resultado);
            }
        }
        public static void CargarGruposXML(List<Grupo> listaGrupos)
        {
            string resultado = Controlador.GuardarGrupoXML("groupList.xml", listaGrupos);
            if (resultado == "ok")
                Menu.MostrarMensaje("\tArchivo cargado.");
            else
                Menu.MostrarMensaje("\tError al cargar grupos.");
        }
        public static void GuardarGruposXML(List<Grupo> listaGrupos)
        {
            string resultado = Controlador.GuardarGrupoXML("groupList.xml", listaGrupos);
            if (resultado == "ok")
                Menu.MostrarMensaje("\tArchivo guardado con exito.");
            else
                Menu.MostrarMensaje("\tNo se pudo guardar. Error: " + resultado);
        }
        public static string MostrarListado(List<Grupo> listaGrupos, Dictionary<int, Jugador> dicJugadores)
        {
            string lista = "";
            foreach (Grupo grupo in listaGrupos)
            {
                lista += grupo.DarDatos(dicJugadores);

            }
            return lista;
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