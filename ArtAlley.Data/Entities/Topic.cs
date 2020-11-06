using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Entities
{
    public class Topic
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string ImagePath { get; set; }

        public ICollection<TopicFile> TopicFiles { get; set; }
    }

}
