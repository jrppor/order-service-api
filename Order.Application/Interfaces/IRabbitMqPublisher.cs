﻿using Order.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Interfaces
{
    public interface IRabbitMqPublisher
    {
        void Publish(EmailRecordDto dto);
    }
}
