using System;
using System.Diagnostics;

namespace MathFuncsLab
{
    [DebuggerDisplay("n={N}, результат={Result}")]
    public class FactorialResult
    {
        public int N { get; set; }
        public long Result { get; set; }
    }

    [DebuggerDisplay("n={N}, результат={Result}")]
    public class FibResult
    {
        public int N { get; set; }
        public long Result { get; set; }
    }

    public static class MathFuncs
    {
        private static readonly TraceSource ts = new TraceSource("MathFuncs");

        static MathFuncs()
        {
#if DEBUG
            ts.Switch.Level = SourceLevels.All;
#else
            ts.Switch.Level = SourceLevels.Error;
#endif
            ts.Listeners.Clear();
            ts.Listeners.Add(new ConsoleTraceListener());
        }

        
        private static bool traceRecursion = false;
        private const int TraceLimit = 10;
 

        public static FactorialResult FactorialIterative(int n)
        {
            Debug.Assert(n >= 0, "Факториал не определён для отрицательных");
            Debug.Assert(n <= 20, "Переполнение long выше 20!");

            long result = 1;
            try
            {
                for (int i = 2; i <= n; i++)
                {
                    result = checked(result * i);
                }
            }
            catch (OverflowException)
            {
                ts.TraceEvent(TraceEventType.Error, 3, "Переполнение при вычислении {0}! (итеративно)", n);
                throw;
            }

            return new FactorialResult { N = n, Result = result };
        }

        public static FactorialResult FactorialRecursive(int n)
        {
            Debug.Assert(n >= 0, "Факториал не определён для отрицательных");
            Debug.Assert(n <= 20, "Переполнение long выше 20!");

            ts.TraceEvent(TraceEventType.Verbose, 1, "Вход в рекурсию: факториал n={0}", n);
            traceRecursion = n <= TraceLimit;

            long res;
            try
            {
                res = FactRec(n);
            }
            catch (OverflowException)
            {
                ts.TraceEvent(TraceEventType.Error, 3, "Переполнение при вычислении {0}! (рекурсивно)", n);
                throw;
            }

            return new FactorialResult { N = n, Result = res };
        }

        private static long FactRec(int n)
        {
            if (traceRecursion)
            {
                Debug.WriteLine($"рекурсия: n={n}");
                Debug.Indent();
            }

            long res;
            if (n <= 1)
                res = 1;
            else
                res = checked(n * FactRec(n - 1));

            if (traceRecursion)
            {
                Debug.Unindent();
            }
            return res;
        }

        

        public static FibResult FibIterative(int m)
        {
            Debug.Assert(m >= 0);

            if (m < 2)
                return new FibResult { N = m, Result = m };

            long a = 0, b = 1;
            try
            {
                for (int i = 2; i <= m; i++)
                {
                    long c = checked(a + b);
                    a = b;
                    b = c;
                }
            }
            catch (OverflowException)
            {
                ts.TraceEvent(TraceEventType.Error, 3, "Переполнение при вычислении Fib({0}) (итеративно)", m);
                throw;
            }

            return new FibResult { N = m, Result = b };
        }

        public static FibResult FibRecursive(int m)
        {
            Debug.Assert(m >= 0);
ts.TraceEvent(TraceEventType.Verbose, 1, "Вход в рекурсию: Fib m={0}", m);

            if (m > 30)
            {
                ts.TraceEvent(TraceEventType.Warning, 2,
                    "m={0} > 30: рекурсивный Фибоначчи будет считаться очень долго!", m);
            }

            traceRecursion = m <= TraceLimit;
            long res = FibRec(m);
            return new FibResult { N = m, Result = res };
        }

        private static long FibRec(int n)
        {
            if (traceRecursion)
            {
                Debug.WriteLine($"рекурсия: n={n}");
                Debug.Indent();
            }

            long res;
            if (n < 2)
                res = n;
            else
                res = FibRec(n - 1) + FibRec(n - 2);

            if (traceRecursion)
            {
                Debug.Unindent();
            }
            return res;
        }
    }

    class Program
    {
        
        static double Ms(Stopwatch sw)
        {
            return sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string line = Console.ReadLine();

                if (!int.TryParse(line, out int value))
                {
                    Console.WriteLine("Это не число, попробуйте ещё раз.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Число должно быть от {min} до {max}.");
                    continue;
                }
                return value;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Факториал и числа Фибоначчи ===");

            
            int n = ReadInt("Введите n для факториала (0..20): ", 0, 20);
            int m = ReadInt("Введите m для Фибоначчи (0..90): ", 0, 90);

            Stopwatch sw = new Stopwatch();

            
            sw.Restart();
            FactorialResult fi = MathFuncs.FactorialIterative(n);
            sw.Stop();
            Console.WriteLine($"\n{n}! (итеративно)  = {fi.Result}, время: {Ms(sw):F4} мс");

#if DEBUG
            sw.Restart();
            FactorialResult fr = MathFuncs.FactorialRecursive(n);
            sw.Stop();
            Console.WriteLine($"{n}! (рекурсивно)  = {fr.Result}, время: {Ms(sw):F4} мс");
#endif

            
            sw.Restart();
            FibResult bi = MathFuncs.FibIterative(m);
            sw.Stop();
            Console.WriteLine($"\nFib({m}) (итеративно) = {bi.Result}, время: {Ms(sw):F4} мс");

#if DEBUG
            if (m > 45)
            {
                Console.WriteLine("Рекурсивный Fib для m > 45 пропущен (слишком долго).");
            }
            else
            {
                if (m > 30)
                    Console.WriteLine("Внимание: m > 30, рекурсивный вариант может считаться долго...");

                sw.Restart();
                FibResult br = MathFuncs.FibRecursive(m);
                sw.Stop();
                Console.WriteLine($"Fib({m}) (рекурсивно) = {br.Result}, время: {Ms(sw):F4} мс");
            }
#endif

            Console.WriteLine("\nГотово. Нажмите Enter для выхода.");
            Console.ReadLine();
        }
    }
}