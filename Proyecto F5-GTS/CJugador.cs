using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDataReader;
using System.Threading.Tasks;

namespace Proyecto_F5_GTS
{
    internal class Jugador
    {
        private int _id;
        private string _nombre;
        private string _posicion;
        private string _calificacion;
        private double _punTotal;
        private (string _nombre, int _puntuacion)[] _stats =
        {
            ("VEL" , 0), ("AGT" , 0), ("PAS" , 0), ("GMB" , 0), ("DEF" , 0),
            ("FIS" , 0), ("PEG" , 0), ("TIR" , 0), ("ATJ" , 0), ("REF" , 0)
        };
        //Propiedades R-W
        public int ID { get => _id; set => _id = value; }
        public string NOMBRE { get => _nombre; set => _nombre = value; }
        public string POSICION { get => _posicion; set => _posicion = value; }
        public string CALIFICACION { get => _calificacion; set => _calificacion = value; }
        public double PUNTOTAL { get => _punTotal; set => _punTotal = value; }
        public (string _nombre, int _puntuacion)[] STATS => _stats;

        private static readonly Dictionary<string, string> NombresCompletosStats = new Dictionary<string, string>
    {
        { "VEL", "VELOCIDAD" },
        { "AGT", "AGUANTE" },
        { "PAS", "PASE" },
        { "GMB", "GAMBETA" },
        { "DEF", "DEFENSA" },
        { "FIS", "FISICO" },
        { "PEG", "PEGADA" },
        { "TIR", "TIRO" },
        { "ATJ", "ATAJADA" },
        { "REF", "REFLEJO" }
    };
        //Constructor sin parametrizar
        public Jugador()
        {
            ID = 0;
            NOMBRE = "";
            POSICION = "";
            CALIFICACION = "";
            _stats = new (string Nombre, int Puntacion )[] 
            { 
                ("VEL", 0), ("AGT", 0), ("PAS", 0), ("GMB", 0), ("DEF", 0),
                ("FIS", 0), ("PEG", 0), ("TIR", 0), ("ATJ", 0), ("REF", 0) 
            };
        }
        //Constructor parcialmente parametrizado
        public Jugador (int id, string nombre)
        {
            this.ID = id;
            this.NOMBRE = nombre;
            this.POSICION = CargarPosicion();
            CargarStats();
        } 
        //Constructor con un parametro para busqueda en lista
        public Jugador ( string nombre)
        {
            this.NOMBRE = nombre;
        }
        //Constructor totalmente parametrizado (UTILIZADO PARA POBLAR LA LISTA CUANDO SE CARGA DESDE EL ARCHIVO)
        public Jugador (int id, string nombre, string posicion, string calificacion, double punTotal, (string Nombre, int Puntuacion) [] stats )
        {
            this.ID = id;
            this.NOMBRE = nombre;
            this.POSICION = posicion;
            this.CALIFICACION = calificacion;
            this.PUNTOTAL = punTotal;
            for (int i = 0; i < stats.Length; i++)
            {
                this.STATS[i] = stats[i];
            }
        }
        public void CargarStats()
        {
            for (int i = 0; i < STATS.Length; i++)
            {
                string nombreCompleto = NombresCompletosStats.ContainsKey(STATS[i]._nombre)
                ? NombresCompletosStats[STATS[i]._nombre]
                : STATS[i]._nombre; // Si no existe en el diccionario, usa el mismo nombre

                Console.Write($"\n\tIngrese puntuación para {nombreCompleto}: ");
                int puntuacion;
                while (!int.TryParse(Console.ReadLine(), out puntuacion) || puntuacion < 0 || puntuacion > 10)
                {
                    Console.Write("\n\tValor inválido. Ingrese un número entre 0 y 10: ");
                }
                STATS[i] = (STATS[i]._nombre, puntuacion);
            }
            CalcularCalificacion();
        }
        public string CargarPosicion()
        {
            int opcion = 0;
            string posicion = "";
            Console.Write("\n\tSeleccione la posicion:\n\t 1. Arquero.\n\t 2. Defensor.\n\t 3. Volante. \n\t 4. Delantero.\n\tOpcion: ");
            do
            {
                if(!int.TryParse(Console.ReadLine(),out opcion) || opcion < 1 || opcion > 4)
                {
                    Console.WriteLine("\n\tIngrese una opcion valida.");
                }

            } while (opcion < 1 || opcion > 4);
            switch (opcion)
            {
                case 1:
                    posicion = "ARQ";
                    Console.WriteLine("\n\tSe selecciono ARQUERO.");
                    break;
                case 2:
                    posicion = "DEF";
                    Console.WriteLine("\n\tSe selecciono DEFENSOR.");
                    break;
                case 3:
                    posicion = "VOL";
                    Console.WriteLine("\n\tSe selecciono VOLANTE.");
                    break;
                case 4:
                    posicion = "DEL";
                    Console.WriteLine("\n\tSe selecciono DELANTERO.");
                    break;
            }
            return posicion;
        } 
        public void CalcularCalificacion()
        {
            double[] pesos;
            //PONDERACION DE CADA ESTADISTICA SEGUN POSICION
            switch (this.POSICION)
            {
                case "ARQ":
                    pesos = new double[] { 0.8, 0.8, 1.2, 1.0, 1.2, 1.2, 1.0, 1.0, 1.5, 1.5 };
                    break;
                case "DEF":
                    pesos = new double[] { 1.2, 1.0, 1.2, 0.8, 1.5, 1.5, 0.8, 1.0, 1.0, 1.2 };
                    break;
                case "VOL":
                    pesos = new double[] { 1.2, 1.0, 1.5, 1.5, 1.2, 0.8, 1.2, 1.0, 0.8, 1.0 };
                    break;
                case "DEL":
                    pesos = new double[] { 1.2, 1.0, 1.0, 1.2, 0.8, 1.2, 1.5, 1.5, 0.8, 1.0 };
                    break;
                default:
                    pesos = new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
                    break;
            }
            double totalPonderado = 0;
            double totalPesos = 0;
            for (int i = 0; i < this.STATS.Length; i++)
            {
                totalPonderado += this.STATS[i]._puntuacion * pesos[i];
                totalPesos += pesos[i];
            }
            //PROMEDIAR NOTA
            double promedio = totalPonderado / totalPesos;
            if (promedio >= 9)
                this.CALIFICACION = "S";
            else if (promedio >= 8)
                this.CALIFICACION = "A";
            else if (promedio >= 7)
                this.CALIFICACION = "B";
            else if (promedio >= 6)
                this.CALIFICACION = "C";
            else if (promedio >= 5)
                this.CALIFICACION = "D";
            else
                this.CALIFICACION = "F";
            this.PUNTOTAL = Math.Round(promedio, 2);
        }
        public bool CambiarStat(int indice, int nuevaPunt)
        {
            if (indice >= 0 && indice < _stats.Length && nuevaPunt >= 0 && nuevaPunt <= 100)
            {
                _stats[indice] = (_stats[indice]._nombre, nuevaPunt);
                CalcularCalificacion(); // Siempre que cambie una stat, se recalcula
                return true;
            }
            return false;
        }
        public string DarDatos()
        {
            // Construimos la información básica del jugador
            string datos = $"\n\n\tID: {ID}\n\tNombre: {NOMBRE}\n\tPosicion: {POSICION}\n\tCalificacion: {CALIFICACION}\n\tPuntaje: {PUNTOTAL:F2}\n";

            // Agregamos las estadísticas
            datos += "\tEstadísticas:\n";
            foreach (var stat in STATS)
            {
                datos += $"\t- {stat._nombre}: {stat._puntuacion}\n";
            }
            return datos;
        }
        public string DarMenosDatos()
        {
            // Construimos la información básica del jugador
            string datos = $"\n\n\tID: {ID}\n\tNombre: {NOMBRE}\n\tPosicion: {POSICION}\n\tCalificacion: {CALIFICACION}\n\tPuntaje: {PUNTOTAL:F2}\n";
            return datos;
        }
        public string DarDatosGrupo()
        {
            // Construimos la información básica del jugador
            string datos = $"\n\n\t\tID: {ID}\n\t\tNombre: {NOMBRE}\n\t\tPosicion: {POSICION}\n\t\tCalificacion: {CALIFICACION}\n\t\tPuntaje: {PUNTOTAL:F2}\n";
            return datos;
        }
        public string FichaJugador()
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
            sb.AppendLine($"\t\t[POSICION]: {POSICION}");
            sb.AppendLine($"\t\t[CALIFICACION]: {CALIFICACION}");
            sb.AppendLine($"\t\t[PUNTUACION]: {PUNTOTAL:N2}\n");
            return sb.ToString();
        }
    }
}