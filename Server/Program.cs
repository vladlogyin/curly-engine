using System;
using System.IO;
using System.Threading;
using AwesomeSockets.Domain.Sockets;
using System.Collections.Generic;
using AwesomeSockets.Sockets;
using System.Dynamic;
using System.Runtime.Serialization.Formatters.Binary;

namespace CurlyEngine.Server
{
	
	class ServerCLI
	{
        static ServerClass srv;
		public static void Main (string[] args)
        {
            srv = new ServerClass(new DirectoryInfo(Environment.CurrentDirectory));
			Console.WriteLine ("Hello Curlyserver!");
            srv.Start();
			Console.ReadLine ();
            srv.Stop();
		}
	}
	public class ServerClass
	{
        /// <summary>
        /// The default port of the server.
        /// </summary>
		public static int Port=27001;//default port for discovery
        /// <summary>
        /// The connection thread.
        /// </summary>
        Thread connectionThread;

        ISocket listensocket;
        List<ISocket> clients;


        /// <summary>
        /// Indicates whether server is running.
        /// </summary>
        public bool Running = false;
		public ServerClass(DirectoryInfo basedir,int port=27001)
		{
            listensocket = AweSock.TcpListen(Port);
            Console.WriteLine("asdf");
            clients = new List<ISocket>();
            //client = AweSock.TcpAccept(listensocket);
            Console.WriteLine("asdf1");
            connectionThread = new Thread(new ThreadStart(delegate
            {
                clients.Add(AweSock.TcpAccept(listensocket));
                Console.WriteLine("Found client " + clients[clients.Count-1].GetRemoteEndPoint().ToString());
            }));
            Console.WriteLine("asdf2");
		}
        public void Start()
        {
            connectionThread.Start();
            Running = true;
        }
        public void Stop()
        {
            connectionThread.Abort();
            clients.Clear();
            Running = false;
        }
        //public 
	}

}
