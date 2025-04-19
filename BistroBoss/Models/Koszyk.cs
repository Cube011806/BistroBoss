using System.ComponentModel.DataAnnotations.Schema;

namespace BistroBoss.Models
{
    public class Koszyk
    {
        public int Id { get; set; }
        public virtual ICollection<Produkt> Produkty { get; set; }
        public virtual Zamowienie Zamowienie { get; set; }
        [ForeignKey("Zamowienie")]
        public virtual int ZamowienieId { get; set; }
        public string UzytkownikId { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; } = null!;
    }
}
