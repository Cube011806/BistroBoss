namespace BistroBoss.Models
{
    public class KoszykSessionProduktDto
    {
        public int ProduktId { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public int Ilosc { get; set; } = 1;
    }
}