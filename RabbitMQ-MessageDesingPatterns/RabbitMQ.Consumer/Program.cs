
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region Point-to-Point (P2P) Design 

//string queueName = "point-to-point";
//channel.QueueDeclare(queue:queueName,
//    durable:false,
//    exclusive:false,
//    autoDelete:false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue:queueName,
//    autoAck:false,
//    consumer:consumer);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region Publish/Subscribe (Pub/Sub) Design

//string exchangeName = "pub-sub-exchange";

//channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
//string queueName = channel.QueueDeclare().QueueName;

//channel.QueueBind(queue: queueName,
//    exchange: exchangeName,
//    routingKey: String.Empty);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue:queueName,autoAck: false,consumer: consumer);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};
#endregion

#region Work Queue Design

//string queueName = "work-queue"; 

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName,
//    autoAck: true,
//    consumer: consumer);

//channel.BasicQos(
//    prefetchCount: 1,
//    prefetchSize: 0,
//    global: false);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region Request/Response Design

string requestQueueName = "request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
    //.....
    byte[] responseMessage = Encoding.UTF8.GetBytes($"Completed process : {message}");
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        basicProperties: properties,
        body: responseMessage);
};
#endregion

Console.Read();
