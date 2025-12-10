using System;

namespace MunicipalServicesApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
    }
}
