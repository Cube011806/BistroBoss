using System.ComponentModel.DataAnnotations.Schema;

namespace BistroBoss.Models
{
    public class Koszyk
    {
        public int Id { get; set; }

        public string? UzytkownikId { get; set; } // może być null dla sesji (lub gdybyśmy chcieli rozszerzyć)
        public virtual Uzytkownik? Uzytkownik { get; set; }

        public virtual ICollection<KoszykProdukt> KoszykProdukty { get; set; } = new List<KoszykProdukt>();
    }

}
