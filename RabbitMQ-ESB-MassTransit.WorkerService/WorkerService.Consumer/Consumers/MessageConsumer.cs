using MassTransit;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Consumer.Consumers
{
    public class MessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Received message: {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
