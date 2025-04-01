using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BistroBoss.Models
{
    public class Status
    {
        public int Id { get; set; }
        //to jeszcze trzeba zrobić unique
        [MaxLength(15)]
        public string Nazwa { get; set; } = string.Empty;
        //nie wiem czy to zadziała
        public virtual ICollection<Zamowienie> Zamowienia { get; set; } = new List<Zamowienie>();
    }
}
