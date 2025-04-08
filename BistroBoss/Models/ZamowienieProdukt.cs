using System.ComponentModel.DataAnnotations;

namespace BistroBoss.Models
{
    public class ZamowienieProdukt
    {
        [Key]
        public int Id { get; set; }
        public int ZamowienieId { get; set; }
        public virtual Zamowienie Zamowienie { get; set; } = null!;
        public int ProduktId { get; set; }
        public virtual Produkt Produkt { get; set; } = null!;
    }
}
