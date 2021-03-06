﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAlley.Data.Entities
{
    public class TopicFile
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public Topic Topic { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string FilePath { get; set; }

        public int CountOfPlaying { get; set; }
    }
}
