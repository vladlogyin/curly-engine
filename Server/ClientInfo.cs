using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CurlyEngine.Server
{
    /// <summary>
    /// CientInfo class to provide position, id,name,inventory id
    /// </summary>
    [Serializable]
    public class ClientInfo
    {
        public float X = 0;
        public float Y = 0;
        public int Layer = 0;
        public int World = 0;
        public int ID;
        public int Health = 100;
        public string Name;
        public ClientInfo(int id, string name)
        {
            this.ID = id;
            this.Name = name;
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
