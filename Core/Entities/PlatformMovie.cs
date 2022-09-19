﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PlatformMovie
    {
        public int Id { get; set; }
        public int PlatformId { get; set; }
        public int MovieId { get; set; }
        public string Url { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
