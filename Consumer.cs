using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Kafka
{
    internal class Consumer
    {
        public string Id { get; set; }
        public Topic Topic { get; set; }
        public Dictionary<Partition, long> Offsets { get; set; }
    }
}
