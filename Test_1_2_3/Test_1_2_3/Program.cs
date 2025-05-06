using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите строку:");
            string str = Console.ReadLine();

            LB(ref str);

            if (Regex.IsMatch(str, "^[a-zA-Z]+$"))
            {
                string comp = Comp(str);

                Console.WriteLine("Сжатая строка: " + comp);
            }

            else
            {
                string decomp = Decomp(str);

                Console.WriteLine("Исходная строка: " + decomp);
            }
        }

        static private string LB(ref string str)
        {
            bool prov = true;

            while (prov)
            {
                if (Regex.IsMatch(str, "^[a-z][a-z0-9]*$") && !string.IsNullOrEmpty(str))
                {
                    Console.WriteLine($"Строка: {str}");
                    prov = false;
                    break;
                }

                else
                {
                    Console.WriteLine("Некорректный ввод или строка содержит недопустимые символы!");
                    str = Console.ReadLine();
                }

            }
            return str;
        }

        static private string Comp(string str)
        {
            var sb = new StringBuilder(str.Length);
            int count = 1;

            for (int i = 1; i <= str.Length; i++)
            {
                if (str[i] == str[i - 1])
                {
                    count++;
                }
                else
                {
                    sb.Append(str[i - 1]);
                    if (count > 1)
                        sb.Append(count);
                    count = 1;
                }
            }

            return sb.ToString();
        }

        static private string Decomp(string str)
        {
            var sb = new StringBuilder(str.Length * 2);
            var regex = new Regex(@"([a-zA-Z])(\d+)?", RegexOptions.Compiled);

            foreach (Match match in regex.Matches(str))
            {
                if (!match.Success)
                    continue;

                char ch = match.Groups[1].Value[0];
                int count = 1;

                if (match.Groups[2].Success)
                {
                    if (!int.TryParse(match.Groups[2].Value, out count) || count < 1)
                        Console.WriteLine($"Некорректное число: {match.Value}"); 
                }

                sb.Append(ch, count);
            }

            return sb.ToString();
        }
    }
}
