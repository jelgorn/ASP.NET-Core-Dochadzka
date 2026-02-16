using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenciaStudentov.Models
{
        public class Predmet
      {
        [Key]
        public int PredmetId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nazov { get; set; } = string.Empty;

        [StringLength(500)]
        public string Popis { get; set; } = string.Empty;

        public int UcitelId { get; set; } // ID učiteľa
        [ForeignKey(nameof(UcitelId))]
        public virtual Pouzivatel? Ucitel { get; set; } // Vzťah na učiteľa

        public virtual ICollection<Znamka> Znamky { get; set; } = new List<Znamka>();

        public virtual ICollection<Dochadzka>? Dochazky { get; set; }
        public virtual ICollection<PriradeniePredmetovUcitelom>? PriradenePredmetyUcitelom { get; set; }
        public virtual ICollection<PriradeniePredmetovZiakom>? PriradeniePredmetovZiakom { get; set; }
      }
}

