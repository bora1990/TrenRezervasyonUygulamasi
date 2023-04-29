namespace AdaYazilimTrenRezervasyonUygulamasi.Models.RequestClass
{
    public class Tren
    {
        private string Ad = "Başkent Ekspress";
        public IEnumerable<TrenVagon> Vagonlar { get; set; }
    }
}
