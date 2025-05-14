// See https://aka.ms/new-console-template for more information
using Kafka.Producer;

Console.WriteLine("Kafka Producer");

var kafkaService = new KafkaService();

await kafkaService.CreateTopicAsync();