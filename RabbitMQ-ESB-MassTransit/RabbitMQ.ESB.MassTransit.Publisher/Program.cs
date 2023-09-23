

using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

string rabbitMQUri = "amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv";

string queueName = "mass-transit";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(
    new($"{rabbitMQUri}/{queueName}"));

Console.Write("Send to message: ");
string message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new Message()
{
    Text = message
});

Console.Read();