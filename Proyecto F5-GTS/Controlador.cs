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
        //Metodo que retorna el ultimo id en la lista de jugadores
        public static int UltimoID(List<Jugador> listaJugadores)
        {
            if (listaJugadores != null && listaJugadores.Count > 0)
            {
                return listaJugadores[listaJugadores.Count - 1].ID;
            }
            else
            {
                return 0;
            }
        }
        //Metodo de busqueda en lista, para corroborar que el elemento a insertar no se encuentra en la lista
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
        public static bool CrearJugador( string nombre, List<Jugador> listaJugadores )
        {
            if ( Buscar(listaJugadores, nombre) == null)
            {
                int id = UltimoID(listaJugadores)+1;
                Jugador newJugador = new Jugador(id, nombre);
                listaJugadores.Add(newJugador);
                return true;
            }
            Menu.MostrarMensaje("\nJugador ya registrado con ese nombre.");
            return false;
        }

        public static bool CargarJugador(string nombre, string posicion, string calificacion, double puntotal, (string nombre, int puntuacion)[] stats, List<Jugador> listaJugadores)
        {
            if (Buscar(listaJugadores, nombre) == null)
            {
                int id = UltimoID(listaJugadores);
                Jugador newJugador = new Jugador(id, nombre, posicion, calificacion, puntotal, stats);
                listaJugadores.Add(newJugador);
                return true;
            }
            Menu.MostrarMensaje("\nJugador ya registrado con ese nombre.");
            return false;
        }

        public static string GuardarEnXML(string archivo, List<Jugador> listaJugadores)
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

                foreach (Jugador jugador in listaJugadores)//Recorre la lista añadiendo cada objeto hijo en el formato dado
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
                    XmlElement statsElement = xmlDoc.CreateElement("Estadisticas");
                    foreach (var stat in jugador.STATS)
                    {
                        XmlElement statElement = xmlDoc.CreateElement(stat.Nombre);
                        statElement.InnerText = stat.Puntuacion.ToString();
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

        public static string LeerXML (string archivo, List<Jugador> listaJugadores)
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
                    XmlNode statsNode = jugadorNode.SelectSingleNode("Estadisticas");
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
    }
}
