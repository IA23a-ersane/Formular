using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Formular
{
    public class User
    {
        public int Id { get; set; }  // Primärschlüssel
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Email { get; set; }
        public string Telefonnummer { get; set; }
        public DateTime Geburtsdatum { get; set; } = DateTime.Now;
        public bool IsKaleArkasiSelected { get; set; }
        public bool IsMisafirSelected { get; set; }
        public bool IsUstKatSelected { get; set; }
        public bool IsAltKatSelected { get; set; }
        public bool IsVIPAndClubLevelSelected { get; set; }

        // Navigation property for the many-to-many relationship
        public ICollection<Spiel> Spiele { get; set; } = new List<Spiel>();
    }
}