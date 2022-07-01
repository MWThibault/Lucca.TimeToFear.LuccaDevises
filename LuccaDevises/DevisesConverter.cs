using LuccaDevises.Entities;
using LuccaDevises.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuccaDevises
{
    public class DevisesConverter
    {
        public List<DevisesConvertItem> Devises { get; }

        private List<DevisesLinkedListItem> SearchResults { get; }

        public DevisesConverter(List<DevisesConvertItem> devises)
        {
            this.Devises = devises;
            this.SearchResults = new List<DevisesLinkedListItem>();
        }

        public int? Convert(string deviseFrom, string deviseTo, double value)
        {
            this.SearchResults.Clear();

            // recherche des routes permettant de réaliser le calcul
            Search(deviseFrom, deviseTo, 0, null);

            // Sélection de la route la plus rapide pour faire le calcul
            var minTravelResult = SearchResults.OrderBy(x => x.CountLinkedList()).FirstOrDefault();
            if (minTravelResult != null)
            {
                return Calculate(value, minTravelResult.GetStartingItem());
            }

            // pas de route trouvée
            return null;
        }

        private int Calculate(double value, DevisesLinkedListItem result)
        {
            if (result.Operators == DevisesOperators.FromToTo)
            {
                value = Math.Round(value * result.Item.ValueConvertion, 4);
            }
            else if (result.Operators == DevisesOperators.ToToFrom)
            {
                value = Math.Round(value * Math.Round(1 / result.Item.ValueConvertion, 4), 4);
            }

            if (result.Next != null)
                return Calculate(value, result.Next);

            // résultat arrondi à l'entier
            return (int)Math.Round(value, 0);
        }

        private void Search(string deviseSearch, string deviseStop, int countRecursive, DevisesLinkedListItem resultLinkedList)
        {
            // On ne peut pas affecter plus de route que d'élément dans le tableau, donc on sort de la récursivité une fois tout le tableau parcouru
            if (countRecursive == Devises.Count)
                return;

            foreach (var item in Devises)
            {
                // On évite de faire chemin inverse sur la même ligne du tableau des devises
                if (item == resultLinkedList?.Previous?.Item)
                    continue;

                // Vérification chemin de From => To
                SearchComparative(deviseSearch, deviseStop, countRecursive, resultLinkedList, item, item.DeviseFrom, item.DeviseTo, DevisesOperators.FromToTo);
                // Vérification chemin inverse de To => From
                SearchComparative(deviseSearch, deviseStop, countRecursive, resultLinkedList, item, item.DeviseTo, item.DeviseFrom, DevisesOperators.ToToFrom);
            }
            return;
        }

        private void SearchComparative(string deviseSearch, string deviseStop, int countRecursive, DevisesLinkedListItem resultLinkedList, DevisesConvertItem item, string compare1, string compare2, DevisesOperators operators)
        {
            if (compare1 == deviseSearch)
            {
                if (resultLinkedList == null)
                    resultLinkedList = new DevisesLinkedListItem(null);
                resultLinkedList.Item = item;
                resultLinkedList.Operators = operators;
                if (compare2 == deviseStop)
                {
                    SearchResults.Add(resultLinkedList.GetClone());
                }
                else
                {
                    resultLinkedList.Next = new DevisesLinkedListItem(resultLinkedList);
                    Search(compare2, deviseStop, ++countRecursive, resultLinkedList.Next);
                }
            }
        }
    }
}
