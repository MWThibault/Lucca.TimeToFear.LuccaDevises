using System;

namespace LuccaDevises.Core.Entities
{
    public class DevisesConvertItem
    {
        public string DeviseFrom { get; set; }
        public string DeviseTo { get; set; }
        public double ValueConvertion { get; set; }

        public override string ToString()
        {
            return String.Join("=>", DeviseFrom, DeviseTo);
        }
    }
}
