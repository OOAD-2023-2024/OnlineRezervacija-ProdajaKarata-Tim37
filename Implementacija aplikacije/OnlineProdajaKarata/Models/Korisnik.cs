using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace OnlineProdajaKarata.Models
{
    public class Korisnik : IdentityUser
    {
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String JMBG { get; set; }
        public String KorisnickoIme { get; set; }
        public DateTime DatumRodjenja { get; set; }
        [Required]
        public String Email { get; set; }
        public String Password { get; set; }
        public int BrojKupljenihKarata { get; set; }
    }
}
