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

			ProxySocket login = new ProxySocket(44453);
			ProxySocket zone = new ProxySocket(44463);

			login.destination = new IPEndPoint(IPAddress.Parse("23.235.226.194"), 44453);
			zone.destination = new IPEndPoint(IPAddress.Parse("23.235.226.194"), 44463);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Console.Read();
			//Application.Run(new MainForm());
		}
	}
}
