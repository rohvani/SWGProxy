using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy
{
	public class Session
	{
		public byte[] xorKey;
		public byte[] zlibFlags;

		public Session(byte[] xorKey, byte[] zlibFlags)
		{
			this.xorKey = xorKey;
			this.zlibFlags = zlibFlags;
		}
	}
}
