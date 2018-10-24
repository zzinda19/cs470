using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using cs470project.Models;
using cs470project.Dtos;

namespace cs470project.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Resarch Project Mapping
            // Domain to Dto
            CreateMap<ResearchProject, ResearchProjectDto>()
                .ForMember(p => p.InsertDate, opt => opt.MapFrom(src => ((DateTime) src.InsertDate).ToShortDateString()));

            // Dto to Domain
            CreateMap<ResearchProjectDto, ResearchProject>()
                .ForMember(p => p.ProjectID, opt => opt.Ignore())
                .ForMember(p => p.InsertDate, opt => opt.Ignore());

            //Research Accession Mapping
            // Domain to Dto
            CreateMap<ResearchProjectAccession, ResearchAccessionDto>();
            // .ForMember;

            // Dto to Domain
            CreateMap<ResearchAccessionDto, ResearchProjectAccession>();
                //.ForMember()
                //.ForMember();
        }
    }
}