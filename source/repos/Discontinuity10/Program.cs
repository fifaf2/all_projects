using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Discontinuity10
{
    public enum DiscontinuityType
    {
        None,
        Removable,
        Pole
    }

    [DebuggerDisplay("x={X}, f={Y}, тип разрыва={DiscontinuityType}")]
    public class RationalPoint
    {
        public double X { get; set; }
        public double? Y { get; set; }
        public DiscontinuityType DiscontinuityType { get; set; }
    }

    [DebuggerTypeProxy(typeof(RationalTableDebugView))]
    public class RationalTable
    {
        public List<RationalPoint> Points { get; } = new List<RationalPoint>();
    }

    public class RationalTableDebugView
    {
        public List<RationalPoint> None = new List<RationalPoint>();
        public List<RationalPoint> Removable = new List<RationalPoint>();
        public List<RationalPoint> Pole = new List<RationalPoint>();

        public RationalTableDebugView(RationalTable table)
        {
            foreach (RationalPoint p in table.Points)
            {
                if (p.DiscontinuityType == DiscontinuityType.None)
                    None.Add(p);
                else if (p.DiscontinuityType == DiscontinuityType.Removable)
                    Removable.Add(p);
                else if (p.DiscontinuityType == DiscontinuityType.Pole)
                    Pole.Add(p);
            }
        }
    }

    public static class RationalFuncs
    {
        private static readonly TraceSource ts = new TraceSource("MixedRational");
        private const double Eps = 1e-9;

        static RationalFuncs()
        {
#if DEBUG
            ts.Switch.Level = SourceLevels.All;
#else
            ts.Switch.Level = SourceLevels.Error;
#endif
            ts.Listeners.Clear();
            ts.Listeners.Add(new ConsoleTraceListener());
        }

        public static void CheckLimits()
        {
            Debug.Assert(
                ComputeLimit(-1) == double.PositiveInfinity ||
                ComputeLimit(-1) == double.NegativeInfinity,
                "Предел в -1 должен быть ±∞");

            Debug.Assert(
                Math.Abs(ComputeLimit(1) - 1.5) < Eps,
                "Устранимый предел в 1 должен быть 3/2");
        }

        public static DiscontinuityType GetTypeOfPoint(double x)
        {
            if (Math.Abs(x - (-1)) < Eps)
                return DiscontinuityType.Pole;

            if (Math.Abs(x - 1) < Eps)
                return DiscontinuityType.Removable;

            return DiscontinuityType.None;
        }

        public static double CalcRawFunction(double x)
        {
            Debug.Assert(x != -1, "Полюс при x=-1");
            Debug.Assert(x != 1, "Устранимый разрыв при x=1");

            return (x * x * x - 1) / (x * x - 1);
        }

        public static double ComputeLimit(double x)
        {
            if (Math.Abs(x - (-1)) < Eps)
                return double.PositiveInfinity;

            if (Math.Abs(x - 1) < Eps)
            {
                double nearX = 1.0 + 1e-7;
                double limit = (nearX * nearX * nearX - 1) / (nearX * nearX - 1);
                return Math.Round(limit, 5);
            }

            return CalcRawFunction(x);
        }

        public static void LogPoint(double x, double? f, DiscontinuityType type)
        {
            Debug.WriteLine($"x={x}, f={f}, тип={type}");

            if (type != DiscontinuityType.None)
            {
                ts.TraceEvent(TraceEventType.Error, 3,
                    "Попадание в разрыв: x = {0}, тип = {1}", x, type);
            }
            else if (Math.Abs(x - 1) < 0.15 || Math.Abs(x + 1) < 0.15)
            {
                ts.TraceEvent(TraceEventType.Warning, 2,
                    "Окрестность точки ±1: x = {0}", x);
            }
            else
            {
                ts.TraceEvent(TraceEventType.Information, 1,
                    "Обычная точка: x = {0}, f = {1:F3}", x, f);
            }
        }
    }

    class Program
    {
        static double Ms(Stopwatch sw)
        {
            return sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
        }

        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string line = Console.ReadLine()?.Replace(".", ",");

                if (!double.TryParse(line, out double value))
                {
                    Console.WriteLine("Это не число, попробуйте ещё раз.");
                    continue;
                }
                return value;
            }
        }

        static void Main(string[] args)
        {
            RationalFuncs.CheckLimits();

            Console.WriteLine("=== Вариант 10. Функция: f(x) = (x^3 - 1) / (x^2 - 1) ===");

            double userX = ReadDouble("Введите x: ");
            DiscontinuityType userType = RationalFuncs.GetTypeOfPoint(userX);

            if (userType == DiscontinuityType.Removable)
            {
                double limit = RationalFuncs.ComputeLimit(1);
                Console.WriteLine($"В точке x = 1 устранимый разрыв. Предел равен {limit}");
#if DEBUG
                RationalFuncs.CalcRawFunction(userX);
#endif
            }
            else if (userType == DiscontinuityType.Pole)
            {
                double limit = RationalFuncs.ComputeLimit(-1);
                Console.WriteLine($"В точке x = -1 разрыв второго рода (полюс). Предел равен {limit}");
#if DEBUG
                RationalFuncs.CalcRawFunction(userX);
#endif
            }
            else
            {
                double y = RationalFuncs.CalcRawFunction(userX);
                Console.WriteLine($"Результат: f({userX}) = {Math.Round(y, 4)}");

                if (Math.Abs(userX - 1) < 0.2)
                    Console.WriteLine("Подсказка: точка близка к устранимому разрыву (x = 1).");
                else if (Math.Abs(userX + 1) < 0.2)
                    Console.WriteLine("Подсказка: точка близка к полюсу (x = -1).");
            }

            Console.WriteLine("\nСтроим таблицу на [-2; 2] с шагом 0.1:");
            RationalTable table = new RationalTable();

            Stopwatch swNone = new Stopwatch();
            Stopwatch swRemovable = new Stopwatch();
            Stopwatch swPole = new Stopwatch();

            for (double x = -2.0; x <= 2.0; x = Math.Round(x + 0.1, 1))
            {
                DiscontinuityType type = RationalFuncs.GetTypeOfPoint(x);
                double? f = null;

                if (type == DiscontinuityType.None)
                {
                    swNone.Start();
                    f = Math.Round(RationalFuncs.CalcRawFunction(x), 4);
                    swNone.Stop();
                }
                else if (type == DiscontinuityType.Removable)
                {
                    swRemovable.Start();
                    f = null;
                    swRemovable.Stop();
                }
                else if (type == DiscontinuityType.Pole)
                {
                    swPole.Start();
                    f = null;
                    swPole.Stop();
                }

                RationalFuncs.LogPoint(x, f, type);
                table.Points.Add(new RationalPoint { X = x, Y = f, DiscontinuityType = type });
            }

            Console.WriteLine("\nИтоговая таблица:");
            Console.WriteLine("X\tf(X)\t\tТип разрыва");

            foreach (RationalPoint pt in table.Points)
            {
                if (pt.Y != null)
                    Console.WriteLine($"{pt.X}\t{pt.Y}\t\t{pt.DiscontinuityType}");
                else
                    Console.WriteLine($"{pt.X}\tразрыв\t\t{pt.DiscontinuityType}");
            }

            Console.WriteLine($"\nВремя расчёта обычных точек:        {Ms(swNone):F4} мс");
            Console.WriteLine($"Время обработки устранимого разрыва: {Ms(swRemovable):F4} мс");
            Console.WriteLine($"Время обработки полюса:              {Ms(swPole):F4} мс");

            Console.WriteLine("\nГотово. Нажмите Enter для выхода.");
            Console.ReadLine();
        }
    }
}