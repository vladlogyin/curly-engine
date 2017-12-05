using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;


namespace CurlyEngine.Client
{
	public static class ContentLoader
	{
		public static int LoadTexture(byte[] texdata)
		{
			int id = GL.GenTexture ();
			GL.BindTexture (TextureTarget.Texture2D,id);
			Bitmap bmp = new Bitmap (new MemoryStream (texdata));
			BitmapData data = bmp.LockBits (new Rectangle (0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D (TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,data.Width,data.Height,0,OpenTK.Graphics.OpenGL.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);
			bmp.UnlockBits (data);

			GL.TexParameter (TextureTarget.Texture2D,TextureParameterName.TextureWrapS,(int)TextureWrapMode.Clamp);
			GL.TexParameter (TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.Clamp);

			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			return id;
		}
		public static int LoadTexture(string path)
		{
			int id = GL.GenTexture ();
			GL.BindTexture (TextureTarget.Texture2D,id);
			Bitmap bmp = new Bitmap (path);
			BitmapData data = bmp.LockBits (new Rectangle (0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D (TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,data.Width,data.Height,0,OpenTK.Graphics.OpenGL.PixelFormat.Bgra,PixelType.UnsignedByte,data.Scan0);
			bmp.UnlockBits (data);

			GL.TexParameter (TextureTarget.Texture2D,TextureParameterName.TextureWrapS,(int)TextureWrapMode.Clamp);
			GL.TexParameter (TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.Clamp);

			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			return id;
		}
	}
}

