# Evaluador-de-Expresiones
Implementación del Algoritmo de Shunting Yard para un evaluador de expresiones aritmeticas

## Resolviendo una expresion matematica
Para resolver una expresion matematica se utiliza la funcion Solve() que recibe como parametro la expresion en un string

```c#
  // Se guarda el resultado de la expresion 
  double resultado = Eval.Solve("(2 + 2) * 2");
  Console.WriteLine("Resultado = " + resultado);
  
  // Resultado en consola
  // Resultado = 8
```
