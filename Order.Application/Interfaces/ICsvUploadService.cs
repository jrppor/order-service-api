﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Interfaces
{
    public interface ICsvUploadService
    {
        Task ProcessCsv(Stream stream);
    }
}
