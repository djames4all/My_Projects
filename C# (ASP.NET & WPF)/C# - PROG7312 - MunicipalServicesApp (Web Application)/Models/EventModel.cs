using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string Category { get; set; } = "General";

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public int Priority { get; set; } = 5;

        public List<string> Tags { get; set; } = new();

        public string ImageUrl { get; set; } = "/images/placeholder-event.jpg";

        public bool IsFeatured { get; set; } = false;
    }
}
