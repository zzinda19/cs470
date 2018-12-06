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
            CreateMap<ResearchProjectAccession, AccessionDto>();
            CreateMap<ResearchProjectPatient, MRNDto>();
            CreateMap<ResearchProjectAccession, KeyPairDto>();
            CreateMap<ResearchProjectUser, ResearchProjectUserDto>();
            CreateMap<ResearchUser, ResearchUserDto>();

            // Dto to Domain
            CreateMap<ResearchProjectDto, ResearchProject>()
                .ForMember(p => p.ProjectID, opt => opt.Ignore())
                .ForMember(p => p.InsertDate, opt => opt.Ignore());
            CreateMap<AccessionDto, ResearchProjectAccession>();
            CreateMap<ResearchProjectUserDto, ResearchProjectUser>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.ResearchUser.UserId));
            CreateMap<ResearchUserDto, ResearchUser>();
        }
    }
}