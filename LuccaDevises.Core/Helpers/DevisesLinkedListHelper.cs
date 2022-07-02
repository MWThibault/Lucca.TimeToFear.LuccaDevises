using LuccaDevises.Core.Entities;

namespace LuccaDevises.Core.Helpers
{
    public static class DevisesLinkedListHelper
    {
        public static DevisesLinkedListItem GetClone(this DevisesLinkedListItem item)
        {
            return item.GetClone(null);
        }

        public static int CountLinkedList(this DevisesLinkedListItem item)
        {
            return item.GetStartingItem().CountLinkedList(1);
        }

        public static DevisesLinkedListItem GetStartingItem(this DevisesLinkedListItem item)
        {
            if (item.Previous == null)
                return item;
            return GetStartingItem(item.Previous);
        }


        private static DevisesLinkedListItem GetClone(this DevisesLinkedListItem item, DevisesLinkedListItem previous)
        {
            DevisesLinkedListItem result = new DevisesLinkedListItem(item.Previous != previous ? item.Previous?.GetClone(item) : null);
            result.Item = item.Item;
            result.Operators = item.Operators;
            result.Next = item.Next?.GetClone(item);

            return result;
        }

        private static int CountLinkedList(this DevisesLinkedListItem item, int level)
        {
            if (item.Next != null)
                return item.Next.CountLinkedList(++level);
            return level;
        }
    }
}
