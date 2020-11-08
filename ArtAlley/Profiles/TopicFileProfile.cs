using ArtAlley.Data.Entities;
using ArtAlley.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Profiles
{
    public class TopicFileProfile : Profile
    {
        public TopicFileProfile()
        {
            CreateMap<TopicFile, TopicFileModel>()
                .ForMember(p => p.Url, opt => opt.MapFrom(p => p.FilePath));
            CreateMap<TopicFileModel, TopicFile>()
                .ForMember(p => p.FilePath, opt => opt.MapFrom(p => p.Url));
        }
    }
}
