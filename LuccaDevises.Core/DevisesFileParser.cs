using LuccaDevises.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LuccaDevises.Core
{
    public class DevisesFileParser
    {
        public string DeviseStart { get; private set; }
        public string DeviseStop { get; private set; }
        public double DeviseConvertValue { get; private set; }
        public List<DevisesConvertItem> DevisesRepo { get; private set; }

        public void Parse(string filePath)
        {
            Clear();

            if (!File.Exists(filePath))
                throw new Exception("Erreur fichier non trouvé");

            using (var sr = File.OpenText(filePath))
            {
                if (sr.EndOfStream)
                {
                    throw new Exception("Erreur fichier : vide");
                }
                string line1 = sr.ReadLine();
                ParseQueryConvertLine(line1);

                if (sr.EndOfStream)
                {
                    throw new Exception("Erreur fichier : manque des lignes");
                }
                string line2 = sr.ReadLine();
                if (!int.TryParse(line2, out int nbDevisesDatas))
                    throw new Exception("Erreur ligne 2 : la valeur doit être un entier");

                for (int i = 0; i < nbDevisesDatas; i++)
                {
                    if (sr.EndOfStream)
                    {
                        throw new Exception("Erreur fichier : manque des lignes de convertion");
                    }
                    string lineDevise = sr.ReadLine();
                    ParseRepoLine(lineDevise, i + 3); // +3 pour les 2 premières lignes déja traité directement et on débute l'affichage à 1 et non 0 pour l'homme
                }
            }
        }

        private void ParseRepoLine(string lineDevise, int i)
        {
            if (!String.IsNullOrEmpty(lineDevise))
            {
                string[] lineDeviseExplode = lineDevise.Split(";");
                if (lineDeviseExplode.Length == 3)
                {
                    string[] convertValueExplode = lineDeviseExplode[2].Split(".");
                    
                    if (convertValueExplode.Length != 2)
                        throw new Exception(String.Format("Erreur ligne {0} : la valeur doit être un nombre décimal avec un . en séparateur", i));
                    
                    if (convertValueExplode[1].Length != 4)
                        throw new Exception(String.Format("Erreur ligne {0} : la valeur doit être un nombre décimal avec 4 décimales", i));

                    // InvariantCulture pour forcer l'usage du "." en séparateur de parsing
                    if (!double.TryParse(lineDeviseExplode[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double valueConvertionParse))
                        throw new Exception(String.Format("Erreur ligne {0} : la valeur doit être un nombre à 4 décimales positif", i));


                    if (lineDeviseExplode[0].Length != 3)
                        throw new Exception(String.Format("Erreur ligne {0} : la devise de départ doit être sur 3 caractères", i));

                    if (lineDeviseExplode[1].Length != 3)
                        throw new Exception(String.Format("Erreur ligne {0} : la devise de destination doit être sur 3 caractères", i));


                    DevisesRepo.Add(new DevisesConvertItem
                    {
                        DeviseFrom = lineDeviseExplode[0],
                        DeviseTo = lineDeviseExplode[1],
                        ValueConvertion = valueConvertionParse
                    });
                }
                else
                {
                    throw new Exception(String.Format("Erreur ligne {0} : 3 éléments requis, sous la forme DD;DA;T", i));
                }
            }
            else
            {
                throw new Exception(String.Format("Erreur ligne {0} : vide", i));
            }
        }

        private void ParseQueryConvertLine(string line1)
        {
            if (!String.IsNullOrEmpty(line1))
            {
                string[] line1Explode = line1.Split(";");
                if (line1Explode.Length == 3)
                {
                    DeviseStart = line1Explode[0];
                    if (DeviseStart.Length != 3)
                        throw new Exception("Erreur ligne 1 : la devise de départ doit être sur 3 caractères");

                    DeviseStop = line1Explode[2];
                    if (DeviseStop.Length != 3)
                        throw new Exception("Erreur ligne 1 : la devise de convertion doit être sur 3 caractères");

                    if (!int.TryParse(line1Explode[1], out int deviseConvertValueParse))
                        throw new Exception("Erreur ligne 1 : la valeur à convertir doit être un entier");
                    if (deviseConvertValueParse <= 0)
                        throw new Exception("Erreur ligne 1 : la valeur à convertir doit être supérieur à 0");
                    DeviseConvertValue = deviseConvertValueParse;
                }
                else
                {
                    throw new Exception("Erreur ligne 1 : 3 éléments requis, sous la forme D1;M;D2");
                }
            }
            else
            {
                throw new Exception("Erreur ligne 1 : vide");
            }
        }

        private void Clear()
        {
            DeviseStart = string.Empty;
            DeviseStop = string.Empty;
            DeviseConvertValue = 0;
            if (DevisesRepo == null)
                DevisesRepo = new List<DevisesConvertItem>();
            DevisesRepo.Clear();
        }
    }
}
