using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
    public class Message
    {
        public int MessageId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string MessageSubject { get; set; }
        public string MessageText { get; set; }
        public string SentimentLabel { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
