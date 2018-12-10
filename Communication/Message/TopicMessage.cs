using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class TopicMessage : Message
    {
        private string topicName;
        public TopicMessage(string msg, string name,string ToName) : base(msg, name)
        {
            this.topicName = ToName;
        }

        public string TopicName { get => topicName; set => topicName = value; }
    }
}
