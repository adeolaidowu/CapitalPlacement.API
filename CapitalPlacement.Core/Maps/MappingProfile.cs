using AutoMapper;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.Maps
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Question, CreateQuestionDto>().ReverseMap();
            CreateMap<Question, GetQuestionDto>().ReverseMap();
            CreateMap<CandidateApplicationDto, CandidateApplication>().ReverseMap();
        }
    }
}
