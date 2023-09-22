
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://rxniewnv:q7meGNULORnRbaxpZqUDUhitAarKxkAC@woodpecker.rmq.cloudamqp.com/rxniewnv");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


#region Point-to-Point (P2P) Design 

//string queueName = "point-to-point";
//channel.QueueDeclare(
//    queue: queueName, 
//    durable: false,
//    exclusive: false,
//    autoDelete: true);

//byte[] message = Encoding.UTF8.GetBytes("Hellooooooo!");
//channel.BasicPublish(exchange: string.Empty,
//     routingKey: queueName,
//     body: message);

#endregion

#region Publish/Subscribe (Pub/Sub) Design

//string exchangeName = "pub-sub-exchange";
//channel.ExchangeDeclare(
//    exchange: exchangeName, 
//    type:ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Helloooo" + i);
//    channel.BasicPublish(
//        exchange: exchangeName,
//        routingKey: string.Empty,
//        body: message);
//}

#endregion

#region Work Queue Design
//string queueName = "work-queue";

//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Helloooo" + i);
//    channel.BasicPublish(
//        exchange: string.Empty,
//        routingKey: queueName,
//        body: message);
//}

#endregion

#region Request/Response Design
string requestQueueName = "request-response-queue";
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

string replyQueueName = channel.QueueDeclare().QueueName;

string correlationId = Guid.NewGuid().ToString();

#region Request Message Create and Send
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Hellooo" + i);
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: requestQueueName,
        body: message,
        basicProperties: properties);
}
#endregion
#region Response Queue Listen
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        //....
        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};
#endregion

#endregion
Console.Read();