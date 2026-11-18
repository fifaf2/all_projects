using System;
using System.Diagnostics;

namespace Otladoch_class
{
    [DebuggerDisplay("n={N}, результат={Result}")]
    class FactorialResult
    {
        public int N { get; set; }
        public long Result { get; set; }
    }

    [DebuggerDisplay("n={N}, результат={Result}")]
    class FibResult
    {
        public int N { get; set; }
        public long Result { get; set; }
    }

    static class MathFuncs
    {
#if DEBUG
        static readonly TraceSource Log = new TraceSource("MathFuncs", SourceLevels.Verbose);
        static bool logFib = true;

        static MathFuncs()
        {

            Trace.Listeners.Add(new ConsoleTraceListener());
            Log.Listeners.Add(new ConsoleTraceListener());
        }
#endif

        public static long FactorialIter(int n)
        {
            Debug.Assert(n >= 0, "Факториал не определён для отрицательных");
            Debug.Assert(n <= 20, "Переполнение long выше 20!");
            if (n < 0)
                throw new ArgumentOutOfRangeException("n", "Факториал не определён для отрицательных");
            if (n > 20)
            {
#if DEBUG
                Log.TraceEvent(TraceEventType.Error, 1, "Переполнение long выше 20!");
#endif
                throw new OverflowException("Переполнение long выше 20!");
            }

            long result = 1;
            for (int i = 2; i <= n; i++)
                FactorialIter 
           return result;
        }

        public static long FibonacciIter(int m)
        {
            Debug.Assert(m >= 0);
            if (m < 0)
                throw new ArgumentOutOfRangeException("m");
            if (m > 92)
            {
#if DEBUG
                Log.TraceEvent(TraceEventType.Error, 2, "Переполнение long");
#endif
                throw new OverflowException("Переполнение long");
            }

            long a = 0;
            long b = 1;
            for (int i = 0; i < m; i++)
            {
                long next = a + b;
                a = b;
                b = next;
            }
            return a;
        }

#if DEBUG
        public static long FactorialRec(int n)
        {
            Debug.Assert(n >= 0, "Факториал не определён для отрицательных");
            Debug.Assert(n <= 20, "Переполнение long выше 20!");
            if (n < 0)
                throw new ArgumentOutOfRangeException("n", "Факториал не определён для отрицательных");
            if (n > 20)
            {
                Log.TraceEvent(TraceEventType.Error, 1, "Переполнение long выше 20!");
                throw new OverflowException("Переполнение long выше 20!");
            }

            Debug.WriteLine($"рекурсия: n={n}");
            Log.TraceEvent(TraceEventType.Verbose, 0, $"вход в рекурсию, n={n}");
            if (n <= 1)
                return 1;

            Debug.Indent();
            long result = n * FactorialRec(n - 1);
            Debug.Unindent();
            return result;
        }

        public static long FibonacciRec(int m)
        {
            Debug.Assert(m >= 0);
            if (m < 0)
                throw new ArgumentOutOfRangeException("m");
            if (m > 92)
            {
                Log.TraceEvent(TraceEventType.Error, 2, "Переполнение long");
                throw new OverflowException("Переполнение long");
            }

            if (m > 30 && logFib)
            {
                logFib = false;
                Log.TraceEvent(TraceEventType.Warning, 3, "m > 30: рекурсивный Фибоначчи взорвётся по времени");
            }

            if (logFib)
            {
                Debug.WriteLine($"рекурсия: n={m}");
                Log.TraceEvent(TraceEventType.Verbose, 0, $"вход в рекурсию, n={m}");
                Debug.Indent();
            }

            long result = m <= 1 ? m : FibonacciRec(m - 1) + FibonacciRec(m - 2);

            if (logFib)
                Debug.Unindent();
            return result;
        }
#endif
    }

    internal class Program
    {
        static void Main()
        {
            Console.Write("Введите n (факториал): ");
            int n = int.Parse(Console.ReadLine());
            Console.Write("Введите m (Фибоначчи): ");
            int m = int.Parse(Console.ReadLine());

            var watch = Stopwatch.StartNew();
            var factIter = new FactorialResult { N = n, Result = MathFuncs.FactorialIter(n) };
            watch.Stop();
            Console.WriteLine($"n! итеративно = {factIter.Result}, время {watch.Elapsed.TotalMilliseconds:0.###} мс");

#if DEBUG
            watch.Restart();
            var factRec = new FactorialResult { N = n, Result = MathFuncs.FactorialRec(n) };
            watch.Stop();
            Console.WriteLine($"n! рекурсивно = {factRec.Result}, время {watch.Elapsed.TotalMilliseconds:0.###} мс");
#endif

            watch.Restart();
            var fibIter = new FibResult { N = m, Result = MathFuncs.FibonacciIter(m) };
            watch.Stop();
            Console.WriteLine($"Fib итеративно = {fibIter.Result}, время {watch.Elapsed.TotalMilliseconds:0.###} мс");

#if DEBUG
            watch.Restart();
            var fibRec = new FibResult { N = m, Result = MathFuncs.FibonacciRec(m) };
            watch.Stop();
            Console.WriteLine($"Fib рекурсивно = {fibRec.Result}, время {watch.Elapsed.TotalMilliseconds:0.###} мс");
#endif
        }
    }
}
