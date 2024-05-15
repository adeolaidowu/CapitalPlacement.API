using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.DTOs
{
    public class GetQuestionDto
    {
        public string? id { get; set; }
        public string? Text { get; set; }
        public string Type { get; set; }
        public List<string>? Options { get; set; }
        public int MaxChoiceAllowed { get; set; }
        public bool EnableOtherOption { get; set; }
    }
}
