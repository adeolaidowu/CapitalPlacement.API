using CapitalPlacement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.DTOs
{
    public class CreateQuestionDto
    {
        public string? Text { get; set; }
        public QuestionType Type { get; set; }
        public List<string>? Options { get; set; }
        public int MaxChoiceAllowed { get; set; }
        public bool EnableOtherOption { get; set; } = false;
    }
}
