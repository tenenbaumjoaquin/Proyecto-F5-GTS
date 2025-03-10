using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ExcelDataReader;
using System.Threading.Tasks;

namespace Proyecto_F5_GTS
{
    static class Menu
    {
        public static int MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("\t***********************************************");
            Console.WriteLine("\t*    Sistema de Emparejamiento de Equipos     *");
            Console.WriteLine("\t***********************************************");
            Console.WriteLine("\n\t[1] Listar Jugadores.");
            Console.WriteLine("\n\t[2] Crear Jugador.");
            Console.WriteLine("\n\t[3] Salir y guardar.");

            return PedirDatoMenu();
        }

        //Retorna el valor de la opcion elegida
        public static int PedirDatoMenu()
        {
            int devolucion;
            Console.WriteLine("\n\tIngrese una opcion entre 1 y 3");
            do
            {
                if (!int.TryParse(Console.ReadLine(), out devolucion) || devolucion < 1 || devolucion > 3)
                {
                    Console.WriteLine("\tIngrese una opcion entre 1 y 3.");
                }
            } while (devolucion < 1 || devolucion > 3);
            return devolucion;
        }
        public static string LeerString(string mensaje)
        {
            Console.WriteLine(mensaje);
            string lectura = Console.ReadLine();
            return lectura;
        }

        //Validacion de Int
        public static int valInt(string mensaje)
        {
            int conversion = 0;
            Console.WriteLine(mensaje);
            while (true)
            {
                string lectura = Console.ReadLine()?.Trim();
                if (int.TryParse(lectura, out conversion))
                {
                    return conversion;
                }
                else
                {
                    Console.WriteLine("El numero ingresado no es valido. Ingrese otro.");
                }
            }
        }
        //Validacion de Double
        public static double ValDouble(string mensaje)
        {
            double conversion = 0;
            Console.WriteLine(mensaje);
            while (true)
            {
                string lectura = Console.ReadLine()?.Trim(); // Evita null y elimina espacios
                if (double.TryParse(lectura, out conversion))
                {
                    return conversion; // Retorna el valor si es válido
                }

                Console.WriteLine("El número ingresado no es válido. Intente nuevamente.");
            }
        }
        public static void MostrarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
            Console.WriteLine("\tPresione una tecla para seguir.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
