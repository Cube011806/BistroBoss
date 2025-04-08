using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace BistroBoss.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public DateTime DataZamowienia { get; set; }
        public float CenaCalkowita { get; set; }
        public int PrzewidywanyCzasRealizacji { get; set; }
        public int StatusId { get; set; }
        public virtual Status Status { get; set; } = null!;
        public int DostawaId { get; set; }
        public virtual Dostawa Dostawa { get; set; } = null!;
        public int? OpiniaId { get; set; }
        public virtual Opinia Opinia { get; set; } = null!;
        public string? UzytkownikId { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; } = null!;
        public virtual ICollection<ZamowienieProdukt> ZamowioneProdukty { get; set; } = new List<ZamowienieProdukt>();
    }
}
