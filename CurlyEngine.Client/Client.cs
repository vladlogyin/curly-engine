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
using CurlyEngine.Server;

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
            
            test = new Perlin() { Frequency = 0.1, OctaveCount = 3, Seed = 1234 };
			//test= new Plane(Pla
		}
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
            #region Fonts
            drawing = new QFontDrawing();

            QFontBuilderConfiguration buildOpts = new QFontBuilderConfiguration()
            {
                ShadowConfig = new QFontShadowConfiguration()
                {
                    BlurRadius=5,
                    BlurPasses=5,
                    Type=ShadowType.Blurred
                },
                TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit,
                Characters = CharacterSet.General
            };
            verdana = new QFont("Data/Fonts/verdana.ttf",20,buildOpts);
            QFontRenderOptions textOpts = new QFontRenderOptions()
            {
                Colour = Color.Aqua,
                DropShadowActive = true
            };


            #endregion
            GL.ClearColor (0.2f, 0.2f, 0.2f, 1f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
            GL.Viewport(0, 0, this.Width, this.Height);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
            GL.Viewport(0, 0, this.Width, this.Height);
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
            GL.Clear (ClearBufferMask.ColorBufferBit);

            #region Text rendering
            drawText(Motd, this.Width / 2, this.Height / 2 - 18, verdana, MotdColor, 38, QFontAlignment.Centre, true, Color.White);
            drawText(TopBar, this.Width / 2, this.Height - 10, verdana, TopBarColor, 20, QFontAlignment.Centre,true,Color.White);
            drawText(BottomLeft, 10, 32, verdana, BottomLeftColor, 16, QFontAlignment.Left, true, Color.White);
            drawText(BottomRight, this.Width-200, 32, verdana, BottomRightColor, 16, QFontAlignment.Left, true, Color.White);

            drawing.ProjectionMatrix = Matrix4.CreateOrthographic(this.Width, this.Height, -1, 1);
            drawing.RefreshBuffers();

            drawing.Draw();
            drawing.DrawingPrimitives.Clear();
            #endregion
			this.SwapBuffers ();
		}
        void drawText(string text, float x, float y, QFont font, Color color, float size, QFontAlignment align = QFontAlignment.Left,bool shadow = false)
        {
            drawText(text, x, y, font,color,size,align,shadow,Color.Black);
        }
        void drawText(string text, float x, float y, QFont font, Color color, float size, QFontAlignment align, bool shadow, Color shadowcolor)
        {
            QFontDrawingPrimitive dp = new QFontDrawingPrimitive(verdana, new QFontRenderOptions() { Colour = color, DropShadowActive = shadow, DropShadowColour = shadowcolor });
            dp.Print(text, new Vector3(x-this.Width/2, y-this.Height/2, 0), align);
            drawing.DrawingPrimitives.Add(dp);
        }
	}
}

