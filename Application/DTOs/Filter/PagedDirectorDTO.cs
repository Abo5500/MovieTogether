﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Filter
{
    public class PagedDirectorDTO : PagedBase
    {
        public List<DirectorDTO> Directors { get; set; } = new();
    }
}
