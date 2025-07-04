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
            int opcion, opcionGrupo = 0, opcionJugador = 0, idGrupo = 0, idJugador = 0, opcionSTAT = 0;
            bool resultado = false;
            opcion = Menu.MostrarMenu();
            while (opcion != 0)
            {
                switch (opcion)
                {
                    //MUESTRA LISTA JUGADORES Y OPCIONES VINCULADAS
                    case 1:
                        if (MostrarListado(listaJugadores) != "")
                        {
                            Console.WriteLine(MostrarListado(listaJugadores));
                            //Menu.MostrarMensaje("\tLista desplegada.")
                            idJugador = Menu.ValID("\n\tSeleccione el ID de un jugador o 0 para volver.\n\tOpcion: ", dicJugadores);
                            if (idJugador != 0)
                            {
                                Jugador jugadorAModificar = Controlador.BuscarId(listaJugadores, idJugador);
                                Console.Clear();
                                Console.WriteLine(jugadorAModificar.DarDatos());
                                opcionJugador = Menu.MostrarMenuJugador();
                                switch (opcionJugador)
                                {
                                    //Modificar nombre
                                    case 1:
                                        resultado = ModificarNombreJugador(listaJugadores, idJugador);
                                        if (resultado)
                                        {
                                            Menu.MostrarMensaje("\tNombre actualizado.");
                                            ActualizarDiccionario(dicJugadores, listaJugadores);
                                        }
                                        else
                                            Menu.MostrarMensaje("\tError al cambiar el nombre.");
                                        break;
                                    //Modificar POSICION
                                    case 2:
                                        resultado = ModificarPosicion(listaJugadores, idJugador);
                                        if (resultado)
                                        {
                                            Menu.MostrarMensaje("\tPosicion actualizada.");
                                            ActualizarDiccionario(dicJugadores, listaJugadores);
                                        }
                                        else
                                            Menu.MostrarMensaje("\tError al cambiar la posicion.");
                                        break;
                                    //Modificar STAT
                                    case 3:
                                        resultado = ModificarSTATS(listaJugadores, idJugador);
                                        if ((resultado))
                                        {
                                            Menu.MostrarMensaje("\tSTATS actualizadas.");
                                            ActualizarDiccionario(dicJugadores, listaJugadores);
                                        }
                                        else
                                            Menu.MostrarMensaje("\tError al modificar STATS");
                                        break;
                                    //Eliminar Jugador
                                    case 4:
                                        resultado = EliminarJugador(listaJugadores, listaGrupos, idJugador);
                                        ActualizarDiccionario(dicJugadores, listaJugadores);
                                        break;
                                }
                            }
                            break;
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
                    //MUESTRA LISTA GRUPOS Y OPCIONES VINCULADAS
                    case 3:
                        if (MostrarListaGrupos(listaGrupos, dicJugadores) != "")
                        {
                            Console.WriteLine(MostrarListaGrupos(listaGrupos));
                            //Menu.MostrarMensaje("\tLista desplegada.");
                            idGrupo = Menu.ValInt("\n\tSeleccione el ID de un Grupo o 0 para volver.");
                            if (idGrupo != -1)
                            {
                                Console.Clear();
                                Grupo grupoAModificar = Controlador.BuscarId(listaGrupos, idGrupo);
                                Console.WriteLine(grupoAModificar.DarDatos(dicJugadores));
                                opcionGrupo = Menu.MostrarMenuGrupo();
                                switch (opcionGrupo)
                                {
                                    //Cambiar nombre
                                    case 1:
                                        resultado = ModificarNombreGrupo(listaGrupos, idGrupo);
                                        if (resultado)
                                            Menu.MostrarMensaje("\tNombre actualizado.");
                                        else
                                            Menu.MostrarMensaje("\tError al cambiar el nombre.");
                                        break;
                                    //Agregar jugador
                                    case 2:
                                        resultado = AgregarJugadorAGrupo(listaJugadores, listaGrupos, dicJugadores, idGrupo);
                                        if (resultado)
                                            Menu.MostrarMensaje("\tJugadores agregados correctamente.");
                                        else
                                            Menu.MostrarMensaje("\tError al agregar jugadores.");
                                        break;
                                    //Eliminar jugador de grupo
                                    case 3:
                                        resultado = EliminarJugadorDeGrupo(listaJugadores, listaGrupos, dicJugadores, idGrupo);
                                        if (resultado)
                                            Menu.MostrarMensaje("\tJugadores eliminados correctamente.");
                                        else
                                            Menu.MostrarMensaje("\tError al eliminar jugadores.");
                                        break;
                                    //Eliminar grupo
                                    case 4:
                                        resultado = EliminarGrupo(listaGrupos, idGrupo);
                                        if (resultado)
                                            Menu.MostrarMensaje("\tGrupo eliminado con exito.");
                                        else
                                            Menu.MostrarMensaje("\tError al eliminar grupo.");
                                        break;
                                }
                            }
                        }
                        else
                            Menu.MostrarMensaje("\tLista vacia.");
                        break;
                    //Crea Grupo
                    case 4:
                        resultado = CrearGrupo(listaGrupos);
                        if (resultado)
                            Menu.MostrarMensaje("\tGrupo creado con exito.");
                        else
                            Menu.MostrarMensaje("\tError al crear grupo.");
                        break;
                }
                opcion = Menu.MostrarMenu();
            }
            GuardarDatosXML(listaJugadores, listaGrupos);
            Menu.MostrarMensaje("\tHasta la proxima.");
        }
        //FUNCIONES JUGADOR
        public static bool CrearJugador(List<Jugador> listaJugadores)
        {
            string nombre = Menu.LeerString("\tIngrese el nombre del jugador: ");
            return Controlador.CrearJugador(nombre, listaJugadores);
        }
        public static bool EliminarJugador(List<Jugador> listaJugadores, List<Grupo> listaGrupos, int idJugador)
        {
            return Controlador.EliminarJugador(listaJugadores, listaGrupos, idJugador);
        }
        public static bool ModificarNombreJugador(List<Jugador> listaJugadores, int ID)
        {
            string nuevoNombre = Menu.LeerString("\n\tIngrese el nuevo nombre del jugador:");
            return Controlador.CambiarNombreJugador(listaJugadores, ID, nuevoNombre);
        }
        public static bool ModificarPosicion(List<Jugador> listaJugadores, int ID)
        {
            return Controlador.ModificarPosicion(listaJugadores,ID);
        }
        public static bool ModificarSTATS(List<Jugador> listaJugadores, int ID)
        {
            int opcion = Menu.MostrarMenuSTATS();

            if (opcion >= 1 && opcion <= 10)
            {
                return Controlador.CambiarSTATS(listaJugadores, ID, opcion - 1); // Convertimos a índice 0–9
            }
            else if (opcion == 0)
            {
                Console.WriteLine("\tVolviendo al menú anterior...");
                return false;
            }
            else
            {
                Console.WriteLine("\tOpción inválida. Intente nuevamente.");
                return false;
            }
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
                lista += jugador.DarMenosDatos();
            }
            return lista;
        }
        //FUNCIONES GRUPO
        public static bool CrearGrupo(List<Grupo> listaGrupos)
        {
            string nombre = Menu.LeerString("\tIngrese el nombre del grupo: ");
            return Controlador.CrearGrupo(nombre, listaGrupos);
        }
        public static bool EliminarGrupo(List<Grupo> listaGrupos, int ID)
        {
            return Controlador.EliminarGrupo(listaGrupos, ID);
        }
        public static bool ModificarNombreGrupo(List<Grupo> listaGrupos, int ID)
        {
            string nuevoNombre = Menu.LeerString("\n\tIngrese el nuevo nombre del grupo:");
            return Controlador.CambiarNombreGrupo(listaGrupos, ID, nuevoNombre);
        }
        public static bool AgregarJugadorAGrupo(List<Jugador> listaJugadores, List<Grupo> listaGrupos, Dictionary<int, Jugador> idDiccionario, int grupoID)
        {
            Console.WriteLine("\n\tSe mostrara la lista de jugadores.\n");
            Console.WriteLine(MostrarListado(listaJugadores));
            Console.Write("\n\tIngrese el ID del jugador que desea agregar.\n\tSi desea agregar mas de un jugador ingrese los ID separados con ',' \n\tID's: ");
            string IDS = Console.ReadLine();
            List<int> listaId = Menu.ValIDS(IDS, idDiccionario);
            return Controlador.AgregarJugadorAGrupo(listaId, grupoID, listaGrupos);
        }
        public static bool EliminarJugadorDeGrupo(List<Jugador> listaJugadores, List<Grupo> listaGrupos, Dictionary<int, Jugador> idDiccionario, int grupoID)
        {
            Console.WriteLine("\n\tSe mostrara la lista de jugadores.\n");
            Console.WriteLine(MostrarListaGrupos(listaGrupos, idDiccionario));
            Console.Write("\n\tIngrese el ID del jugador que desea eliminar del grupo.\n\tSi desea eliminar mas de un jugador ingrese los ID separados con ',' \n\tID's: ");
            string IDS = Console.ReadLine();
            List<int> listaId = Menu.ValIDS(IDS, idDiccionario);
            return Controlador.EliminarJugadorDeGrupo(listaId, grupoID, listaGrupos);
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
        public static string MostrarListaGrupos(List<Grupo> listaGrupos)
        {
            string lista = "";
            foreach (Grupo grupo in listaGrupos)
            {
                lista += grupo.DarMenosDatos();

            }
            return lista;
        }
        //FUNCIONES DE GUARDADO Y CARGA DE ARCHIVOS
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
    }
}