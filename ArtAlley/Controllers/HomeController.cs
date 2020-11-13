using ArtAlley.Data.Entities;
using ArtAlley.Data.Repositories;
using ArtAlley.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITopicRepository topicRepository;
        private readonly IMapper mapper;

        public HomeController(ITopicRepository topicRepository, IMapper mapper)
        {
            this.topicRepository = topicRepository;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var entity = topicRepository.Get();
            if (entity == null)
            {
                return NotFound();
            }
            var model = mapper.Map<IEnumerable<Topic>, List<TopicModel>>(entity);
            return View(model);
        }
    }
}
