using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace EvidenciaStudentov.ViewModels
{
    public class UcitelProfilViewModel
    {   
        public string Meno { get; set; } = string.Empty;
        public string Priezvisko { get; set; } = string.Empty;
        public string MenoUcitela { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        public DateTime DatumNarodenia { get; set; }

        public int PocetPredmetov { get; set; }
        public int PocetZnamok { get; set; }
        public int PocetDochadzok { get; set; }
        public int PocetStudentov { get; set; }
        
        public UpravitProfilViewModel Upravit { get; set; } = new();

        public DateTime? PoslednaZmena { get; set; }

        public List<PredmetInfo> Predmety { get; set; } = new List<PredmetInfo>();
        

        public class PredmetInfo
        {
            public int PredmetId { get; set; }
            public string Nazov { get; set; } = string.Empty;
            public string? Popis { get; set; }
            public int PocetZakov { get; set; }
            
        }

        public class UpravitProfilViewModel
        {
            public string Email { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            public string AktualneHeslo { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            public string NoveHeslo { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("NoveHeslo", ErrorMessage = "Heslá sa nezhodujú.")]
            public string PotvrditNoveHeslo { get; set; } = string.Empty;
            
        }
    }
}

