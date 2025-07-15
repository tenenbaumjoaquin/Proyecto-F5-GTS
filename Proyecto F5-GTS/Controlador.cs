using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDataReader;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;

namespace Proyecto_F5_GTS
{
    //Controlador STATIC para invocar las funciones sin instanciar
    static class Controlador
    {
        //FUNCIONES JUGADOR
        //Metodo que retorna el ultimo id en la lista de jugadores
        public static int UltimoIDJugador(List<Jugador> listaJugadores)
        {
            if (listaJugadores != null && listaJugadores.Count > 0)
                return listaJugadores[listaJugadores.Count - 1].ID;
            else
                return 0;
        }
        public static Jugador Buscar(List<Jugador> listaJugadores, string nombre)
        {
            int indice = listaJugadores.IndexOf(new Jugador(nombre));
            if (indice == -1)
            {
                return null;
            }
            else
            {
                return listaJugadores[indice];
            }
        }
        //BUSCAR POR ID Y RETORNA EL JUGADOR
        public static Jugador BuscarId(List<Jugador> listaJugadores, int ID)
        {
            foreach (Jugador jugador in listaJugadores)
            {
                if (jugador.ID == ID)
                    return jugador;
            }
            return null;
        }
        public static bool CrearJugador(string nombre, List<Jugador> listaJugadores)
        {
            if (Buscar(listaJugadores, nombre) == null)
            {
                int id = UltimoIDJugador(listaJugadores) + 1;
                Jugador newJugador = new Jugador(id, nombre);
                listaJugadores.Add(newJugador);
                newJugador.DarMenosDatos();
                return true;
            }
            Menu.MostrarMensaje("\n\tJugador ya registrado con ese nombre.");
            return false;
        }
        public static bool CambiarNombreJugador(List<Jugador> listaJugadores, int ID, string nuevoNombre)
        {
            Jugador jugador = listaJugadores.FirstOrDefault(j => j.ID == ID);
            if (jugador != null)
            {
                jugador.NOMBRE = nuevoNombre;
                return true;
            }
            return false;
        }
        public static bool CambiarSTATS(List<Jugador> listaJugadores, int ID, int indiceStat)
        {
            Jugador jugador = listaJugadores.FirstOrDefault(j => j.ID == ID);

            if (jugador == null || indiceStat < 0 || indiceStat >= jugador.STATS.Length)
                return false;

            Console.Write($"\tIngrese nuevo valor para {jugador.STATS[indiceStat]._nombre} (0-10): ");
            if (int.TryParse(Console.ReadLine(), out int nuevoValor) && nuevoValor >= 0 && nuevoValor <= 10)
            {
                // Esta llamada ya se encarga de actualizar la stat y recalcular todo
                return jugador.CambiarStat(indiceStat, nuevoValor);
            }
            else
            {
                Console.WriteLine("\tValor inválido. No se realizó ningún cambio.");
                return false;
            }
        }
        public static bool ModificarPosicion(List<Jugador> listaJugadores, int ID)
        {
            Jugador jugador = listaJugadores.FirstOrDefault(j =>j.ID == ID);
            if (jugador == null)
                return false;
            else
            {
                jugador.POSICION = jugador.CargarPosicion();
                jugador.CalcularCalificacion();
                return true;
            }
        }
        //FUNCIONES DE GRUPO
        //Retorna el ultimo id en la lista de grupos
        public static int UltimoID(List<Grupo> listaGrupos)
        {
            if (listaGrupos != null && listaGrupos.Count > 0)
                return listaGrupos[listaGrupos.Count - 1].ID;
            else
                return 0;
        }
        public static Grupo Buscar(List<Grupo> listaGrupo, string nombre)
        {
            int indice = listaGrupo.IndexOf(new Grupo(nombre));
            if (indice == -1)
                return null;
            else
                return listaGrupo[indice];
        }
        public static Grupo BuscarId(List<Grupo> listaGrupo, int ID)
        {
            foreach (Grupo grupo in listaGrupo)
            {
                if (grupo.ID == ID)
                    return grupo;
            }
            return null;
        }
        public static bool EliminarJugador( List<Jugador> listaJugadores, List<Grupo> listaGrupos, int ID)
        {
            Jugador jugadorAEliminar = BuscarId(listaJugadores, ID);
            if( jugadorAEliminar != null)
            {
                foreach (Grupo grupo in listaGrupos)
                {
                    if (grupo.JUGADORES.Contains(ID))
                    {
                        grupo.JUGADORES.Remove(ID);
                        grupo.COUNT--;
                    }
                }
                listaJugadores.Remove(jugadorAEliminar);
                return true;
            }
            Menu.MostrarMensaje("\n\tJugador no encontrado.");
            return false;
        }
        public static bool CrearGrupo ( string nombre, List<Grupo> listaGrupos )
        {
            if( Buscar(listaGrupos, nombre) == null)
            {
                int id = UltimoID(listaGrupos)+1;
                Grupo grupo = new Grupo(id, nombre);
                listaGrupos.Add(grupo);
                return true;
            }
            Menu.MostrarMensaje("\n\tGrupo ya registrado con ese nombre.");
            return false;
        }
        public static bool EliminarGrupo (List<Grupo> listaGrupos, int ID)
        {
            Grupo grupoAEliminar = BuscarId(listaGrupos, ID);
            if( grupoAEliminar != null)
            {
                listaGrupos.Remove(grupoAEliminar);
                return true;
            }
            Menu.MostrarMensaje("\n\tJugador no encontrado.");
            return false ;
        }
        public static bool AgregarJugadorAGrupo( List<int> idsJugadores, int idGrupo, List<Grupo> listaGrupos)
        {
            Grupo grupoSeleccionado = BuscarId(listaGrupos, idGrupo);
            if ( grupoSeleccionado != null)
            {
                foreach(int id in idsJugadores)
                {
                    grupoSeleccionado.AgregarJugador(id);
                }
                return true;
            }
            else
            {
                Console.WriteLine("\nGrupo no encontrado.");
                return false;
            }
        }
        public static bool EliminarJugadorDeGrupo( List<int> idsJugadores, int idGrupo, List<Grupo> listaGrupos)
        {
            Grupo grupoSeleccionado = BuscarId(listaGrupos, idGrupo) ;
            if (grupoSeleccionado != null)
            {
                foreach(int id in idsJugadores)
                    grupoSeleccionado.EliminarJugador(id) ;
                return true;
            }
            else
            {
                Console.WriteLine("\nGrupo no encontrado.");
                return false;
            }
        }   
        public static bool CambiarNombreGrupo(List<Grupo> listaGrupo, int ID, string nuevoNombre)
        {
            Grupo grupo = listaGrupo.FirstOrDefault(g => g.ID == ID);
            if (grupo != null)
            {
                grupo.NOMBRE = nuevoNombre;
                return true;
            }
            return false;
        }
        //FUNCIONES DE LECTURA Y GUARDADO
        public static bool CargarJugador(string nombre, string posicion, string calificacion, double puntotal, (string nombre, int puntuacion)[] stats, List<Jugador> listaJugadores)
        {
            if (Buscar(listaJugadores, nombre) == null)
            {
                int id = UltimoIDJugador(listaJugadores);
                Jugador newJugador = new Jugador(id, nombre, posicion, calificacion, puntotal, stats);
                listaJugadores.Add(newJugador);
                return true;
            }
            Menu.MostrarMensaje("\n\tJugador ya registrado con ese nombre.");
            return false;
        }
        
