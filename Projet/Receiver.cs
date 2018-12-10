﻿
using Communication;
using System;
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
                Server.BroadcastTheMessage(msg);


            }

        }
    }
}