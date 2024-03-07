using Subscriber;

Console.WriteLine("Hello, World!");

var rabbitReader = new RabbitReader();

Console.WriteLine("Rabbit reader: ");

rabbitReader.Read();

Console.ReadLine();
