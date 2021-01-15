using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Evaluador
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variable que guarda el comando
            // que ingrese el usuario.
            string input;

            // Se ejecuta un ciclo while hasta
            // que el usuario ejecuta el comando
            // 'exit'
            while(true) {
                Console.Write("\n  > ");
                
                // Se guarda la expresion o comando
                // que el usuario ingrese.
                input = Console.ReadLine();

                // Si es el comando exit, se termina
                // el comando.
                if(input.Equals("exit")) break;
                // Si es el comando cls, se limpia
                // la consola.
                else if(input.Equals("cls")) Console.Clear();
                // Si no es un comando, entonces se resuelve
                // la expresion matematica.
                else Console.WriteLine("  - " + Eval.Solve(input));
            }
        }
    }
}
