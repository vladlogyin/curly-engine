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
    /// <summary>
    /// Class for client sockets and information.
    /// </summary>
    public class ClientSockets
    {
        ClientInfo clinf;
        public ClientSockets
    }
    /// <summary>
    /// CientInfo class to provide position, id,name,inventory id
    /// </summary>
    [Serializable]
    public class ClientInfo
    {
        public float X=0;
        public float Y=0;
        public int Layer=0;
        public int World = 0;
        public int ID;
        public int Health;
        public ClientInfo(int id, string name)
        {
            ID = id;
        }
        /// <summary>
        /// Serializes a ClientInfo class into a byte array
        /// </summary>
        /// <returns>A byte array representing the ClientInfo</returns>
        /// <param name="cinf">ClientInfo to serialize</param>
        public static byte[] GetByteArray(ClientInfo cinf)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, cinf);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Converts a serialized ClientInfo back to its original state
        /// </summary>
        /// <returns>ClientInfo representing the byte array</returns>
        /// <param name="data">byte array containing the serialized ClientInfo</param>
        public static ClientInfo GetClientInfo(byte[] data)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(data, 0, data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return (ClientInfo)binForm.Deserialize(memStream);
            }
        }
    }
}
