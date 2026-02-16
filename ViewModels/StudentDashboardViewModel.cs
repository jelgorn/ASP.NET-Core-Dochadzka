namespace EvidenciaStudentov.ViewModels
{
    public class StudentDashboardViewModel
    {
        public StudentProfilViewModel Profil { get; set; } = new();
        public List<NovaZnamkaViewModel> Znamky { get; set; } = new();
    }
}

