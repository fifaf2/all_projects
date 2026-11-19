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

        public RationalPoint(double x, double? y, DiscontinuityType type)
        {
            X = x;
            Y = y;
            DiscontinuityType = type;
        }
    }

    [DebuggerTypeProxy(typeof(RationalTableDebugView))]
    public class RationalTable
    {
        public List<RationalPoint> Points = new List<RationalPoint>();
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

    public static class Guard
    {
        public static TraceSource trace = new TraceSource("MixedRational", SourceLevels.All);
        public static void InitTrace()
        {
            trace.Listeners.Clear();
            trace.Listeners.Add(new ConsoleTraceListener());
        }

        public static void CheckRawX(double x)
        {
            Debug.Assert(x != -1, "Полюс при x = -1");
            Debug.Assert(x != 1, "Устранимый разрыв при x = 1");
        }

        public static void CheckLimits()
        {
            Debug.Assert(
                Program.ComputeLimit(-1) == double.PositiveInfinity ||
                Program.ComputeLimit(-1) == double.NegativeInfinity,
                "Предел в -1 должен быть ±∞");

            Debug.Assert(
                Math.Abs(Program.ComputeLimit(1) - 1.5) < 1e-9,
                "Устранимый предел в 1 должен быть 3/2");
        }

        public static void LogPoint(double x, double? f, DiscontinuityType type)
        {
            Debug.WriteLine($"x={x}, f={f}, тип={type}");

            if (type != DiscontinuityType.None)
            {
                trace.TraceEvent(TraceEventType.Error, 1, $"Попадание в разрыв: x = {x}, тип = {type}");
            }
            else if (Math.Abs(x - 1) < 0.15 || Math.Abs(x + 1) < 0.15)
            {
                trace.TraceEvent(TraceEventType.Warning, 2, $"Окрестность точки ±1: x = {x}");
            }
            else
            {
                trace.TraceEvent(TraceEventType.Information, 3, $"Обычная точка: x = {x}, f = {f:F3}");
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Guard.InitTrace();
            Guard.CheckLimits();

            Console.WriteLine("Вариант 10. Функция: f(x) = (x^3 - 1) / (x^2 - 1)");
            Console.Write("Введите x: ");
            string input = Console.ReadLine().Replace(".", ",");

            if (!double.TryParse(input, out double userX))
            {
                Console.WriteLine("Ошибка: нужно ввести число.");
                return;
            }

            DiscontinuityType userType = GetTypeOfPoint(userX);

            if (userType == DiscontinuityType.Removable)
            {
                double limit = ComputeLimit(1);
                Console.WriteLine($"В точке x = 1 устранимый разрыв. Предел равен {limit}");
                CalcRawFunction(userX);
            }
            else if (userType == DiscontinuityType.Pole)
            {
                double limit = ComputeLimit(-1);
                Console.WriteLine($"В точке x = -1 разрыв второго рода (полюс). Предел равен {limit}.");
                CalcRawFunction(userX);
            }
            else
            {
                double y = CalcRawFunction(userX);
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
                DiscontinuityType type = GetTypeOfPoint(x);
                double? f = null;

                if (type == DiscontinuityType.None)
                {
                    swNone.Start();
                    f = Math.Round(CalcRawFunction(x), 4);
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

                Guard.LogPoint(x, f, type);
                table.Points.Add(new RationalPoint(x, f, type));
            }

            Console.WriteLine("\nИтоговая таблица:");
            Console.WriteLine("X \t f(X) \t\t Тип разрыва");

            foreach (RationalPoint pt in table.Points)
            {
                if (pt.Y != null)
                    Console.WriteLine($"{pt.X} \t {pt.Y} \t\t {pt.DiscontinuityType}");
                else
                    Console.WriteLine($"{pt.X} \t разрыв \t {pt.DiscontinuityType}");
            } 
            Console.WriteLine($"Время расчёта обычных точек: {swNone.Elapsed.TotalMilliseconds} мс");
            Console.WriteLine($"Время обработки устранимого разрыва: {swRemovable.Elapsed.TotalMilliseconds} мс");
            Console.WriteLine($"Время обработки полюса: {swPole.Elapsed.TotalMilliseconds} мс");
        }
        public static double CalcRawFunction(double x)
        {
            Guard.CheckRawX(x);
            return (x * x * x - 1) / (x * x - 1);
        }

        public static DiscontinuityType GetTypeOfPoint(double x)
        {
            if (Math.Abs(x - (-1)) < 1e-9)
                return DiscontinuityType.Pole;

            if (Math.Abs(x - 1) < 1e-9)
                return DiscontinuityType.Removable;

            return DiscontinuityType.None;
        }

        public static double ComputeLimit(double x)
        {
            if (Math.Abs(x - (-1)) < 1e-9)
            {
                return double.PositiveInfinity;
            }

            if (Math.Abs(x - 1) < 1e-9)
            {
                double nearX = 1.0 + 1e-7;
                double limit = (nearX * nearX * nearX - 1) / (nearX * nearX - 1);
                return Math.Round(limit, 5);
            }

            return CalcRawFunction(x);
        }
    }
}