        public static string GuardarGrupoXML( string archivo, List<Grupo> grupos)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(xmlDeclaration);
                XmlElement rootElement = xmlDoc.CreateElement("grupos");
                xmlDoc.AppendChild(rootElement);
                foreach (Grupo grupo in grupos)
                {
                    XmlElement grupoElement = xmlDoc.CreateElement("grupo");

                    XmlElement idElement = xmlDoc.CreateElement("id");
                    idElement.InnerText = grupo.ID.ToString();
                    grupoElement.AppendChild(idElement);

                    XmlElement nombreElement = xmlDoc.CreateElement("nombre");
                    nombreElement.InnerText = grupo.NOMBRE;
                    grupoElement.AppendChild(nombreElement);

                    /*
                    XmlElement direccionElement = xmlDoc.CreateElement("direccion");
                    direccionElement.InnerText = grupo.DIRECCION;
                    grupoElement.AppendChild(direccionElement);

                    XmlElement horarioElement = xmlDoc.CreateElement("horario");
                    horarioElement.InnerText = grupo.HORARIO;
                    grupoElement.AppendChild(horarioElement);
                    */

                    XmlElement countElement = xmlDoc.CreateElement("count");
                    countElement.InnerText = grupo.COUNT.ToString();
                    grupoElement.AppendChild(countElement);

                    // Sección para guardar la lista de IDs de jugadores
                    XmlElement jugadoresElement = xmlDoc.CreateElement("jugadores");
                    foreach (int jugadorId in grupo.JUGADORES)
                    {
                        XmlElement idJugadorElement = xmlDoc.CreateElement("id");
                        idJugadorElement.InnerText = jugadorId.ToString();
                        jugadoresElement.AppendChild(idJugadorElement);
                    }
                    grupoElement.AppendChild(jugadoresElement);

                    rootElement.AppendChild(grupoElement);
                }
                xmlDoc.Save(archivo);
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string GuardarJugadorXML(string archivo, List<Jugador> jugadores)
        {
            //Utiliza la funcion tryCatch a fin de capturar cualquier error en tiempo de ejecucion
            //Y sino informar el error
            try
            {

                XmlDocument xmlDoc = new XmlDocument();//Crea un nuevo archvio XML
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);//Declara el formato del archivo
                xmlDoc.AppendChild(xmlDeclaration); //agrega la declaracion al archivo

                XmlElement rootElement = xmlDoc.CreateElement("jugadores");//Asigna el campo clave del archivo (Clase padre) y lo establece como la raiz de la lista
                xmlDoc.AppendChild(rootElement); 

                foreach (Jugador jugador in jugadores)//Recorre la lista añadiendo cada objeto hijo en el formato dado
                {
                    //Crea el objeto generalizado o padre
                    XmlElement jugadorElement = xmlDoc.CreateElement("jugador"); //Crea y asigna el primer elemento y campo

                    //Crea, lee, asigna y agrega el nuevo campo del elemento hijo
                    XmlElement idElement = xmlDoc.CreateElement("id"); //Crea el elemento
                    idElement.InnerText = jugador.ID.ToString(); //Asigna el valor
                    jugadorElement.AppendChild(idElement); //Agrega elemento

                    XmlElement nombreElement = xmlDoc.CreateElement("nombre"); //crea el elemento
                    nombreElement.InnerText = jugador.NOMBRE; //Asigna el valor, lo convierte en string
                    jugadorElement.AppendChild(nombreElement); //Agrega el elemento

                    XmlElement posicionElement = xmlDoc.CreateElement("posicion"); //crea el elemento
                    posicionElement.InnerText = jugador.POSICION; //Asigna el valor, lo convierte en string
                    jugadorElement.AppendChild(posicionElement); //Agrega el elemento

                    XmlElement calificacionElement = xmlDoc.CreateElement("calificacion");
                    calificacionElement.InnerText = jugador.CALIFICACION;
                    jugadorElement.AppendChild(calificacionElement);

                    XmlElement puntuacionElement = xmlDoc.CreateElement("puntuacion");
                    puntuacionElement.InnerText = jugador.PUNTOTAL.ToString();
                    jugadorElement.AppendChild (puntuacionElement);

                    // Sección para guardar estadísticas
                    XmlElement statsElement = xmlDoc.CreateElement("estadisticas");
                    foreach (var stat in jugador.STATS)
                    {
                        XmlElement statElement = xmlDoc.CreateElement(stat._nombre);
                        statElement.InnerText = stat._puntuacion.ToString();
                        statsElement.AppendChild(statElement);
                    }
                    jugadorElement.AppendChild(statsElement);

                    rootElement.AppendChild(jugadorElement); //agrega el objeto al la raiz del archivo
                }

                xmlDoc.Save(archivo); //Guarda el archivo con el nombre transferido a la funcion
                return "ok"; //Retorna ok
            }
            catch (Exception ex) //Si el proceso falla en algun lugar, se captura el error, se retornar como mensaje y el programa no se interrumpe
            {
                return ex.Message;
            }
        }
        public static string LeerJugadorXML (string archivo, List<Jugador> listaJugadores)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument ();
                xmlDoc.Load (archivo);
                XmlNodeList jugadorNodes = xmlDoc.SelectNodes("//jugador");
                
