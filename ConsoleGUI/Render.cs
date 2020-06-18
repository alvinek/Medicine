using System;
using System.Collections.Generic;

namespace Medicine
{
    static class ConsoleGUI
    {
        public static void Render(string str)
        {
            Console.Clear();
            StartEndRender();
            LineRender(str);
        }

        public static void Render(List<string> strList)
        {
            Console.Clear();
            StartEndRender();
            foreach (var str in strList)
            {
                LineRender(str);
            }
            StartEndRender();
        }

        static void StartEndRender() => Console.WriteLine("*************************************");
        static void LineRender(string str) => Console.WriteLine($"* {str} ");

        public static string PromptRender(string str)
        {
            LineRender(str);
            Console.Write("#: ");
            return Console.ReadLine();
        }

        public static int PromptRenderInt(string str)
        {
            LineRender(str);
            int parsedInput;

            while (true)
            {
                Console.Write("#: ");
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out parsedInput))
                {
                    break;
                }
                else
                {
                    ErrorRender("Spróbuj podać jeszcze raz");
                }
            }

            return parsedInput;
        }

        public static decimal PromptRenderDecimal(string str)
        {
            LineRender(str);
            decimal parsedInput;

            while (true)
            {
                Console.Write("#: ");
                var userInput = Console.ReadLine();
                if (decimal.TryParse(userInput, out parsedInput))
                {
                    break;
                }
                else
                {
                    ErrorRender("Spróbuj podać jeszcze raz");
                }
            }

            return parsedInput;
        }

        public static void ErrorRender(string str, bool wait = false)
        {
            if (!wait)
            {
                Console.WriteLine("*ERR: " + str);
                return;
            }

            str += " Naciśnij ENTER";
            Console.WriteLine("*ERR: " + str);
            Console.ReadLine();
        }
    }
}
