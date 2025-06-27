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
            List<Grupo> listaGrupos = new List<Grupo>();
            CargarDatosXML(listaJugadores, listaGrupos);
            Dictionary<int, Jugador> dicJugadores = listaJugadores.ToDictionary(j => j.ID);
            int opcion;
            bool resultado = false;
            opcion = Menu.MostrarMenu();
            while (opcion != 0)
            {
                switch (opcion)
                {
                    //MUESTRA LISTA JUGADORES
                    case 1:
                        if (MostrarListado(listaJugadores) != "")
                        {
                            Console.WriteLine(MostrarListado(listaJugadores));
                            Menu.MostrarMensaje("\tLista desplegada.");
                        }
                        else
                        {
                            Menu.MostrarMensaje("\tLista vacia.");
                        }
                        break;
                    //Crea Jugador
                    case 2:
                        resultado = CrearJugador(listaJugadores);
                        if (resultado)
                        {
                            Menu.MostrarMensaje("\tJugador creado con exito.");
                            ActualizarDiccionario(dicJugadores, listaJugadores);
                        }
                        else
                            Menu.MostrarMensaje("\tError al crear jugador.");
                        break;
                    //Muestra Lista Grupos
                    case 5:
                        if (MostrarListaGrupos(listaGrupos, dicJugadores) != "")
                        {
                            Console.WriteLine(MostrarListaGrupos(listaGrupos, dicJugadores));
                            Menu.MostrarMensaje("\tLista desplegada.");
                        }
                        else
                            Menu.MostrarMensaje("\tLista vacia.");
                        break;
                    
                    //Crea Grupo
                    case 6:
                        resultado = CrearGrupo(listaGrupos);
                        if (resultado)
                            Menu.MostrarMensaje("\tGrupo creado con exito.");
                        else
                            Menu.MostrarMensaje("\tError al crear grupo.");
                        break;
                    //Agrega jugador a grupo
                    case 7:
                        resultado = AgregarJugador(listaJugadores,listaGrupos,dicJugadores);
                        if (resultado)
                            Menu.MostrarMensaje("\tJugadores agregados con exito.");
                        else
                            Menu.MostrarMensaje("\tError al agregar jugadores.");
                        break;
                }
                opcion = Menu.MostrarMenu();
            }
            GuardarDatosXML(listaJugadores, listaGrupos);
            Menu.MostrarMensaje("\tHasta la proxima.");
        }
        public static bool CrearJugador(List<Jugador> listaJugadores)
        {
            string nombre = Menu.LeerString("\tIngrese el nombre del jugador: ");
            return Controlador.CrearJugador(nombre, listaJugadores);
        }
        public static bool CrearGrupo(List<Grupo> listaGrupos)
        {
            string nombre = Menu.LeerString("\tIngrese el nombre del grupo: ");
            return Controlador.CrearGrupo(nombre, listaGrupos);
        }

        public static bool AgregarJugador(List<Jugador> listaJugadores, List<Grupo> listaGrupos, Dictionary<int, Jugador> idDiccionario)
        {
            Console.WriteLine("\n\tSe mostrara la lista de grupos:\n");
            Console.WriteLine(MostrarListaGrupos(listaGrupos, idDiccionario));
            int grupoID = Menu.ValInt("\n\tIngrese el ID del grupo a seleccionar\t");
            Console.WriteLine("\n\t");
            Console.WriteLine("\n\tSe mostrara la lista de jugadores.\n");
            Console.WriteLine(MostrarListado(listaJugadores));
            Console.WriteLine("\n\tIngrese el ID del jugador que desea agregar.\n\tSi desea agregar mas de un jugador ingrese los ID separados con ',' \n\tID's: ");
            string IDS = Console.ReadLine();
            List<int> listaId = Menu.ValIDS(IDS, idDiccionario);

            return Controlador.AgregarJugadorAGrupo(listaId, grupoID, listaGrupos);
        }

        public static void CargarDatosXML(List<Jugador> listaJugadores, List<Grupo> listaGrupos)
        {
            string resultadoJugadores = Controlador.LeerJugadorXML("playerList.xml", listaJugadores);
            string resultadoGrupos = Controlador.LeerGrupoXML("groupList.xml", listaGrupos);

            if (resultadoJugadores == "ok" && resultadoGrupos == "ok")
            {
                Menu.MostrarMensaje("\tArchivos cargados correctamente.");
            }
            else
            {
                string mensajeError = "\tOcurrieron errores al cargar los archivos:\n";
                if (resultadoJugadores != "ok")
                    mensajeError += $"\t- Error al cargar jugadores: {resultadoJugadores}\n";
                if (resultadoGrupos != "ok")
                    mensajeError += $"\t- Error al cargar grupos: {resultadoGrupos}\n";

                Menu.MostrarMensaje(mensajeError);
            }
        }
        public static void GuardarDatosXML(List<Jugador> listaJugadores, List<Grupo> listaGrupos)
        {
            string resultadoJugadores = Controlador.GuardarJugadorXML("playerList.xml", listaJugadores);
            string resultadoGrupos = Controlador.GuardarGrupoXML("groupList.xml", listaGrupos);

            if (resultadoJugadores == "ok" && resultadoGrupos == "ok")
            {
                Menu.MostrarMensaje("\tArchivos guardados con éxito.");
            }
            else
            {
                string mensajeError = "\tNo se pudieron guardar todos los archivos.\n";
                if (resultadoJugadores != "ok")
                    mensajeError += $"\tError al guardar jugadores: {resultadoJugadores}\n";
                if (resultadoGrupos != "ok")
                    mensajeError += $"\tError al guardar grupos: {resultadoGrupos}\n";

                Menu.MostrarMensaje(mensajeError);
            }
        }
        public static string MostrarListaGrupos(List<Grupo> listaGrupos, Dictionary<int, Jugador> dicJugadores)
        {
            string lista = "";
            foreach (Grupo grupo in listaGrupos)
            {
                lista += grupo.DarDatos(dicJugadores);

            }
            return lista;
        }
        public static void ActualizarDiccionario(Dictionary<int, Jugador> dic, List<Jugador> lista)
        {
            dic.Clear();
            foreach (var j in lista)
                dic[j.ID] = j;
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

