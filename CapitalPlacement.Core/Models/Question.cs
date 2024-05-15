using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.Models
{
    public class Question
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public QuestionType Type { get; set; }
        public List<string>? Options { get; set; }
        public int MaxChoiceAllowed { get; set; }
        public bool EnableOtherOption { get; set; } = false;
    }

    public enum QuestionType
    {
        Paragraph,
        YesOrNo,
        Dropdown,
        MultipleChoice,
        Date,
        Number
    }
}
