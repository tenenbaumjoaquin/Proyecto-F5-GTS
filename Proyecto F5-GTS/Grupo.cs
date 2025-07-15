using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_F5_GTS
{
    internal class Grupo
    {
        private int _id;
        private string _nombre;
        //private string _direccion;
        //private string _horario;
        private int _count;
        private List<int> _jugadores;

        public int ID { get =>  _id; set => _id = value; }
        public string NOMBRE { get => _nombre ; set => _nombre = value; }
        //public string DIRECCION { get => _direccion ; set => _direccion = value; }
       // public string HORARIO { get => _horario ; set => _horario = value; }
        public int COUNT { get => _count; set => _count = value; }
        public List<int> JUGADORES => _jugadores;

        public Grupo()
        {
            this.ID = 0;
            this.NOMBRE = "";
            //this.DIRECCION = "";
            //this.HORARIO = "";
            this.COUNT = 0;
            this._jugadores = new List<int>();
        }
        public Grupo (string nombre)
        {
            this.ID = -1;
            this.NOMBRE = nombre;
            //this.DIRECCION = "";
            //this.HORARIO = "";
            this.COUNT = 0;
            this._jugadores = new List<int>();
        }
        //Constructor parametrizado para crear el grupo
        public Grupo ( int id, string nombre)
        {
            this.ID = id;
            this.NOMBRE = nombre;
            //this.DIRECCION= direccion;
            //this.HORARIO = horario;
            this.COUNT = 0;
            this._jugadores = new List<int>();
        }
        //Constructor totalmente parametrizado para instanciar objeto cargando de archivo
        public Grupo ( int id, string nombre, List<int> ids)
        {
            this.ID = id;
            this.NOMBRE = nombre;
            //this.DIRECCION = direccion;
            //this.HORARIO = horario;
            this.COUNT = ids.Count;
            this._jugadores = new List<int>(ids);

        }
        public bool AgregarJugador(int jugadorId)
        {
            if (!this.JUGADORES.Contains(jugadorId))
            {
                this.JUGADORES.Add(jugadorId);
                COUNT++;
                return true;
            }
            else 
            {
                Console.WriteLine("\n\tJugador ya en grupo.");
                return false;
            }
        }
        public bool EliminarJugador(int jugadorId)
        {
            if (this.JUGADORES.Contains(jugadorId))
            {
                this.JUGADORES.Remove(jugadorId);
                COUNT--;
                return true;
            }
            else
            {
                Console.WriteLine("\n\tERROR: el jugador no se haya en el grupo.");
                return false;
            }
        }
        public string DarDatos(Dictionary<int, Jugador> dicJugadores)
        {
            string datos = $"\n\n\tID: {ID}\n\tNombre del grupo: {NOMBRE}\n";
            datos += $"\tCantidad de jugadores: {COUNT}\n";
            
            if (_jugadores.Count == 0)
                datos += "\tJugadores: Grupo vacio.\n";
            else
            {
                foreach (int id in _jugadores)
                {
                    if (dicJugadores.TryGetValue(id, out Jugador jugador)) // Búsqueda en O(1)
                    {
                        datos += jugador.FichaJugador();
                    }
                }
            } 
            return datos;
        }
        public string FichaGrupo()
        {
            int anchoNombre = NOMBRE.Length;
            int ancho = Math.Max(anchoNombre + 8, 30);
            string borde = new string('*', ancho);
            string nombreCentrado = $"*{NOMBRE.Center(ancho - 2)}*";
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"\n\t{borde}");
            sb.AppendLine($"\t{nombreCentrado}");
            sb.AppendLine($"\t{borde}");
            sb.AppendLine($"\t\t[ID]: {ID}");
            sb.AppendLine($"\t\t[N° Jugadores]: {COUNT}");
            return sb.ToString();
        }
    }
}