using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenciaStudentov.Models
{
    public class Znamka
    {
        [Key]
        public int ZnamkaId { get; set; }

        [Required]
        public int PredmetId { get; set; }

        [Required]
        public int PouzivatelId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Známka musí byť v rozsahu 1 až 5.")]
        public int Hodnota { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Datum { get; set; }

        [StringLength(250)]
        public string? Poznamka { get; set; }

        // Navigačné vlastnosti bez Required
        [ForeignKey(nameof(PredmetId))]
        public virtual Predmet? Predmet { get; set; }

        [ForeignKey(nameof(PouzivatelId))]
        public virtual Pouzivatel? Pouzivatel { get; set; }
    }
}
