using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Controllers
{
    [Route("image")]
    public class ImageController : Controller 
    {
        private readonly string DirectoryPath = "static";

        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;

            if (files.Count <= 0)
            {
                return BadRequest();
            }
            var file = files[0];
            if (file.Length == 0)
            {
                return BadRequest();
            }

            var di = new DirectoryInfo(DirectoryPath);
            if (!di.Exists)
            {
                di.Create();
            }
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString("N") + fileExtension;
            var fullPath = Path.Combine(DirectoryPath, fileName);
            using (var inputStream = new FileStream(fullPath, FileMode.Create))
            {
                // read file to stream
                await file.CopyToAsync(inputStream);
                // stream to byte array
                byte[] array = new byte[inputStream.Length];
                inputStream.Seek(0, SeekOrigin.Begin);
                inputStream.Read(array, 0, array.Length);
            }
            var result = new { Path = $"/{DirectoryPath}/{fileName}" }; 
            return Ok(result);
     }
    }
}
