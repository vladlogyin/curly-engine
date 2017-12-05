using System;
using System.IO;
using CurlyEngine.Client;
using CurlyEngine.Server;
namespace CurlyEngine
{
	class Test
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello Curlyworld test!");
			ClientBase game = new ClientBase();
			game.Run ();
		}
	}
}
