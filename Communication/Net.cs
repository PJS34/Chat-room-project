using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;
namespace Communication
{
    public class Net
    {
        private static Object verrousSend = new Object();
        private static Object verrouRcv = new Object();
       /*
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
        */
        public static void SendMsg(Stream s, Message msg)
        {
            while (!Monitor.TryEnter(verrousSend)) ;
            
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, msg);
            Monitor.Exit(verrousSend);
        }


        public static Message rcvMsg(Stream s)
        {
            while (!Monitor.TryEnter(verrouRcv)) ;
            BinaryFormatter bf = new BinaryFormatter();
            Monitor.Exit(verrouRcv);
            return (Message)bf.Deserialize(s);
        }


    }
}
