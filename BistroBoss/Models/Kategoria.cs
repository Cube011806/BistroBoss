﻿using System.ComponentModel.DataAnnotations;

namespace BistroBoss.Models
{
    public class Kategoria
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nazwa { get; set; } = string.Empty;
        public virtual ICollection<Produkt> Produkty { get; set; } = new List<Produkt>();
    }
}
