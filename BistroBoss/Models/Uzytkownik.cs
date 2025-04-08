﻿using Microsoft.AspNetCore.Identity;

namespace BistroBoss.Models
{
    public class Uzytkownik : IdentityUser
    {
        public int AccessLevel { get; set; } = 0; // 0 - Klient, 1 - Pracownik, 2 - Administrator
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public DateTime? DataUrodzenia { get; set; }
        public virtual ICollection<Opinia> Opinie { get; set; } = new List<Opinia>();
        public virtual ICollection<Zamowienie> Zamowienia { get; set; } = new List<Zamowienie>();

    }
}
