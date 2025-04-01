using System.ComponentModel.DataAnnotations;

namespace BistroBoss.Models
{
    public class Produkt
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string Nazwa { get; set; } = string.Empty;
        [MaxLength(150)]
        public string Opis { get; set; } = string.Empty;
        public float Cena { get; set; }
        public int CzasPrzygotowania { get; set; }
        public int KategoriaId { get; set; }
        public virtual Kategoria Kategoria { get; set; } = null!;
        public virtual ICollection<ZamowienieProdukt> ZamowieniaProduktu { get; set; } = new List<ZamowienieProdukt>();
    }
}
