using Order.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Parsers
{
    public static class CsvParser
    {
        public static List<EmailRecordDto> Parse(Stream stream)
        {
            var result = new List<EmailRecordDto>();
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    result.Add(new EmailRecordDto
                    {
                        Email = parts[0],
                        FullName = parts[1],
                        RegisteredAt = DateTime.Parse(parts[2])
                    });
                }
            }
            return result;
        }
    }
}
