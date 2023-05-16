namespace Karmaplusplus.Models;

    public class VolunteeringResponse
    {
        public List<Volunteering> Volunteerings { get; set; } = new List<Volunteering>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }