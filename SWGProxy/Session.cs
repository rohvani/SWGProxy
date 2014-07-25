using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy
{
	public class Session
	{
		public byte[] crcSeed;

		public Session(byte[] crcSeed)
		{
			this.crcSeed = crcSeed;
		}
	}
}
