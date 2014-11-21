using SWGProxy.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy.Utilities
{
	static public class PacketLookup
	{
		static public SWGPacket Find(byte[] data)
		{
			PacketReader reader = new PacketReader(data); reader.ReadShort();
			uint opcode = reader.ReadUInt();

			switch(opcode)
			{
				case 0x3436AEB6:
					return new LoginClusterStatus(data);
				default:
					return new GenericPacket(data);
			}
		}
	}
}
