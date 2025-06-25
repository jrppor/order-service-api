using Microsoft.Extensions.Configuration;
using Order.Application.DTOs;
using Order.Application.Interfaces;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Order.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly string _hostName;
        private readonly string _exchange;
        private readonly string _queue;
        private readonly string _routingKey;

        public RabbitMqPublisher(IConfiguration config)
        {
            _hostName = config["RabbitMQ:HostName"] ?? string.Empty;
            _exchange = config["RabbitMQ:Exchange"] ?? string.Empty;
            _queue = config["RabbitMQ:Queue"] ?? string.Empty ;
            _routingKey = config["RabbitMQ:RoutingKey"] ?? string.Empty;
        }

        public void Publish(EmailRecordDto dto)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(_exchange, ExchangeType.Direct);
            channel.QueueDeclare(_queue, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(_queue, _exchange, _routingKey);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dto));
            channel.BasicPublish(_exchange, _routingKey, null, body);
        }
    }
}
