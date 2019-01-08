
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
        private TcpClient comm2;
        public Receiver(TcpClient s, TcpClient s2)
        {
            comm = s;
            comm2 = s2;
        }

        public TcpClient Comm { get => comm; }
        public TcpClient Comm2 { get => comm2; }

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
                    if (msgTopic.Msg.Equals("Creation") && !Server.CheckExistingTopic(msgTopic.TopicName))
                    {
                        Server.newTopic(msgTopic.TopicName);

                    }
                    else
                    {
                        if (msg.Msg.Equals("quit"))
                        {
                            Server.DeleteUserInTopic(msgTopic,this);
                        }
                        else
                        {
                           // Server.checkUserInTopic(msgTopic, this);
                            //ListTopics[msgTopic.TopicName]
                            Server.BroadcastByTopic(msgTopic);
                        }
                    }
                }
                else if (msg.GetType().Equals(typeof(AuthMessage)))
                {
                    AuthMessage auth = (AuthMessage)msg;
                    if (auth.Msg.Equals("Login"))
                    {
                        if (Server.Identification(auth))
                        {
                            auth.Success = true;
                            Server.newUserConnected(auth.P, this);
                        }
                        else
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
                else if (msg.GetType().Equals(typeof(Private_Message)))
                {
                    Console.WriteLine("Redirecting the private message");
                    Private_Message msgPrivate = (Private_Message)msg;
                    Server.SendPrivateMessage(msgPrivate);
                    // Net.SendMsg(comm2.GetStream(), msg);
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

        internal void doOperationInformations()
        {
            Message msg = Net.rcvMsg(comm2.GetStream());
            if (msg.GetType().Equals(typeof(TopicMessage)))
            {
                TopicMessage msgTopic = (TopicMessage)msg;
                Server.checkUserInTopic(msgTopic, this);
                
            }
               
            if (msg.GetType().Equals(typeof(RequestMessage)))
            {
                RequestMessage reqmsg = (RequestMessage)msg;
                if (reqmsg.Msg.Equals("RequireListTopic"))
                {
                    reqmsg.Msg = Server.getListTopics();
                }
                else if (reqmsg.Msg.Equals("RequireListUsers"))
                {
                    reqmsg.Msg = Server.getConnectedUsers();
                }
                Net.SendMsg(comm2.GetStream(), reqmsg);
            }
        }
    }
}