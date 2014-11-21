using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy.Packets
{
	class GenericPacket : SWGPacket
	{
		byte[] data;

		public GenericPacket(byte[] data)
		{
			this.data = data;
		}

		public byte[] ToArray()
		{
			return data;
		}
	}
}
