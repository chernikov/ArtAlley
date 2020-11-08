using ArtAlley.Data.Repositories;
using ArtAlley.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Controllers
{
    [Route("{url}")]
    public class PageController : Controller
    {
        private readonly ITopicRepository topicRepository;
        private readonly IMapper mapper;

        public PageController(ITopicRepository topicRepository, IMapper mapper)
        {
            this.topicRepository = topicRepository;
            this.mapper = mapper;
        }

        public IActionResult Index(string url)
        {
            var entity = topicRepository.FindByUrl(url);
            if (entity == null)
            {
                return NotFound();
            }
            var model = mapper.Map<TopicModel>(entity);
            
            return View(model);
        }
    }
}
