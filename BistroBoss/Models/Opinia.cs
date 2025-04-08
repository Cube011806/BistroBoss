﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BistroBoss.Models
{
    public class Opinia
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Komentarz { get; set; } = string.Empty;
        [Range(1, 5, ErrorMessage = "Wartość oceny zamówienia musi być liczbą całkowitą z przedziału od 1 do 5!")]
        public byte Ocena { get; set; }
        public int ZamowienieId { get; set; }

        public virtual Zamowienie Zamowienie { get; set; } = null!;
        public string UzytkownikId { get; set; } = string.Empty;
        public Uzytkownik Uzytkownik { get; set; } = null!;
    }
}
