using System;
using System.IO;

namespace CurlyEngine.Server
{
	
	class ServerCLI
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello Curlyworld!");
			Console.ReadLine ();
		}
	}
	class ServerClass
	{
		public static int Port=27001;//default port for discovery

		public ServerClass(DirectoryInfo basedir,int port=27001)
		{

		}
	}
}
