using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Evaluador
{
    class Eval
    {
        // Funcion que convierte un string en un
        // ArraList. La funcion recibe como parametros
        // el string y almacena cada caracter en un
        // ArrayList. Devuelve el ArrayList con cada
        // caracter almacenado.
        static ArrayList ToArrayList(string input){
            // ArrayList que guardará cada caracter de
            // string recibido como parametro.
            ArrayList chars = new ArrayList();

            // Ciclo foreach que recorre cada caracter c
            // el string input y lo agrega al ArrayList.
            foreach(char c in input) chars.Add(c.ToString());
            
            // Se retorna el ArrayList con los caracteres
            // almacenados.
            return chars;
        }

        // Funcion que convierte una expresion matematica
        // en notacion infija a postfija. La funcion recibe
        // como parametro la expresion matematica como un
        // string y ejecuta el Algoritmo de Shunting Yard
        // devuelve la expresion matematica en notacion 
        // postfija.
        //
        // ref 1 -- https://es.wikipedia.org/wiki/Algoritmo_shunting_yard
        // ref 2 -- https://www.chris-j.co.uk/parsing.php
        static ArrayList ToPostFix(string input) {
            // ArrayList que guardará cada caracter
            // de la expresion matematica con la ayuda
            // de la funcion ToArrayList().
            ArrayList chars = ToArrayList(input);
            // ArrayList que guardará cada caracter de la notacion
            // postfija.
            ArrayList output = new ArrayList();
            
            // Pila que guardará cada operador del string
            // que recibe como parametro.
            Stack operators = new Stack();

            // Comienza el algoritmo analizando cada caracter del
            // string almacenados en el ArrayList chars.
            foreach(string c in chars) {
                // Si el caracter es un numero, se agrega directamente
                if(IsNumber(c)) output.Add(c);
                // Si el caracter es un operador A (+, -, /, *, ^) ....
                else if(IsOperator(c)) {
                    // Si no es el primer operador que nos encontramos...
                    if(operators.Count != 0) {
                        // mientras haya operadores almacenados en la pila
                        while (operators.Count != 0)
                        {   
                            // Si el operador A en la pila es de 
                            // mayor jerarquia que el operador B,
                            // se quita el operador A de la pila
                            // y se agrega al ArrayList output.
                            if (Value((operators.Peek()).ToString()) >= Value(c))
                            {   
                                output.Add(operators.Pop());
                            }
                            // Si no, termina el ciclo.
                            else
                            {
                                break;
                            }
                        }
                        // Se agrega el operador B
                        operators.Push(c);
                    } 
                    // Si es el primer operador que nos encontramos, 
                    // se agrega directamente a la pila.
                    else {
                        operators.Push(c);
                    } 
                }
                // Si el caracter es un parentesis abierto...
                else if(c.Equals("(")) {
                    // Se agrega a la pila
                    operators.Push(c);
                }
                // Si el caracter es un parentesis cerrado...
                else if(c.Equals(")")) {
                    // Se inicia un ciclo y termina hasta que nos
                    // encontremos en la pila el parentesis abierto
                    // que se agrego anteriormente.
                    while(true) {
                        // Si el operador en la pila es un parentesis
                        // abierto, se saca de la pila y termina el
                        // ciclo
                        if(((operators.Peek()).ToString()).Equals("(")){
                            operators.Pop();
                            break;
                        } 
                        // Si no se encuentra al parentesis abierto, 
                        // agrega al ArrayList output el operador
                        // que haya en la pila.
                        else {
                            output.Add(operators.Pop());
                        }
                    }
                }
            }

            // Mientras hayan operadores sin agregar al ArrayList
            while(operators.Count != 0) {
                // Se agregan los operadores faltantes
                // al ArrayList.
                output.Add(operators.Pop());
            }

            // Se devuelve el ArrayList que guarda cada caracter
            // de la notacion postfija.
            return output;
        }

        // Algoritmo para resolver una expresion en notacion postfija.
        // La funcion Solve recibe como parametro una expresion
        // matematica en notacion infija, hace la conversion de infija
        // a postfija y devuelve el resulta de la expresion.
        public static double Solve(string expression) {
            // ArrayList que guardará cada caracter
            // de la expresion en notacion postfija
            // con la ayuda de la funcion ToPostFix();
            ArrayList chars = ToPostFix(expression);

            // Variable que guardará el resultado de
            // la expresion.
            double result = 0;
            
            // Ciclo for para recorrer cada elemento del
            // ArrayList chars.
            for(int i = 0; i < chars.Count; i++) {
                // Si el caracter es un operador, quiere decir que
                // los dos caracteres anteriores son los dos numeros
                // a resolver con dicho operador. Ejemplo: expresion 
                // en notacion postfija -> 23-
                //
                // El operador es un '-' y los dos caracteres
                // anteriores son los numeros que se resolverán con
                // el operador encontrado 2 - 3 = -1. El resulado
                // ocuparía la posicion del operador y las posiciones
                // anterios se eliminan.
                if(IsOperator(chars[i].ToString())) {
                    // Se guarada el operador encontrado
                    string c = chars[i].ToString();
                    // Se almacena los valores de los dos caracteres
                    // anteriores.
                    string x = chars[i-2].ToString();
                    string y = chars[i-1].ToString();

                    // Dependiendo el tipo de operador se realizará la
                    // operacion correspondiente con la ayuda de las
                    // funciones de operaciones aritmeticas.
                    if(c.Equals("+")) result = Add(Convert.ToDouble(x), Convert.ToDouble(y));
                    else if(c.Equals("-")) result = Sub(Convert.ToDouble(x), Convert.ToDouble(y));
                    else if(c.Equals("*")) result = Mult(Convert.ToDouble(x), Convert.ToDouble(y));
                    else if(c.Equals("/")) result = Div(Convert.ToDouble(x), Convert.ToDouble(y));
                    else if(c.Equals("^")) result = Pow(Convert.ToDouble(x), Convert.ToDouble(y));

                    // Se agrega el resultado en la posicion del operador
                    chars[i] = result;
                    // Se eliminan las posiciones anteriores de los numeros,
                    // por si aun hay más operadores por encontrar en el
                    // ArrayList.
                    chars.RemoveAt(i-1);
                    chars.RemoveAt(i-2);

                    // Se reinicia el indice para volver a la posicion inicial
                    // debido a la eliminacion de las dos posiciones anteriores.
                    i = 0;
                }
            }

            // El unico elemento que queda en el ArrayList
            // es el resultado de todo la expresion en 
            // notacion postfija.
            return Convert.ToDouble(chars[0]);
        }

        // Funcion que valida si un caracter es 
        // una numero. La funcion recibe como
        // parametro un caracter de tipo string
        // y si es un numero retorna true.
        static Boolean IsNumber(string c) {
            // Expresion regular para validar
            // si el caracter es un numero. 
            Regex rgx = new Regex(@"\b[0-9]");
            Match match = rgx.Match(c);

            // Si es un numero retorna un 
            // valor true.
            if(match.Success) return true;
            else return false;
        }

        // Funcion que valida si un caracter es 
        // un operador. La funcion recibe como
        // parametro un caracter de tipo string
        // y si es un operador retorna true.
        static Boolean IsOperator(string c) {
            if(c.Equals("+") || c.Equals("-") || c.Equals("*") || c.Equals("/") || c.Equals("^")) return true;
            else return false;
        }

        // Funcion que retorna el valor 
        // jerarquico de un operador.
        // La funcion recibe como parametro
        // el operador como string.
        static int Value(string c) {
            if(c.Equals("+") || c.Equals("-")) return 1;
            else if(c.Equals("*") || c.Equals("/")) return 2;
            else if(c.Equals("^")) return 3;
            else return 0;
        }

        // Funciones para realizar las operaciones aritmeticas
        // Reciben como parametros los dos numeros para realizar
        // las operaciones. Retornan el resultado.
        static double Add(double x, double y) { return x + y; }
        static double Sub(double x, double y) { return x - y; }
        static double Div(double x, double y) { return x / y; }
        static double Mult(double x, double y) { return x * y; }
        static double Pow(double x, double y) { return Math.Pow(x, y); }
    }
}
