using ArtAlley.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Controllers
{
    [Route("api/play")]
    public class PlayFileController : Controller
    {
        private readonly ITopicFileRepository topicFileRepository;
        private readonly IMapper mapper;

        public PlayFileController(ITopicFileRepository topicFileRepository, IMapper mapper)
        {
            this.topicFileRepository = topicFileRepository;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            topicFileRepository.Increase(id);
            return Ok();
        }
    }
}
