using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Communication;
namespace Communication
{
    public class Net
    {
        
        
       
        public static void SendIdentity(Stream s, Profile Profile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, Profile);
        }
        public static Profile RcvIdentity(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (Profile)bf.Deserialize(s);
        }
        public static void SendMsg(Stream s, Message msg)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, msg);
        }


        public static Message rcvMsg(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (Message)bf.Deserialize(s);
        }


    }
}
