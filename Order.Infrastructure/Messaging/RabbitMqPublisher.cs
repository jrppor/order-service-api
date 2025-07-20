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
            // สร้าง Connection ไปยัง RabbitMQ
            var factory = new ConnectionFactory { HostName = _hostName };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // ประกาศ Exchange และ Queue
            channel.ExchangeDeclare(
                exchange: _exchange,
                type: ExchangeType.Direct,
                durable: true
            );

            channel.QueueDeclare(
                queue: _queue,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            channel.QueueBind(
                queue: _queue,
                exchange: _exchange,
                routingKey: _routingKey
            );

            // แปลง DTO เป็น JSON และเป็น byte[]
            var messageJson = JsonSerializer.Serialize(dto);
            var messageBody = Encoding.UTF8.GetBytes(messageJson);

            // ส่งข้อความเข้า Exchange
            channel.BasicPublish(
                exchange: _exchange,
                routingKey: _routingKey,
                basicProperties: null,
                body: messageBody
            );

            Console.WriteLine($"📤 Published to '{_exchange}' with routingKey '{_routingKey}': {messageJson}");
        }

    }
}
