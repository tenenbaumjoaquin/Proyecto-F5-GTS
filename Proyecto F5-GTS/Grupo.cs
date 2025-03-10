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
        private string _direccion;
        private string _horario;
        private int _count;
        private List<int> _jugadores;

        public int ID { get =>  _id; set => _id = value; }
        public string NOMBRE { get => _nombre ; set => _nombre = value; }
        public string DIRECCION { get => _direccion ; set => _direccion = value; }
        public string HORARIO { get => _horario ; set => _horario = value; }
        public int COUNT { get => _count; set => _count = value; }
        public List<int> JUGADORES => _jugadores;

        public Grupo()
        {
            this.ID = 0;
            this.NOMBRE = "";
            this.DIRECCION = "";
            this.HORARIO = "";
            this.COUNT = 0;
            this._jugadores = new List<int>();
        }

        public Grupo ( int id, string nombre, string direccion, string horario)
        {
            this.ID = id;
            this.NOMBRE = nombre;
            this.DIRECCION= direccion;
            this.HORARIO = horario;
            this.COUNT = 0;
            this._jugadores = new List<int>();
        }
        public void AgregarJugador(int jugadorId)
        {
            if (!this.JUGADORES.Contains(jugadorId))
            {
                this.JUGADORES.Add(jugadorId);
                COUNT++;
            }
        }
        public void EliminarJugador(int jugadorId)
        {
            if (this.JUGADORES.Contains(jugadorId))
            {
                this.JUGADORES.Remove(jugadorId);
                COUNT--;
            }
        }

    }
}
