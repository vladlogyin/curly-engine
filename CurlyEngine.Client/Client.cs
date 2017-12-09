using System;
using System.Drawing;
using System.Threading;
using CurlyEngine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using SharpNoise;
using SharpNoise.Models;
using SharpNoise.Modules;
using SharpNoise.Utilities;
using QuickFont;
using QuickFont.Configuration;
using System.IO;
using AwesomeSockets.Domain.Sockets;
using AwesomeSockets.Sockets;
using AwesomeSockets.Buffers;
using CurlyEngine.Server;
using Buffer = AwesomeSockets.Buffers.Buffer;

namespace CurlyEngine.Client
{
    
	/// <summary>
	///CurlyEngine client base class.
	/// </summary>
	public class ClientBase
	{
        /// <summary>
        /// The rendering thread.
        /// </summary>
        Thread renderThread;
        /// <summary>
        /// The server connection thread.
        /// </summary>
        Thread connectionThread;
        ISocket server;
        public int Connected;
        public int ID;

        Buffer inBuf;
        Buffer outBuf;

        /// <summary>
        /// The lighting and occlusion thread.
        /// </summary>
        Thread lightingThread;
		ClientRenderer rend;
        public bool Running;
		public ClientBase()
		{
            //
            connectionThread = new Thread(new ThreadStart(delegate{
                rend.TopBarColor = Color.Brown;
                rend.TopBar = "Not Connected";
                Connected = 0;
                while (Connected != 1)
                {
                    
                    try
                    {
                        server = AweSock.TcpConnect("127.0.0.1", ServerClass.Port);
                        rend.TopBarColor = Color.Lime;
                        rend.TopBar = "Connected";
                        Connected = 1;
                    }
                    catch
                    {
                        rend.TopBarColor = Color.Red;
                        rend.TopBar = "Failed to connect!";
                        Connected = -1;
                    }
                }
            }));
            //
			rend = new ClientRenderer ();
            Running = false;
		}
		public void Start()
		{
            connectionThread.Start();
            //
			rend.Run (60);

            Running = true;
		}
        public void Stop()
        {
            connectionThread.Abort();
            Running = false;
        }
	}
    /// <summary>
    /// Client renderer.
    /// </summary>
    public class ClientRenderer : GameWindow
    {
        FontProvider fp;
        /// <summary>
        /// Local copy of the map.
        /// </summary>
        Map mp;
        /// <summary>
        /// Density of each pixel on screen
        /// real pixels/game pixel
        /// </summary>
        public int pixeldensity = 16;
        /// <summary>
        /// to be calculated on runtime
        /// </summary>
        public int Xsize;
        /// <summary>
        /// to be calculated on runtime
        /// </summary>
        public int Ysize;

        public float Xpos=0;

        public float Ypos=0;

        /// <summary>
        /// local copy of ClientInfo
        /// </summary>
        ClientInfo cinf;
        public string BottomLeft="kek";
        public Color BottomLeftColor = Color.White;
        public string TopBar="kek";
        public Color TopBarColor = Color.White;
        public string Motd="kek";
        public Color MotdColor = Color.White;
        public string TopRight="kek";
        public Color TopRightColor = Color.White;
        public string BottomRight="kek";
        public Color BottomRightColor = Color.White;
        Perlin test;
        QFont verdana;
        QFontDrawing drawing;
		/// <summary>
		/// Initializes a new instance of the <see cref="CurlyEngine.Client.ClientRenderer"/> class.
		/// </summary>
        public ClientRenderer() : base (800,600,GraphicsMode.Default,"curly-engine renderer",GameWindowFlags.Default,DisplayDevice.Default,3,2,GraphicsContextFlags.Default)
		{
            fp = new FontProvider();
            mp = new Map(1234);
            mp.GenerateChunk(0,0);
            test = new Perlin() { Frequency = 0.1, OctaveCount = 3, Seed = 1234 };
			//test= new Plane(Pla
		}
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
            #region Fonts
            drawing = new QFontDrawing();
            Console.WriteLine(fp.LoadAll() + " errors loading fonts, fonts loaded:");
            foreach (string key in fp.fontkeys)
            {
                Console.WriteLine(key);
            }
            //QFontBuilderConfiguration buildOpts = new QFontBuilderConfiguration()
            //{
            //    ShadowConfig = new QFontShadowConfiguration()
            //    {
            //        BlurRadius=5,
            //        BlurPasses=5,
            //        Type=ShadowType.Blurred
            //    },
            //    TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit,
            //    Characters = CharacterSet.General
            //};
            //verdana = new QFont("Data/Fonts/verdana.ttf",20,buildOpts);
            //QFontRenderOptions textOpts = new QFontRenderOptions()
            //{
            //    Colour = Color.Aqua,
            //    DropShadowActive = true
            //};


