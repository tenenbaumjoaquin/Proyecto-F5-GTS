using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDataReader;
using System.Threading.Tasks;

namespace Proyecto_F5_GTS
{
    internal class CJugador
    {
        private int Id;
        private string Nombre;
        private string Posicion;
        private string Calificacion;
        private double punTotal;
        private (string Nombre, int Puntuacion)[] Stats =
        {
            ("VEL" , 0), ("AGT" , 0), ("PAS" , 0), ("GMB" , 0), ("DEF" , 0),
            ("FIS" , 0), ("PEG" , 0), ("TIR" , 0), ("ATJ" , 0), ("REF" , 0)
        };

        //Propiedades R-W
        public int ID { get => Id; set => Id = value; }
        public string NOMBRE { get => Nombre ; set => Nombre = value; }
        public string POSICION { get => Posicion; set => Posicion = value; }
        public string CALIFICACION { get => Calificacion; set => Calificacion = value; }
        public double PUNTOTAL { get => punTotal; set => punTotal = value; }
        public (string Nombre, int Puntuacion)[] STATS => Stats;

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
        public CJugador()
        {
            ID = 0;
            NOMBRE = "";
            POSICION = "";
            CALIFICACION = "";
            Stats = new (string Nombre, int Puntacion )[] 
            { 
                ("VEL", 0), ("AGT", 0), ("PAS", 0), ("GMB", 0), ("DEF", 0),
                ("FIS", 0), ("PEG", 0), ("TIR", 0), ("ATJ", 0), ("REF", 0) 
            };
        }

        //Constructor parcialmente parametrizado
        public CJugador (int id, string nombre)
        {
            this.ID = id;
            this.NOMBRE = nombre;
            this.POSICION = cargarPosicion();
            CargarStats();
        } 
        //Constructor con un parametro para busqueda en lista
        public CJugador ( string nombre)
        {
            this.NOMBRE = nombre;
        }
        //Constructor totalmente parametrizado (UTILIZADO PARA POBLAR LA LISTA CUANDO SE CARGA DESDE EL ARCHIVO)
        public CJugador (int id, string nombre, string posicion, string calificacion, double punTotal, (string Nombre, int Puntuacion) [] stats )
        {
            this.ID = id;
            this.NOMBRE = nombre;
            this.POSICION = posicion;
            this.CALIFICACION = calificacion;
            this.PUNTOTAL = punTotal;
            for (int i = 0; i < stats.Length; i++)
            {
                this.Stats[i] = stats[i];
            }
        }

        public void CargarStats()
        {
            for (int i = 0; i < Stats.Length; i++)
            {
                string nombreCompleto = NombresCompletosStats.ContainsKey(Stats[i].Nombre)
                ? NombresCompletosStats[Stats[i].Nombre]
                : Stats[i].Nombre; // Si no existe en el diccionario, usa el mismo nombre

                Console.Write($"\n\tIngrese puntuación para {nombreCompleto}: ");
                int puntuacion;
                while (!int.TryParse(Console.ReadLine(), out puntuacion) || puntuacion < 0 || puntuacion > 10)
                {
                    Console.Write("\n\tValor inválido. Ingrese un número entre 0 y 10: ");
                }
                Stats[i] = (Stats[i].Nombre, puntuacion);
            }
            calcularCalificacion();
        }

        public string cargarPosicion()
        {
            int opcion = 0;
            string posicion = "";
            Console.Write("\nSeleccione la posicion:\n 1. Arquero.\n 2. Defensor.\n 3. Volante. \n 4. Delantero.\n");
            do
            {
                if(!int.TryParse(Console.ReadLine(),out opcion) || opcion < 1 || opcion > 4)
                {
                    Console.WriteLine("\nIngrese una opcion valida.");
                }

            } while (opcion < 1 || opcion > 4);
            switch (opcion)
            {
                case 1:
                    posicion = "ARQ";
                    Console.WriteLine("\nSe selecciono ARQUERO.");
                    break;
                case 2:
                    posicion = "DEF";
                    Console.WriteLine("\nSe selecciono DEFENSOR.");
                    break;
                case 3:
                    posicion = "VOL";
                    Console.WriteLine("\nSe selecciono VOLANTE.");
                    break;
                case 4:
                    posicion = "DEL";
                    Console.WriteLine("\nSe selecciono DELANTERO.");
                    break;
            }
            return posicion;
        } 
        public void calcularCalificacion()
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
            for (int i = 0; i < this.Stats.Length; i++)
            {
                totalPonderado += this.Stats[i].Puntuacion * pesos[i];
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
            this.PUNTOTAL = totalPonderado;
        }

        public string darDatos()
        {
            // Construimos la información básica del jugador
            string datos = $"\n\nID: {ID}\nNombre: {NOMBRE}\nPosicion: {POSICION}\nCalificacion: {CALIFICACION}\nPuntaje: {PUNTOTAL}";

            // Agregamos las estadísticas
            datos += "Estadísticas:\n";
            foreach (var stat in STATS)
            {
                datos += $"- {stat.Nombre}: {stat.Puntuacion}\n";
            }
            return datos;
        }
    }
}
