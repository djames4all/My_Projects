using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class Suggestion
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [Required]
        [StringLength(500)]
        public required string SuggestionText { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
