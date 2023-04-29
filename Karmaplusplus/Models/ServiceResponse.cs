namespace Karmaplusplus.Models;

    public class ServiceResponse
    {
        public List<Service> Services { get; set; } = new List<Service>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }