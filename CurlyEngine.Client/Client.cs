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

namespace CurlyEngine.Client
{
    /// <summary>
    /// CientInfo class to provide position, id,name,inventory id
    /// </summary>
    public class ClientInfo
    {
        public ClientInfo(int id, string name)
        {
            
        }
        public static int GetClientID()
        {
            return 0; 
        }
    }
	/// <summary>
	///CurlyEngine client base class.
	/// </summary>
	public class ClientBase
	{
		ClientRenderer rend;
        public bool Running;
		public ClientBase()
		{
			rend = new ClientRenderer ();
            Running = false;
		}
		public void Start()
		{
			rend.Run (60);
            Running = true;
		}
        public void Stop()
        {
            Running = false;
        }
	}
    /// <summary>
    /// Client renderer.
    /// </summary>
    public class ClientRenderer : GameWindow
    {
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
            verdana = new QFont(new GDIFont("Data/Fonts/" +
                                            "verdana.ttf",30,FontStyle.Regular), new QFontBuilderConfiguration(true));
            drawing = new QFontDrawing();

            QFontRenderOptions textOpts = new QFontRenderOptions()
            {
                Colour = Color.Aqua,
                DropShadowActive = true
            };
            SizeF size = drawing.Print(verdana, "Hello curly-engine", new Vector3(100, 100, 0), QFontAlignment.Left,textOpts);
            drawing.RefreshBuffers();
            #endregion
            GL.ClearColor (Color.Black);
            //GL.MatrixMode(MatrixMode.Modelview);
            GL.Viewport(0, 0, this.Width, this.Height);
            GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			GL.Viewport (0,0,this.Width,this.Height);
            //GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
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
            GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.AccumBufferBit|ClearBufferMask.DepthBufferBit);
            //GL.ClearColor(0.20f, 0.20f, 0.20f, 0.20f);
            drawing.Draw();
			this.SwapBuffers ();
		}
	}
}

