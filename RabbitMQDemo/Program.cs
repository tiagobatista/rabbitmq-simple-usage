using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Declare a queue
channel.QueueDeclare(queue: "my_queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Queue created!");

//Direct Exchange: Routes messages to queues based on the exact matching of the routing key
//Fanout Exchange: Broadcasts the message to all queues attached to the exchange, ignoring the routing key
//Topic Exchange: Routes messages based on wildcard patterns in the routing key, enabling more flexible routing
//Headers Exchange: Routes messages based on headers instead of routing keys

channel.ExchangeDeclare(exchange: "my_direct_exchange",
                        type: ExchangeType.Direct);

Console.WriteLine("Direct exchange created!");


// Send a message with routing key

string message = "Hello, RabbitMQ!";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "my_direct_exchange",
                     routingKey: "my_routing_key",
                     basicProperties: null,
                     body: body);

Console.WriteLine($"Message sent: {message}");