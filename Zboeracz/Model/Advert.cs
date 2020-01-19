using System;

namespace Zbieracz
{
    public class Advert
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Url { get; set; }
        public bool IsPromoted { get; set; }
        public byte[] Picture { get; set; }

    }
}
