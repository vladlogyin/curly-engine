using System;
using AwesomeSockets.Domain.Sockets;
using Buffer = AwesomeSockets.Buffers.Buffer;
namespace CurlyEngine.Server
{

    /// <summary>
    /// Class for client sockets and information.
    /// </summary>
    public class ClientSockets
    {
        /// <summary>
        /// ClientInfo containing name,id, inventoryid and other information
        /// </summary>
        ClientInfo clinf;
        /// <summary>
        /// Socket for specific client
        /// </summary>
        ISocket clientSocket;
        Buffer inBuf;
        Buffer outBuf;
        public ClientSockets()
        {
            inBuf = Buffer.New();
            outBuf = Buffer.New();
        }
    }
}
