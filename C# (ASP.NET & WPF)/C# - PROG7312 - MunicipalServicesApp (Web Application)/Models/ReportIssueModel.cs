using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MunicipalServicesApp.Models
{
    public class ReportIssue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a location.")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a category.")]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a description.")]
        [StringLength(1200, ErrorMessage = "Description can't be more than 1200 characters.")]
        public string Description { get; set; } = string.Empty;

        // Uploaded file (not persisted) - use controller to save
        public IFormFile? MediaFile { get; set; }

        // Saved relative path to wwwroot (e.g. /uploads/unique.png)
        public string? MediaFilePath { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // Status: Pending, In Progress, Resolved
        public string Status { get; set; } = "Pending";
    }
}
