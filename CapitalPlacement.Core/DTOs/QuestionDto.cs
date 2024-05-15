using CapitalPlacement.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.DTOs
{
    public class QuestionDto
    {
        [Required]
        public string? Text { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public List<string>? Options { get; set; }
        public int MaxChoiceAllowed { get; set; }
        public bool EnableOtherOption { get; set; } = false;
    }
}
