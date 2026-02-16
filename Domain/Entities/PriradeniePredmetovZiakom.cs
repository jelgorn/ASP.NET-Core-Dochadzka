using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenciaStudentov.Models
{
    public class PriradeniePredmetovZiakom
    {
        [Key]
        public int PriradeniePredZiakId { get; set; }
        [Required]
        public int PouzivatelId { get; set; }
        [Required]
        public int PredmetId { get; set; }

        [ForeignKey(nameof(PouzivatelId))]
        public virtual Pouzivatel? Pouzivatel { get; set; }
        [ForeignKey(nameof(PredmetId))]
        public virtual Predmet? Predmet { get; set; }
    }
}
