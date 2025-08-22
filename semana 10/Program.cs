using System;
using System.Collections.Generic;
using System.Linq;

namespace VacunacionCovidSetTheory
{
    // Implementación original y comentada: selección aleatoria con Fisher–Yates
    // y operaciones explícitas de teoría de conjuntos con HashSet<T>.
    internal static class Program
    {
        private const int TotalCiudadanos = 500;
        private const int TamañoPfizer = 75;
        private const int TamañoAstra = 75;

        static void Main(string[] args)
        {
            // Semilla reproducible (opcional): si pasas un número por argumentos, se usa como semilla
            // para repetir el mismo escenario. Si no, se usa tiempo actual.
            int seed = args.Length > 0 && int.TryParse(args[0], out var s) ? s : Environment.TickCount;
            var rnd = new Random(seed);

            // 1) Universo de ciudadanos
            var universo = GenerarCiudadanos(TotalCiudadanos);

            // 2) Subconjuntos de vacunados por laboratorio (tamaño fijo)
            var vacunadosPfizer = SeleccionarAleatorios(universo, TamañoPfizer, rnd);
            var vacunadosAstra  = SeleccionarAleatorios(universo, TamañoAstra,  rnd);

            // 3) Operaciones de teoría de conjuntos
            // U = universo, P = Pfizer, A = Astra
            // No vacunados = U \ (P ∪ A)
            var unionPA = Union(vacunadosPfizer, vacunadosAstra);
            var noVacunados = Diferencia(universo, unionPA);

            // Ambas dosis = P ∩ A
            var ambasDosis = Interseccion(vacunadosPfizer, vacunadosAstra);

            // Solo Pfizer = P \ A
            var soloPfizer = Diferencia(vacunadosPfizer, vacunadosAstra);

            // Solo AstraZeneca = A \ P
            var soloAstra = Diferencia(vacunadosAstra, vacunadosPfizer);

            // 4) Salida
            ImprimirEncabezado("RESULTADOS DE LA CAMPAÑA DE VACUNACIÓN (Datos ficticios)");
            Console.WriteLine($"Semilla utilizada: {seed}");
            Console.WriteLine($"Total ciudadanos (U): {universo.Count}");
            Console.WriteLine($"Vacunados Pfizer (P): {vacunadosPfizer.Count}");
            Console.WriteLine($"Vacunados AstraZeneca (A): {vacunadosAstra.Count}");
            Console.WriteLine();

            ImprimirConjunto("Ciudadanos NO vacunados = U \\ (P ∪ A)", noVacunados);
            ImprimirConjunto("Ciudadanos con AMBAS dosis = P ∩ A",   ambasDosis);
            ImprimirConjunto("Ciudadanos SOLO Pfizer = P \\ A",       soloPfizer);
            ImprimirConjunto("Ciudadanos SOLO AstraZeneca = A \\ P",  soloAstra);
        }

        // --- Generación de datos ficticios ---
        private static HashSet<string> GenerarCiudadanos(int n)
        {
            var set = new HashSet<string>();
            for (int i = 1; i <= n; i++)
                set.Add($"Ciudadano {i}");
            return set;
        }

        // Selección aleatoria SIN reemplazo usando Fisher–Yates para garantizar uniformidad
        private static HashSet<string> SeleccionarAleatorios(HashSet<string> universo, int cantidad, Random rnd)
        {
            if (cantidad < 0 || cantidad > universo.Count)
                throw new ArgumentOutOfRangeException(nameof(cantidad), "Cantidad fuera del rango del universo.");

            var lista = universo.ToList();
            // Barajar parcialmente hasta 'cantidad'
            for (int i = 0; i < cantidad; i++)
            {
                int j = rnd.Next(i, lista.Count);
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
            return new HashSet<string>(lista.Take(cantidad));
        }

        // --- Operaciones de teoría de conjuntos (explícitas) ---
        private static HashSet<string> Union(HashSet<string> a, HashSet<string> b)
        {
            var res = new HashSet<string>(a);
            res.UnionWith(b);
            return res;
        }

        private static HashSet<string> Interseccion(HashSet<string> a, HashSet<string> b)
        {
            var res = new HashSet<string>(a);
            res.IntersectWith(b);
            return res;
        }

        private static HashSet<string> Diferencia(HashSet<string> a, HashSet<string> b)
        {
            var res = new HashSet<string>(a);
            res.ExceptWith(b);
            return res;
        }

        // --- Utilidad de salida ---
        private static void ImprimirEncabezado(string titulo)
        {
            Console.WriteLine(new string('=', titulo.Length));
            Console.WriteLine(titulo);
            Console.WriteLine(new string('=', titulo.Length));
            Console.WriteLine();
        }

        private static void ImprimirConjunto(string titulo, HashSet<string> conjunto, int maxMuestra = 50)
        {
            Console.WriteLine($"• {titulo}");
            Console.WriteLine($"  Conteo: {conjunto.Count}");
            if (conjunto.Count > 0)
            {
                // Muestra acotada para mantener legibilidad en consola
                var muestra = conjunto.Take(maxMuestra);
                Console.WriteLine($"  Ejemplos: {string.Join(", ", muestra)}{(conjunto.Count > maxMuestra ? ", ..." : "")}");
            }
            Console.WriteLine();
        }
    }
}

