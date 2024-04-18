using Console.Kafka;

internal class Program
{
    private static void Main(string[] args)
    {
        System.Console.WriteLine("Hello, Kafka!");

        Topic topic = new Topic
        {
            Name = "topic1",
            Partitions = new List<Partition>
            {
                new Partition
                {
                    Id = 1,
                    FileStream = new FileStream("partition1.dat", FileMode.OpenOrCreate)
                }
            }
        };

        System.Console.WriteLine("Created Kafka Topic");

        // Create two consumers
        Consumer consumer1 = new Consumer { Id = "consumer1", Topic = topic, Offsets = new Dictionary<Partition, long>() };
        Consumer consumer2 = new Consumer { Id = "consumer2", Topic = topic, Offsets = new Dictionary<Partition, long>() };

        // Initialize offsets
        foreach (Partition partition in topic.Partitions)
        {
            consumer1.Offsets[partition] = 0;
            consumer2.Offsets[partition] = 0;
        }

        System.Console.WriteLine("Created Kafka Consumers");

        // Write random data to the partition in a loop
        using (FileStream fs = new FileStream("partition1.dat", FileMode.Append))
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
            byte[] data = new byte[1024];
            Random rng = new Random();
            while (true)
            {
                rng.NextBytes(data);
                writer.Write(data);
                Thread.Sleep(1000); // Sleep for a second between writes
            }
        }

    }

    static void Consume(Consumer consumer)
    {
        foreach (Partition partition in consumer.Topic.Partitions)
        {
            while (true)
            {
                byte[] data = ReadFromPartition(consumer, partition, 1024);
                if (data != null)
                {
                    System.Console.WriteLine($"{consumer.Id} read {data.Length} bytes from partition {partition.Id}");
                }
            }
        }
    }

    static byte[] ReadFromPartition(Consumer consumer, Partition partition, int length)
    {
        long offset = consumer.Offsets[partition];
        if (offset >= partition.FileStream.Length)
        {
            // Nothing to read at the current offset, wait for a second
            Thread.Sleep(1000);
            return null;
        }
        else
        {
            byte[] buffer = new byte[length];
            partition.FileStream.Seek(offset, SeekOrigin.Begin);
            partition.FileStream.Read(buffer, 0, length);
            consumer.Offsets[partition] += length;
            return buffer;
        }
    }
}