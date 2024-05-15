using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.Models
{
    public class CandidateApplication
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();        
        public string FirstName { get; set; }        
        public string LastName { get; set; }    
        
        public string Email { get; set; }       
        
        public string Phone { get; set; }        
        public string Nationality { get; set; }       
        
        public DateTime DateOfBirth { get; set; }        
        public string CurrentResidence { get; set; }        
        public string IdNumber { get; set; }        
        public string Gender { get; set; }        
        public string AboutYourself { get; set; }        
        public int YearOfGraduation { get; set; }       
        public bool UkEmbassyRejection { get; set; }      
        
        public DateTime DateMovedToUk { get; set; }        
        public int YearsOfExperience { get; set; }        
        public List<string> Skills { get; set; }
    }
}
