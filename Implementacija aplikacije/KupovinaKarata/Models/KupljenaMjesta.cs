using System.ComponentModel.DataAnnotations.Schema;

namespace KupovinaKarata.Models
{
    public class KupljenaMjesta
    {
        [ForeignKey("Karta")]
        public int IdKarte {  get; set; }
        public int BrojReda {  get; set; }
        public int BrojKolone {  get; set; }
    }
}
