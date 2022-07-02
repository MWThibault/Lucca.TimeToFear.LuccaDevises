using LuccaDevises.Core;
using System;

namespace LuccaDevises
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DevisesFileParser parser = new DevisesFileParser();
                parser.Parse("datas.txt");

                DevisesConverter devisesConverter = new DevisesConverter(parser.DevisesRepo);
                Console.WriteLine(devisesConverter.Convert(parser.DeviseStart, parser.DeviseStop, parser.DeviseConvertValue)?.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
