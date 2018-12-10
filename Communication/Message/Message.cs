using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Communication
{
  
    [Serializable]
    public class Message
    {

        private string msg;
        private string name;

        public Message(string msg, string name)
        {
            this.msg = msg;
            this.name = name;
        }

        public string Msg { get => msg; set => msg = value; }
        public string Name { get => name; set => name = value; }
        public override string ToString()
        {
            return msg + "    " + name;
        }
    }
}