                foreach (XmlNode jugadorNode in jugadorNodes)
                {
                    int id;
                    if (! int.TryParse(jugadorNode.SelectSingleNode("id").InnerText, out id))
                    {
                        id = 0;
                    }
                    string nombre = jugadorNode.SelectSingleNode("nombre").InnerText;
                    string posicion = jugadorNode.SelectSingleNode("posicion").InnerText;
                    string calificacion = jugadorNode.SelectSingleNode("calificacion").InnerText;
                    string puntuacionStr = jugadorNode.SelectSingleNode("puntuacion").InnerText;
                    puntuacionStr = puntuacionStr.Replace(",", ".");
                    double puntuacion = Convert.ToDouble(puntuacionStr, CultureInfo.InvariantCulture);
                    // Leer estadísticas
                    List<(string Nombre, int Puntuacion)> statsList = new List<(string, int)>();
                    XmlNode statsNode = jugadorNode.SelectSingleNode("estadisticas");
                    if (statsNode != null)
                    {
                        foreach (XmlNode statNode in statsNode.ChildNodes)
                        {
                            string statNombre = statNode.Name;
                            int statValor = int.TryParse(statNode.InnerText, out int tempValor) ? tempValor : 0;
                            statsList.Add((statNombre, statValor));
                        }
                    }
                    Jugador jugador = new Jugador(id, nombre, posicion, calificacion, puntuacion, statsList.ToArray());
                    listaJugadores.Add(jugador);
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return $"Error {ex.Message}";
            }
        }
        public static string LeerGrupoXML (string archivo, List<Grupo> grupos)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(archivo);
                XmlNodeList grupoNodes = xmlDoc.SelectNodes("//grupo");

                foreach (XmlNode grupoNode in grupoNodes)
                {
                    int id = int.TryParse(grupoNode.SelectSingleNode("id")?.InnerText, out int tempId) ? tempId : 0;
                    string nombre = grupoNode.SelectSingleNode("nombre")?.InnerText ?? "";
                    int count = int.TryParse(grupoNode.SelectSingleNode("count")?.InnerText, out int tempCount) ? tempCount : 0;

                    List<int> idsJugadores = new List<int>();
                    XmlNode jugadoresNode = grupoNode.SelectSingleNode("jugadores");
                    if (jugadoresNode != null)
                    {
                        foreach (XmlNode idNode in jugadoresNode.SelectNodes("id"))
                        {
                            if (int.TryParse(idNode.InnerText, out int idJugador))
                            {
                                idsJugadores.Add(idJugador);
                            }
                        }
                    }
                    Grupo grupo = new Grupo(id, nombre, idsJugadores);
                    grupo.COUNT = count; // redundante si ya se calculó en el constructor, pero por si acaso
                    grupos.Add(grupo);
                }

                return "ok";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
