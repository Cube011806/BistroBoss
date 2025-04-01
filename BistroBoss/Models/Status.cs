using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BistroBoss.Models
{
    public class Status
    {
        public int Id { get; set; }
        [MaxLength(15)]
        public string Nazwa { get; set; } = string.Empty;
        public virtual ICollection<Zamowienie> Zamowienia { get; set; } = new List<Zamowienie>();
    }
}
