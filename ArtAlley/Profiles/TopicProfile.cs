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
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<Topic, TopicModel>();
            CreateMap<TopicModel, Topic>()
                .ForMember(p => p.TopicFiles, opt => opt.Ignore());
        }
    }
}
