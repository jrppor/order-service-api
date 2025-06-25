using Order.Application.Interfaces;
using Order.Application.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class CsvUploadService : ICsvUploadService
    {
        private readonly IRabbitMqPublisher _publisher;
        //private readonly ILogger<IRabbitMqPublisher> _logger;

        public CsvUploadService(IRabbitMqPublisher publisher/*, ILogger<IRabbitMqPublisher> logger*/)
        {
            _publisher = publisher;
            //_logger = logger;
        }

        public Task ProcessCsv(Stream stream)
        {
            var records = CsvParser.Parse(stream);

            //_logger.LogInformation("Publishing message for {Count}", records.Count.ToString());

            foreach (var record in records)
            {
                _publisher.Publish(record);
            }

            return Task.CompletedTask;
        }
    }
}
