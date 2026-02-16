using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenciaStudentov.Models
{
    public class PriradeniePredmetovUcitelom
    {
        [Key]
        public int PriradeniePredUcitelId { get; set; }

        [Required]
        public int PouzivatelId { get; set; } // ID učiteľa

        [Required]
        public int PredmetId { get; set; } // ID predmetu


        [ForeignKey(nameof(PouzivatelId))]
        public virtual Pouzivatel? Pouzivatel { get; set; }
        [ForeignKey(nameof(PredmetId))]
        public virtual Predmet? Predmet { get; set; }
    }
}
