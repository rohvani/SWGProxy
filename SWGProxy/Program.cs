using SWGProxy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWGProxy
{
	static class Program
	{
		public static Session session;

		[STAThread]
		static void Main()
		{
			Console.Title = "SWGProxy";

			Program.session = new Session(new byte[] { 0x1d, 0x4e, 0x32, 0x87 });
			SOEPacket test = new SOEPacket();
			Console.WriteLine(BitConverter.ToString(test.ToArray()).Replace("-", " "));

			ProxySocket login = new ProxySocket(44453);
			ProxySocket zone = new ProxySocket(44463);

			login.destination = new IPEndPoint(IPAddress.Parse("23.235.226.194"), 44453);
			zone.destination = new IPEndPoint(IPAddress.Parse("23.235.226.194"), 44463);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
