using ArtAlley.Data.Entities;
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
    [Route("/admin/topic")]
    public class TopicController : Controller
    {
        private readonly ITopicRepository topicRepository;
        private readonly IMapper mapper;

        public TopicController(ITopicRepository topicRepository, IMapper mapper)
        {
            this.topicRepository = topicRepository;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var list = topicRepository.Get();
            var result = mapper.Map<IEnumerable<Topic>, IList<TopicModel>>(list);
            return View(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost("edit")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            return View("Index");
        }


    }
}
