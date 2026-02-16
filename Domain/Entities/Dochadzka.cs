using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenciaStudentov.Models
{
    public enum HodinaStatus
    {
        Nezucastnil = 0,
        Pritomny = 1,
        NebolaHodina = 2
    }

    public class Dochadzka
    {
        [Key]
        public int DochazkaId { get; set; }

        [Required]
        public int PredmetId { get; set; }

        [Required]
        public int PouzivatelId { get; set; }

        [Required]
        public bool JePritomny { get; set; } // true = prítomný, false = neprítomný

        [Required]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [ForeignKey(nameof(PredmetId))]
        public virtual Predmet? Predmet { get; set; }

        [ForeignKey(nameof(PouzivatelId))]
        public virtual Pouzivatel? Pouzivatel { get; set; }
    }
}

