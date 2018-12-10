
using Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projet
{
    public class Server
    {
        private int port;
        // private static List<Receiver> ConnectedUsers;
        private static Dictionary<Profile, Receiver> ConnectedUsers;
        private static Dictionary<String, List<Receiver>> ListTopics;

        public static Dictionary<String, List<Receiver>> _ListTopics { get => ListTopics; set => ListTopics = value; }
        public static Dictionary<Profile, Receiver> _ConnectedUsers { get => ConnectedUsers; set => ConnectedUsers = value; }

        public Server(int port)
        {
            this.port = port;
            ConnectedUsers = new Dictionary<Profile, Receiver>();
            ListTopics = new Dictionary<String, List<Receiver>>();
            //newTopic("C#");
            //newTopic("Programming");
            //SerializeTopicList();
            DeserializeTopicList();


        }
        public void SerializeTopicList()
        {
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\AllTopics.txt", FileMode.Create, FileAccess.Write);
            formater.Serialize(stream, ListTopics);
            stream.Close();

        }
        public void DeserializeTopicList()
        {
            IFormatter formater = new BinaryFormatter();
            Stream stream = new FileStream("D:\\EFREI\\S7\\C#\\Projet\\AllTopics.txt", FileMode.Open, FileAccess.Read);
            ListTopics = (Dictionary<String, List<Receiver>>)formater.Deserialize(stream);
            foreach (String topic in ListTopics.Keys)
            {
                ListTopics[topic].Clear();
            }
            stream.Close();

        }
        public void newTopic(string name)
        {
            List<Receiver> usersByTopic = new List<Receiver>();
            ListTopics.Add(name, usersByTopic);

        }


        public static void SendRedirect(Message msg)
        {
            //Use message polymorphism
        }

        public static void BroadcastByTopic(Message msg, String topic)
        {
            if (ListTopics.ContainsKey(topic))
            {

                foreach (Receiver User in ListTopics["topic"])
                {
                    Net.SendMsg(User.Comm.GetStream(), msg);
                }
            }
            else
            {
                Console.WriteLine("Topic inexistant");
            }

        }
        public static void BroadcastTheMessage(Message msg)
        {
            foreach (Receiver User in ConnectedUsers.Values)
            {
                Net.SendMsg(User.Comm.GetStream(), msg);
            }
        }
        public void start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            while (true)
            {

                TcpClient comm = l.AcceptTcpClient();
                Profile newProfileConnected = Net.RcvIdentity(comm.GetStream());
                Console.WriteLine("Connection established with @" + newProfileConnected.Username);
                Receiver newUser = new Receiver(comm);
                ConnectedUsers.Add(newProfileConnected, newUser);

                new Thread(newUser.doOperation).Start();


            }



        }
    }
}
