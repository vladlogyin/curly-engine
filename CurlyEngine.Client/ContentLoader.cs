
















using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using QuickFont;
using QuickFont.Configuration;

namespace CurlyEngine.Client
{
    public static class ContentLoader
    {
        public static int LoadTexture(byte[] texdata)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap(new MemoryStream(texdata));
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            return id;
        }
        public static int LoadTexture(string path)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
            Bitmap bmp = new Bitmap(path);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            return id;
        }
    }
    public class FontProvider
    {
        /// <summary>
        /// relative font paths
        /// </summary>
        public string[] paths ={
            "Data/Fonts/verdana.ttf",//works
            "Data/Fonts/lemonade.ttf",//works
        };
        public Hashtable fonttable;
        /// <summary>
        /// Keys for the fonts located in the font Hashtable
        /// </summary>
        public List<string> fontkeys;
        public FontProvider()
        {
            fonttable = new Hashtable();
            fontkeys = new List<string>();
        }
        public int LoadAll()
        {
            int errors = 0;
            foreach (string pth in paths)
            {
                try{
                    string fontname = pth.Split('/', '\\')[pth.Split('/', '\\').Length - 1].ToLower().Replace(".ttf", "").Replace(".odf", "");
                    fonttable.Add(fontname, new QFont(pth, 20, new QFontBuilderConfiguration()
                    {
                        ShadowConfig = new QFontShadowConfiguration()
                        {
                            BlurRadius = 5,
                            BlurPasses = 5,
                            Type = ShadowType.Blurred
                        },
                        TextGenerationRenderHint = TextGenerationRenderHint.ClearTypeGridFit,
                        Characters = CharacterSet.General
                    }));
                    fontkeys.Add(fontname);
                }
                catch(Exception ex){
                    Console.WriteLine(ex.ToString());
                    errors++;
                }
            }
            return errors;
        }
        public QFont GetFont(string key)
        {
            return (QFont)fonttable[key];
        }
    }
}

