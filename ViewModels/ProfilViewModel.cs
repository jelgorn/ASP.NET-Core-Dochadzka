using Microsoft.AspNetCore.Mvc;

namespace EvidenciaStudentov.ViewModels
{
    public class ProfilViewModel
    {
        public string Meno { get; set; } = string.Empty;
        public string Priezvisko { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DatumNarodenia { get; set; }
    }
}

