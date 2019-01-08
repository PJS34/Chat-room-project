using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [Serializable]
    public class Private_Message : Message
    {
        private string NameDest;
        public Private_Message(string msg, Profile p, string UserDest) : base(msg, p)
        {
            this.NameDest = UserDest;
        }

        public string NameDest1 { get => NameDest; set => NameDest = value; }
    }
}
