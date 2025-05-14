// Program.cs
using Kafka.Producer;                                     // KafkaService sınıfını kullanmak için namespace’i ekliyoruz.

Console.WriteLine("Kafka Producer");                    // Uygulama başladığında konsola bu mesajı basıyoruz.

var kafkaService = new KafkaService();                  // Kafka işlemlerini yapacağımız servisi örnekliyoruz.

var topicName = "use-case-1-topic";                     // Oluşturmak ve mesaj göndermek istediğimiz topic adını tutan değişken.

await kafkaService.CreateTopicAsync(topicName);         // Belirttiğimiz isimde bir topic oluşturma metodunu çağırıyoruz.

await kafkaService.SendSimpleMessageWithNullKeyAsync(topicName);
// Aynı topic’e null key kullanarak basit mesajlar gönderme metodunu çağırıyoruz.

Console.WriteLine("Mesajlar gönderilmiştir.");         // Tüm işlemler tamamlandığında bilgi mesajı basıyoruz.
