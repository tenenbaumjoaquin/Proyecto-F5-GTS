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
            Console.WriteLine("\t***********************************************\n");
            Console.WriteLine("\n\t[JUGADORES]");
            Console.WriteLine("\n\t    [1] Listar Jugadores.");
            Console.WriteLine("\n\t    [2] Crear Jugador.");
            Console.WriteLine("\n\t    [3] Modificar Jugador");
            Console.WriteLine("\n\t    [4] Eliminar Jugador.");
            Console.WriteLine("\n\t[GRUPOS]");
            Console.WriteLine("\n\t    [5] Listar Grupos.");
            Console.WriteLine("\n\t    [6] Crear Grupo.");
            Console.WriteLine("\n\t    [7] Agregar Jugador a Grupo.");
            Console.WriteLine("\n\t    [8] Eliminar Jugador de Grupo.");
            Console.WriteLine("\n\t    [9] Eliminar Grupo.");
            Console.WriteLine("\n\t[0] Salir y guardar.");
            return PedirDatoMenu();
        }

        //Retorna el valor de la opcion elegida
        public static int PedirDatoMenu()
        {
            int opcion;
            Console.WriteLine("\n\tIngrese una opcion entre 1 y 9.\n\tIngrese 0 para salir.");
            while (true) // Bucle infinito hasta que se ingrese un valor válido
            {
                Console.Write("\tOpción: ");
                if (int.TryParse(Console.ReadLine(), out opcion) && opcion >= 0 && opcion <= 9)
                {
                    return opcion; // Retorna la opción válida
                }
                Console.WriteLine("\tEntrada inválida. Ingrese una opción entre 0 y 9.");
            }
        }
        public static string LeerString(string mensaje)
        {
            Console.WriteLine(mensaje);
            string lectura = Console.ReadLine();
            return lectura;
        }

        //Validacion de Int
        public static int ValInt(string mensaje)
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
                    Console.WriteLine("\tEl numero ingresado no es valido. Ingrese otro.");
                }
            }
        }
        // Leer los ID
        public static List<int> ValIDS( string input, Dictionary<int, Jugador> idDiccionario ) 
        {
            List<int> listaNumeros = new List<int>();
            if (string.IsNullOrWhiteSpace(input))
                return listaNumeros; // Devuelve una lista vacía si el input es nulo o vacío
            string[] partes = input.Split(',');
            foreach (string parte in partes)
            {
                if (int.TryParse(parte.Trim(), out int numero))
                {
                    if (idDiccionario.ContainsKey(numero)) // Verifica si el número está en el diccionario
                    {
                        listaNumeros.Add(numero);
                    }
                    else
                    {
                        Console.WriteLine($"Advertencia: '{numero}' no está en la lista de IDs válidos y se omitirá.");
                    }
                }
                else
                {
                    Console.WriteLine($"Advertencia: '{parte}' no es un número válido y se omitirá.");
                }
            }
            return listaNumeros;
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
