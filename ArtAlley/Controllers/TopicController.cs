using ArtAlley.Models;
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

        public IActionResult Index()
        {
            var list = new List<TopicModel>();
            //TODO:
            return View(list);
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
