using AutoMapper;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningDotNetCore.ParkyMapper
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            ///using reverse mapswe can map np to npdto and vice versa
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
