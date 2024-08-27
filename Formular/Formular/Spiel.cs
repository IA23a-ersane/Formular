namespace Formular
{
    public class Spiel
    {
        public int Id { get; set; }  // Primärschlüssel
        public string Titel { get; set; }
        public DateTime Datum { get; set; }
        public string Ort { get; set; }
        public string Mannschaft1 { get; set; }
        public string Mannschaft2 { get; set; }
        public int? ErgebnisMannschaft1 { get; set; }
        public int? ErgebnisMannschaft2 { get; set; }

        // Navigation property for the many-to-many relationship
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}