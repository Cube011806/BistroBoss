using Microsoft.AspNetCore.Identity;

namespace BistroBoss.Models
{
    public class Uzytkownik : IdentityUser
    {
        public int AccessLevel { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public DateTime? DataUrodzenia { get; set; }

    }
}
