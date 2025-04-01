using Microsoft.AspNetCore.Identity;

namespace BistroBoss.Models
{
    public class Uzytkownik : IdentityUser
    {
        public int AccessLevel { get; set; }
    }
}
