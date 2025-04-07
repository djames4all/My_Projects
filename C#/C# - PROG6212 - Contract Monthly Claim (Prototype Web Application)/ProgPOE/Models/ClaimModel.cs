using System;
using System.ComponentModel.DataAnnotations;

namespace ProgPOE.Models
{
    public class ClaimModel
    {
        public int Id { get; set; }

        public string LecturerName { get; set; } // Add this property if missing

        [Required]
        public DateTime ClaimDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Hours worked must be at least 1")]
        public int HoursWorked { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Hourly rate must be greater than 0")]
        public decimal HourlyRate { get; set; }

        public decimal ClaimAmount => HoursWorked * HourlyRate;

        public string AdditionalNotes { get; set; }

        public string Status { get; set; } = "Pending"; // Default to pending
        public string FilePath { get; internal set; }
    }
}
