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

			ProxySocket login = new ProxySocket(44453);
			ProxySocket zone = new ProxySocket(44463);

			login.destination = new IPEndPoint(IPAddress.Parse("173.255.138.49"), 44453);
			zone.destination = new IPEndPoint(IPAddress.Parse("50.97.160.122"), 44463);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
