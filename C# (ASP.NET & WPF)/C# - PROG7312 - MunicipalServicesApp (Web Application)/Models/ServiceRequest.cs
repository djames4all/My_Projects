using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MunicipalServicesApp.Models
{
    // Implement IComparable<ServiceRequest> so generic data structures can use ServiceRequest as T
    public class ServiceRequest : IComparable<ServiceRequest>
    {
        public int Id { get; set; }

        [Display(Name = "Tracking ID")]
        public string TrackingId { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Location { get; set; } = string.Empty;

        // 1 = highest
        [Range(1, 10)]
        public int Priority { get; set; } = 5;

        [StringLength(1000)]
        public string RelatedTrackingIds { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Pending, In Progress, Resolved

        public IFormFile? MediaFile { get; set; }
        public string? MediaFilePath { get; set; }

        public int CompareTo(ServiceRequest? other)
        {
            if (other == null) return 1;
            return Id.CompareTo(other.Id);
        }

        public override string ToString()
        {
            return $"{Id}:{Title}";
        }
    }
}
