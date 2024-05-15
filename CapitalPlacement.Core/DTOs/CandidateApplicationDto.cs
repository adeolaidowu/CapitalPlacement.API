using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.DTOs
{
    public class CandidateApplicationDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string CurrentResidence { get; set; }

        [Required]
        public string IdNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string AboutYourself { get; set; }

        [Required]
        public int YearOfGraduation { get; set; }
        [Required]
        public bool UkEmbassyRejection { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateMovedToUk { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required]
        public List<string> Skills { get; set; }
    }
}
