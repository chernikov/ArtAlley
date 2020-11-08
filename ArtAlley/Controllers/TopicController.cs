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

        [HttpGet("add")]
        public IActionResult Add()
        {
            var topicModel = new TopicModel();

            return View("Edit", topicModel);
        }

        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var entity = topicRepository.FindById(id);
            if (entity == null)
            {
                return NotFound();
            }
            var result = mapper.Map<TopicModel>(entity);
            return View(result);
        }

        [HttpPost("edit")]
        [HttpPost("edit/{id:int}")]
        public IActionResult Edit(TopicModel model)
        {
            var entity = mapper.Map<Topic>(model);
            var topicFiles = mapper.Map<List<TopicFile>>(model.TopicFiles);
            Topic newEntity;
            if (model.Id == 0) {
                newEntity = topicRepository.Create(entity);
            }else
            {
                newEntity = topicRepository.Update(entity);
            }
            topicRepository.UpdateFiles(newEntity.Id, topicFiles);

            return RedirectToAction("Index");
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            topicRepository.RemoveById(id);
            return RedirectToAction("Index");
        }
    }
}
