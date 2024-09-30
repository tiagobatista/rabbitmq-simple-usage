using System.Text;
using RabbitMQ.Client;


public static class EcommerceExample
{

    public static void Demo()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "log_queue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        channel.QueueDeclare(queue: "email_queue",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
        
        channel.ExchangeDeclare(exchange: "task_exchange",
                                type: ExchangeType.Direct);

        channel.QueueBind(queue: "log_queue",
                          exchange: "task_exchange",
                          routingKey: "log");

        channel.QueueBind(queue: "email_queue",
                          exchange: "task_exchange",
                          routingKey: "email");

        var logMessage = "Order placed - logging";
        var emailMessage = "Order placed - send email";

        var logBody = Encoding.UTF8.GetBytes(logMessage);
        var emailBody = Encoding.UTF8.GetBytes(emailMessage);

        channel.BasicPublish(exchange: "task_exchange",
                             routingKey: "log",
                             basicProperties: null,
                             body: logBody);

        channel.BasicPublish(exchange: "task_exchange",
                             routingKey: "email",
                             basicProperties: null,
                             body: emailBody);

        Console.WriteLine("Messages sent for logging and email!");
    }
}