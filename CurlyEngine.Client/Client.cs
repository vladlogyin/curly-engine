using System;
using System.Drawing;
using CurlyEngine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using LibNoise;
using LibNoise.Model;


namespace CurlyEngine.Client
{
	/// <summary>
	///CurlyEngine client base class.
	/// </summary>
	public class ClientBase
	{
		ClientRenderer rend;
		public ClientBase()
		{
			rend = new ClientRenderer ();
		}
		public void Run()
		{
			rend.Run (60);
		}
	}
	/// <summary>
	/// Client renderer.
	/// </summary>
	public class ClientRenderer : GameWindow
	{
		Plane test;
		/// <summary>
		/// Initializes a new instance of the <see cref="CurlyEngine.Client.ClientRenderer"/> class.
		/// </summary>
		public ClientRenderer() : base (800,600)
		{
			//test = new Perlin (){ Seed=1234,OctaveCount = 4,Frequency = 0.1};
			test= new Plane(Pla
		}
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			GL.ClearColor (Color.Black);
		}
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			GL.Viewport (0,0,this.Width,this.Height);
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
			base.OnRenderFrame (e);
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.AccumBufferBit);
			GL.Begin (PrimitiveType.Points);
			for(int x=0;x<this.Width/10;x++)
			{
				for(int y=0;y<this.Height/10;y++)
				{
					//if(test.GetValue (x,y,0)>0.5)
					//{
						GL.Vertex2 (x,y);
					//}
				}
			}
			GL.End();
			this.SwapBuffers ();
		}
	}
}

