using System;

namespace FlatsLib {
    public class Flat {
        public int Id { get; internal set; }
        public string Building { get; set; }
        public string Metro { get; set; }
        public string Address { get; internal set; }
        public string Price { get; internal set; }
        public bool Active { get; set; }
        public string OfferUrl { get; set; }
        public string PhotoUrl { get; set; }
        public int PhotoCount { get; set; }
        public string EscapeDate { get; internal set; }
        public DateTime MailDate { get; internal set; }

        public override bool Equals(object obj)
        {
            var flat = obj as Flat;
            return flat != null &&
                   Id == flat.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}