namespace EvidenciaStudentov.ViewModels
{
    public class PaginatedViewModel<T>
    {
        public List<T> Items { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

