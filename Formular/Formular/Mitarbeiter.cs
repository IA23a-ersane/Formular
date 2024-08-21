using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formular
{
    public class Mitarbeiter
    {
        //Test


        public string Name { get; set; }

        public string Vorname { get; set; }

        public DateTime Eintrittsdatum { get; set; }

        public int Personalnummer { get; set; }

        public string Abteilung { get; set; }

        public string Email { get; set; }

        public string Telefonnummer { get; set; }

        public DateTime Geburtsdatum { get; set; }


        public Mitarbeiter(string name, string vorname, DateTime eintrittsdatum, int personalnummer,
                       string abteilung, string email, string telefonnummer, DateTime geburtsdatum)
        {
            Name = name;
            Vorname = vorname;
            Eintrittsdatum = eintrittsdatum;
            Personalnummer = personalnummer;
            Abteilung = abteilung;
            Email = email;
            Telefonnummer = telefonnummer;
            Geburtsdatum = geburtsdatum;
        }


    }
}