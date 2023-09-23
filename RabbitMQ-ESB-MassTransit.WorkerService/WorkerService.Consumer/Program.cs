using MassTransit;
using WorkerService.Consumer;
using WorkerService.Consumer.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<MessageConsumer>();
            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");
                _configurator.ReceiveEndpoint("example-message-queue",e =>
                {
                    e.ConfigureConsumer<MessageConsumer>(context);
                });
            });
        });
    })
    .Build();

await host.RunAsync();
