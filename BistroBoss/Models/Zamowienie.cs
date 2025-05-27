using System.ComponentModel.DataAnnotations;
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
        //public int StatusId { get; set; }
        public int Status { get; set; } = 0; // 1 -złożono 2- w przygotowaniu 3 - w dostawie 4 - zrealizowano 3 - czeka na odbiór
        //public virtual Status Status { get; set; } = null!;
        [MaxLength(50)]
        public string Miejscowosc { get; set; } = string.Empty;
        [MaxLength(40)]
        public string Ulica { get; set; } = string.Empty;
        [MaxLength(10)]
        public string NumerBudynku { get; set; } = string.Empty;
        [MaxLength(10)]
        public string KodPocztowy { get; set; } = string.Empty;
        //public int DostawaId { get; set; }
        //public virtual Dostawa Dostawa { get; set; } = null!;
        public int? OpiniaId { get; set; }
        public virtual Opinia Opinia { get; set; }
        public string? UzytkownikId { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; } = null!;
        public virtual ICollection<ZamowienieProdukt> ZamowioneProdukty { get; set; } = new List<ZamowienieProdukt>();
        //public virtual Koszyk Koszyk { get; set; }
        //public int KoszykId { get; set; }
    }
}
