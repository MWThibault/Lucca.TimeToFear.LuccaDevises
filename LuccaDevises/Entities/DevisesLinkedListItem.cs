using System;
using System.Collections.Generic;
using System.Text;

namespace LuccaDevises.Entities
{
    public class DevisesLinkedListItem
    {
        public DevisesConvertItem Item { get; set; }
        public DevisesLinkedListItem Previous { get; set; }
        public DevisesLinkedListItem Next { get; set; }
        public DevisesOperators Operators { get; internal set; }

        public DevisesLinkedListItem(DevisesLinkedListItem previous)
        {
            Previous = previous;
        }

        public override string ToString()
        {
            return Previous?.ToString() + Item.ToString() + (Operators == DevisesOperators.ToToFrom ? "/" : "*") + Item.ValueConvertion.ToString() + ";";
        }
    }
}
