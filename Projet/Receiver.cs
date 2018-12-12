
using Communication;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Projet
{
    [Serializable]
    public class Receiver
    {
        private TcpClient comm;

        public Receiver(TcpClient s)
        {
            comm = s;
        }

        public TcpClient Comm { get => comm; }

        public void doOperation()
        {


            Console.WriteLine("A new user joined the room");
            while (true)
            {

                Message msg = Net.rcvMsg(comm.GetStream());
                //Renvoyer a tlm avec la liste dans server
                // Console.WriteLine(msg);
                // Console.WriteLine(msg.Name + " says : " + msg.Msg);
                //Use message polymorphism
                if (msg.GetType().Equals(typeof(TopicMessage)))
                {
                   
                    TopicMessage msgTopic = (TopicMessage)msg;
                    Server.checkUserInTopic(msgTopic,this);
                    //ListTopics[msgTopic.TopicName]
                    Server.BroadcastByTopic(msgTopic);
                }
                else if (msg.GetType().Equals(typeof(AuthMessage)))
                {
                    AuthMessage auth = (AuthMessage)msg;
                    if (auth.Msg.Equals("Login"))
                    {
                        if (Server.Identification(auth))
                        {
                            auth.Success = true;
                        }else
                        {
                            auth.Success = false;
                        }
                    
                        Net.SendMsg(comm.GetStream(), auth);
                    }
                    else if (auth.Msg.Equals("Register"))
                    {
                        if (Server.ProfileRegister(auth))
                        {
                            auth.Success = true;
                        }
                        else
                        {
                            auth.Success = false;
                        }

                        Net.SendMsg(comm.GetStream(), auth);
                    }
                    
                }

                //Server.BroadcastTheMessage(msg);


            }

        }

        public override bool Equals(object obj)
        {
            var receiver = obj as Receiver;
            return receiver != null &&
                   EqualityComparer<TcpClient>.Default.Equals(comm, receiver.comm) &&
                   EqualityComparer<TcpClient>.Default.Equals(Comm, receiver.Comm);
        }

        public override int GetHashCode()
        {
            var hashCode = -868753382;
            hashCode = hashCode * -1521134295 + EqualityComparer<TcpClient>.Default.GetHashCode(comm);
            hashCode = hashCode * -1521134295 + EqualityComparer<TcpClient>.Default.GetHashCode(Comm);
            return hashCode;
        }
    }
}