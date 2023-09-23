using MassTransit;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Publisher.Services
{
    public class PublishMessageService : BackgroundService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublishMessageService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (true)
            {
                Message message = new()
                {
                    Text = $"{i++}. Message"
                };
                await _publishEndpoint.Publish(message);
            }
        }
    }
}
