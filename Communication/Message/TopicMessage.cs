using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class TopicMessage : Message
    {
        private string topicName;
        public TopicMessage(string msg, Profile p,string ToName) : base(msg, p)
        {
            this.topicName = ToName;
        }

        public string TopicName { get => topicName; set => topicName = value; }
    }
}
