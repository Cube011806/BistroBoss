namespace BistroBoss.Models
{
    public class ZamowienieProdukt
    {
        public int Id { get; set; }
        public int ZamowienieId { get; set; }
        public virtual Zamowienie Zamowienie { get; set; } = null!;
        public int ProduktId { get; set; }
        public virtual Produkt Produkt { get; set; } = null!;
    }
}
