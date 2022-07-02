using LuccaDevises.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuccaDevises.Core
{
    public class DevisesConverter
    {
        public List<DevisesConvertItem> Devises { get; }

        private List<LinkedList<DevisesLinkedListItem>> SearchResults { get; }

        public DevisesConverter(List<DevisesConvertItem> devises)
        {
            this.Devises = devises;
            this.SearchResults = new List<LinkedList<DevisesLinkedListItem>>();
        }

        public int? Convert(string deviseFrom, string deviseTo, double value)
        {
            this.SearchResults.Clear();

            // recherche des routes permettant de réaliser le calcul
            Search(deviseFrom, deviseTo, 0, null);

            // Sélection de la route la plus rapide pour faire le calcul
            var minTravelResult = SearchResults.OrderBy(x => x.Count).FirstOrDefault();
            if (minTravelResult != null)
            {
                return Calculate(value, minTravelResult);
            }

            // pas de route trouvée
            return null;
        }

        private int Calculate(double value, LinkedList<DevisesLinkedListItem> result)
        {
            foreach (var item in result)
            {
                if (item.Operators == DevisesOperators.FromToTo)
                {
                    value = Math.Round(value * item.Item.ValueConvertion, 4);
                }
                else if (item.Operators == DevisesOperators.ToToFrom)
                {
                    value = Math.Round(value * Math.Round(1 / item.Item.ValueConvertion, 4), 4);
                }
            }

            // résultat arrondi à l'entier
            return (int)Math.Round(value, 0);
        }

        private void Search(string deviseSearch, string deviseStop, int countRecursive, LinkedList<DevisesLinkedListItem> resultLinkedList)
        {
            // On ne peut pas affecter plus de route que d'élément dans le tableau, donc on sort de la récursivité une fois tout le tableau parcouru
            if (countRecursive == Devises.Count)
                return;

            foreach (var item in Devises)
            {
                // On évite de faire chemin inverse sur la même ligne du tableau des devises
                if (item == resultLinkedList?.Last?.Value?.Item)
                    continue;

                // Vérification chemin de From => To
                SearchComparative(deviseSearch, deviseStop, countRecursive, resultLinkedList, item, item.DeviseFrom, item.DeviseTo, DevisesOperators.FromToTo);
                // Vérification chemin inverse de To => From
                SearchComparative(deviseSearch, deviseStop, countRecursive, resultLinkedList, item, item.DeviseTo, item.DeviseFrom, DevisesOperators.ToToFrom);
            }
            return;
        }

        private void SearchComparative(string deviseSearch, string deviseStop, int countRecursive, LinkedList<DevisesLinkedListItem> resultLinkedList, DevisesConvertItem item, string compare1, string compare2, DevisesOperators operators)
        {
            if (compare1 == deviseSearch)
            {
                if (resultLinkedList == null)
                    resultLinkedList = new LinkedList<DevisesLinkedListItem>();
                DevisesLinkedListItem linkedItem = new DevisesLinkedListItem();
                linkedItem.Item = item;
                linkedItem.Operators = operators;
                resultLinkedList.AddLast(linkedItem);

                if (compare2 == deviseStop)
                {
                    SearchResults.Add(resultLinkedList);
                }
                else
                {
                    Search(compare2, deviseStop, ++countRecursive, new LinkedList<DevisesLinkedListItem>(resultLinkedList));
                }
            }
        }
    }
}
