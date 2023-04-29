namespace AdaYazilimTrenRezervasyonUygulamasi.Models.ResponseClass
{
    public class Yanıt
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<YerlesimAyrinti> YerlesimAyrinti { get; set; } = new List<YerlesimAyrinti>();
    }
}
