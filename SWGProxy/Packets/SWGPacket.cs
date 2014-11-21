using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy.Packets
{
	public interface SWGPacket
	{
		byte[] ToArray();
	}
}
