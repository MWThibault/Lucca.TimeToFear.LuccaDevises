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
                if (args.Length != 1)
                    throw new Exception("Le programme doit être executer avec le chemin vers le fichier en paramètre comme suit : LuccaDevises <chemin vers le fichier>");

                string fileName = args[0];
                DevisesFileParser parser = new DevisesFileParser();
                parser.Parse(fileName);

                DevisesConverter devisesConverter = new DevisesConverter(parser.DevisesRepo);
                Console.WriteLine(devisesConverter.Convert(parser.DeviseStart, parser.DeviseStop, parser.DeviseConvertValue)?.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