            #endregion
            Ysize = this.Height / pixeldensity;
            Xsize = this.Width / pixeldensity;
            GL.ClearColor (0.2f, 0.2f, 0.2f, 1f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
            GL.Viewport(0, 0, this.Width, this.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
            Ysize = this.Height / pixeldensity;
            Xsize = this.Width / pixeldensity;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
            GL.Viewport(0, 0, this.Width, this.Height);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
		}
		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{

		}
		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{

		}
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
		}
		protected override void OnRenderFrame (FrameEventArgs e)
		{
            base.OnRenderFrame(e);
            //GL.ClearColor(Color.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
            #region Text rendering
            verdana = fp.GetFont("verdana");
            //drawText(Motd, this.Width / 2, this.Height / 2 - 18, verdana, MotdColor, 38, QFontAlignment.Centre, true, Color.White);
            //drawText(TopBar, this.Width / 2, this.Height - 10, verdana, TopBarColor, 20, QFontAlignment.Centre,true,Color.White);
            //drawText(BottomLeft, 10, 32, verdana, BottomLeftColor, 16, QFontAlignment.Left, true, Color.White);
            //drawText(BottomRight, this.Width-200, 32, verdana, BottomRightColor, 16, QFontAlignment.Left, true, Color.White);

            //drawing.ProjectionMatrix = Matrix4.CreateOrthographic(this.Width, this.Height, -1, 1);
            //drawing.RefreshBuffers();
            //renderMap();
            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(1f, 1f, 1f);
            GL.Vertex2(200, 200);
            GL.Vertex2(200, 300);
            GL.Vertex2(300, 300);
            GL.End();
            //drawing.Draw();
            //drawing.DrawingPrimitives.Clear();
            #endregion
            GL.Flush();
			this.SwapBuffers ();
		}
        private void renderMap()
        {
            GL.Begin(PrimitiveType.Quads);
            for (int x = -Xsize / 2 + (int)Xpos; x < Xsize / 2 + (int)Xpos; x++)
            {
                for (int y = -Ysize / 2+(int)Ypos; y < Ysize / 2+(int)Ypos; y++)
                {
                    //Console.Write("("+x+";" +y+"),");
                    switch(mp.GetPixelToRender(x,y,3))
                    {
                        case -1:
                            {
                                GL.Color3(1f, 0f, 0f);
                            }
                            break;
                        case 1:
                            {
                                GL.Color3(0f, 1f, 0f);
                            }
                            break;
                        case 2:
                            {
                                GL.Color3(0f, 0f, 1f);
                            }
                            break;
                        default:
                            {
                                GL.Color3(1f, 1f, 1f);
                            }
                            break;
                    }
                    //GL.Color3();
                    GL.Vertex2((x - 0.5f- Xpos) * pixeldensity+ this.Width / 2, (y + 0.5f- Ypos) * pixeldensity+ this.Height / 2);
                    GL.Vertex2((x + 0.5f-Xpos) * pixeldensity+ this.Width / 2, (y + 0.5f- Ypos) * pixeldensity+ this.Height / 2);
                    GL.Vertex2((x + 0.5f-Xpos) * pixeldensity+ this.Width / 2, (y - 0.5f- Ypos) * pixeldensity+ this.Height / 2);
                    GL.Vertex2((x - 0.5f-Xpos) * pixeldensity+ this.Width / 2, (y - 0.5f- Ypos) * pixeldensity+ this.Height / 2);
                }
                //Console.WriteLine("");
            }
            GL.Color3(1f, 1f, 1f);
            GL.Vertex2(200, 200);
            GL.Vertex2(200, 300);
            GL.Vertex2(300, 300);
            GL.Vertex2(300, 200);
            GL.End();

        }
        void drawText(string text, float x, float y, QFont font, Color color, float size, QFontAlignment align = QFontAlignment.Left,bool shadow = false)
        {
            drawText(text, x, y, font,color,size,align,shadow,Color.Black);
        }
        void drawText(string text, float x, float y, QFont font, Color color, float size, QFontAlignment align, bool shadow, Color shadowcolor)
        {
            QFontDrawingPrimitive dp = new QFontDrawingPrimitive(font, new QFontRenderOptions() { Colour = color, DropShadowActive = shadow, DropShadowColour = shadowcolor });
            dp.Print(text, new Vector3(x-this.Width/2, y-this.Height/2, 0), align);
            drawing.DrawingPrimitives.Add(dp);
        }
	}
}

