using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
	public class Logger
	{
		public static void PrintPacket(byte[] data)
		{
			Console.WriteLine(BitConverter.ToString(data).Replace('-', ' '));
		}

		public static void Print(string text, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ResetColor();
		}
	}
}
