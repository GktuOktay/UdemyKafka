// KafkaService.cs
using Confluent.Kafka;                                  // Kafka Producer/Consumer için temel tipler.
using Confluent.Kafka.Admin;                            // Topic yönetimi (AdminClient) için ek tipler.
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Producer
{
    internal class KafkaService
    {
        internal async Task CreateTopicAsync(string topicName)
        {
            // AdminClientConfig: Kafka kümesinin adresini belirttiğimiz konfigürasyon nesnesi
            var config = new AdminClientConfig() { BootstrapServers = "localhost:9094" };

            // AdminClientBuilder ile bir yönetici istemcisi (AdminClient) oluşturuyoruz
            using var adminClient = new AdminClientBuilder(config).Build();

            try
            {
                // TopicSpecification: oluşturulacak topic’in adını, partition ve replikasyon sayısını belirtiyoruz
                var topicSpec = new TopicSpecification()
                {
                    Name = topicName,               // Oluşturulacak topic’in adı
                    NumPartitions = 3,              // Bölüm (partition) sayısı
                    ReplicationFactor = 1           // Replikasyon faktörü
                };

                // CreateTopicsAsync: Kafka’ya topic oluşturma isteği gönderiyoruz
                await adminClient.CreateTopicsAsync(new[] { topicSpec });

                Console.WriteLine($"Topic({topicName}) oluştu.");
                // Başarılı ise konsola bilgi mesajı yazdırıyoruz
            }
            catch (Exception ex)
            {
                // Hata yakalama: topic zaten varsa veya başka bir hata çıktıysa mesajı yazdırıyoruz
                Console.WriteLine(ex.Message);
            }
        }

        internal async Task SendSimpleMessageWithNullKeyAsync(string topicName)
        {
            // ProducerConfig: mesaj göndermek için gerekli Kafka sunucu ayarları
            var config = new ProducerConfig() { BootstrapServers = "localhost:9094" };

            // ProducerBuilder ile Null key, string değer tipinde bir producer oluşturuyoruz
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            // 1’den 10’a kadar döngü: her adımda bir mesaj oluşturup yollayacağız
            foreach (var item in Enumerable.Range(1, 10))
            {
                var message = new Message<Null, string>()
                {
                    Value = $"Message({topicName}) - {item}"
                    // Mesajın içeriği: topic adı ve sıra numarası
                };

                // ProduceAsync: mesajı Kafka’ya gönderiyoruz ve sonucu alıyoruz
                var result = await producer.ProduceAsync(topicName, message);

                // Sonuç nesnesinin tüm özelliklerini konsola yazdırıyoruz
                foreach (var propertyInfo in result.GetType().GetProperties())
                {
                    Console.WriteLine($"{propertyInfo.Name}: {propertyInfo.GetValue(result)}");
                }

                Console.WriteLine("-----------------------------------------------");
                await Task.Delay(200);
                // Görüntüleyebilmek için gecikme koyuyoruz.
            }
        }
    }
}
