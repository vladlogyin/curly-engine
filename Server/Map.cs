using System;
//using System.Runtime.CompilerServices;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SharpNoise.Modules;

namespace CurlyEngine.Server
{
    public class Map
    {
        /// <summary>
        /// The seed.
        /// </summary>
        public int Seed;
        /// <summary>
        /// The name of the map
        /// Also the name of the folder the map will be saved in
        /// </summary>
        public string Name;
        public Hashtable chunks;
        public Map(int seed)
        {
            this.Seed = seed;
            this.chunks = new Hashtable();
        }
        public bool IsChunkLoaded(int x, int y)
        {
            return chunks.Contains(x + "x" + y);
        }
        public Chunk GetChunk(int x, int y)
        {
            return (Chunk)chunks[x + "x" + y];
        }
        public void LoadChunk(Chunk ch)
        {
            chunks.Add(ch.x + "x" + ch.y, ch);
        }
        public bool LoadChunk(int x, int y)
        {
            try
            {
                chunks.Add(x+"x"+y,Chunk.GetChunk(File.ReadAllBytes("Map/" + Name + "/chunk_" + x + "x" + y+".dat")));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load chunk:\n"+ex.ToString());
                return false;
            }
        }
        public void GenerateChunk(int x,int y)
        {
            Chunk ch= new Chunk(x,y);
            ch.Regenerate(this.Seed);
            chunks.Add(x+"x"+y,ch);
        }
    }
    /// <summary>
    /// Chunk class containing a pixel grid.
    /// </summary>
    [Serializable]
    public class Chunk
    {
        /// <summary>
        /// Original seed of the chunk.
        /// </summary>
        public int Seed;
        /// <summary>
        /// The side length of a chunk
        /// </summary>
        public const int Length = 10;
        /// <summary>
        /// The number of layers a chunk has
        /// </summary>
        public const int Height = 10;
        /// <summary>
        /// The x location of the chunk.
        /// </summary>
        public int x
        {
            get { return xpos; }
        }
        private int xpos;
        /// <summary>
        /// Gets the y.
        /// </summary>
        public int y
        {
            get { return ypos; }
        }
        private int ypos;
        /// <summary>
        /// The pixel data of the chunk.
        /// layer 0 is the floor layer, while layer 1 is used for walls and player-editable pixels
        /// layer -1 is for the seafloor
        /// </summary>
        public int[,,] pixels;//x,y,layer
        /// <summary>
        /// The entity data of the chunk.
        /// </summary>
        public int[,,] entities;//x,y
        public float[,] lightmap;
        /// <summary>
        /// Litness state
        /// </summary>
        public bool lit;
        public int[,] occlusionmap;
        /// <summary>
        /// Occlusion state of the chunk,
        /// Occlusion is done per client, so server chunks lack occlusion;
        /// </summary>
        public bool occluded;
        public Chunk(int x, int y, int world=0)
        {
            this.xpos = x;
            this.ypos = y;
            pixels = new int[Length, Length, Height];
            entities = new int[Length, Length, Height];
            lightmap = new float[Length, Length];
        }

        public bool Regenerate(int seed)
        {
            this.Seed = seed;
            return Regenerate(new Perlin() { Frequency = .1, OctaveCount = 5, Seed = seed });
        }
        public bool Regenerate()
        {
            return Regenerate(new Perlin() { Frequency = .1, OctaveCount = 5, Seed = this.Seed });
        }
        public bool Regenerate(Perlin perl)
        {
            pixels = new int[Length, Length, Height];
            entities = new int[Length, Length, Height];
            lightmap = new float[Length, Length];

            for (int xp = 0; xp< Length;xp++)
            {
                for (int yp = 0; yp< Length;yp++)
                {
                    if(perl.GetValue(x*Length+xp,y*Length+yp,0)>0)
                    {
                        
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Serializes a Chunk class into a byte array
        /// </summary>
        /// <returns>A byte array representing the Chunk</returns>
        /// <param name="ch">Chunk to serialize</param>
        public static byte[] GetByteArray(Chunk ch)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, ch);
                return ms.ToArray();
            }
        }
        /// <summary>
        /// Converts a serialized Chunk back to its original state
        /// </summary>
        /// <returns>Chunk representing the byte array</returns>
        /// <param name="data">byte array containing the serialized Chunk</param>
        public static Chunk GetChunk(byte[] data)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(data, 0, data.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                return (Chunk)binForm.Deserialize(memStream);
            }
        }
    }
}
