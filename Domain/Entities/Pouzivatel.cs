using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvidenciaStudentov.Models
{
    public class Pouzivatel
    {
        [Key]
        public int PouzivatelId { get; set; }

        [Required, StringLength(50)]
        public string Meno { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Priezvisko { get; set; } = string.Empty;

        [Required, DataType(DataType.Date)]
        public DateTime DatumNarodenia { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Heslo { get; set; } = string.Empty;

        [Required]
        public string Rola { get; set; } = string.Empty;

        public virtual ICollection<Znamka>? Znamky { get; set; }
        public virtual ICollection<Dochadzka>? Dochazky { get; set; }
        public virtual ICollection<PriradeniePredmetovUcitelom>? PriradenePredmetyUcitelom { get; set; }
        public virtual ICollection<PriradeniePredmetovZiakom>? PriradeniePredmetovZiakom { get; set; }

    }

}


