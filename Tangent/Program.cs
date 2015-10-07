using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tangent
{
    class Program
    {
        // Возвращает n!
        // Будем считать в double т.к. значения могут не поместиться в long
        private static double Factorial(long n)
        {
            var factorial = 1.0;
            for (var i = 1; i <= n; i++)
                factorial *= i;

            return factorial;
        }

        // Возвращает число Бернулли с индексом 'n'
        private static double BernulliNumber(long n)
        {
            if (n == 0)
                return 1;

            var sum = 0.0; // значение суммы на данном шаге суммирования

            for (var k = 1; k <= n; k++)
            {
                sum += (BernulliNumber(n - k) * Factorial(n + 1)) / (Factorial(k + 1) * Factorial(n - k));
            }

            return (-1 * sum) / (n + 1);
        }

        // Возвращает модуль "x"
        private static double Abs(double x)
        {
            return x < 0 ? -x : x;
        }

        // Возвращает тангенс числа "x"
        // В листочке неправильная формула,
        // правильную можно посмотреть тут: https://ru.wikipedia.org/wiki/%D0%A7%D0%B8%D1%81%D0%BB%D0%B0_%D0%91%D0%B5%D1%80%D0%BD%D1%83%D0%BB%D0%BB%D0%B8
        private static double Tg(double x)
        {
            var sum = 0.0; // значение суммы на данном шаге суммирования
            var prevSum = -1.0; // значение суммы на прошлом шаге. "-1" прост чтоб оно отличалось от "sum"
            // т.к. нам нужно вывести с точностью до двух знаков до запятой, то можно ввести eps = 0.0001
            var eps = 0.0001;

            // Такстакстакс, нам нельзя использовать функцию возведения в степень
            // и как мы будем считать 2^(2n)?
            // заметим, что на шаге суммирования (i + 1) значение 2^(2*(i + 1)) = (2^(2*i)) * 2^2
            // т.е. мы можем завести переменную, которая будет хранить значение
            // 2^(2*i) и обновлять ее на каждом шаге (умножать на 2^2 = 4)
            var powerOfTwo = 4.0; // = 2^(2*1) == 4

            // Аналогично для x^(2*n - 1)
            // только умножать будем на x^2 = x*x
            var powerOfX = x; // = x^(2*1 - 1) = x

            for (var n = 1; Abs(sum - prevSum) > eps; n++)
            {
                prevSum = sum;

                sum += (Abs(BernulliNumber(2 * n)) * powerOfTwo * (powerOfTwo - 1) * powerOfX) / Factorial(2 * n);

                powerOfTwo *= 4;
                powerOfX *= x * x;
            }

            return sum;
        }

        static void Main()
        {
            bool stop;
            do
            {
                Console.Write("Enter angle ('X'): ");

                double x;
                if (double.TryParse(Console.ReadLine(), out x))
                {
                    if (Abs(x) < Math.PI / 2)
                    {
                        Console.WriteLine("tg(x) = {0:F2}", Tg(x));
                    }
                    else
                    {
                        Console.WriteLine("tg({0}) is undefined", x);
                    }
                }
                else
                {
                    Console.WriteLine("Wrong format.");
                }

                Console.Write("Press any key to continue or 'esc' to exit...");

                stop = Console.ReadKey(true).Key == ConsoleKey.Escape;

                Console.Clear();
            } while (!stop);
        }
    }
}
