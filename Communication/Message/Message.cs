using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Communication
{
  //  public enum MessageRole {REGISTER,LOGIN,MESSAGE};
    [Serializable]
    public class Message
    {

        private string msg;
        private Profile p;
     
        public Message(string msg, Profile p)
        {
            this.msg = msg;
            this.p = p;
            
        }

        public string Msg { get => msg; set => msg = value; }
        public Profile P { get => p; set => p = value; }

        public override string ToString()
        {
            return msg + "    " + p.ToString();
        }
    }
}
