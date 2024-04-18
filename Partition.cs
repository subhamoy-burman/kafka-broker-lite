using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Kafka
{
    internal class Partition
    {
        public int Id { get; set; }
        public FileStream FileStream { get; set; }
    }
}
