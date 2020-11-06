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
        public IActionResult Index(string url)
        {
            ViewBag.Url = url;
            return View();
        }
    }
}
