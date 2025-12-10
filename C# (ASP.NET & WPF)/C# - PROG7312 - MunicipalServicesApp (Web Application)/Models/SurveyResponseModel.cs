using System;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class SurveyResponse
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Which issue affects you the most?")]
        public string PriorityIssue { get; set; }

        [Required]
        [Display(Name = "How satisfied are you with current municipal services?")]
        public string SatisfactionLevel { get; set; }

        [Required]
        [Display(Name = "How easy is it to access services (e.g., billing, reporting issues)?")]
        public string EaseOfAccess { get; set; }

        [Required]
        [Display(Name = "How much do you trust the municipality to use this app to improve services?")]
        public string TrustLevel { get; set; }

        [Required]
        [Display(Name = "Which communication channels do you prefer for service updates?")]
        public string PreferredChannel { get; set; }

        [Required]
        [Display(Name = "Would you participate in future workshops or testing sessions?")]
        public string ParticipateWorkshops { get; set; }


        [Required]
        [StringLength(500)]
        [Display(Name = "Any suggestions for improvement?")]
        public string Suggestions { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
