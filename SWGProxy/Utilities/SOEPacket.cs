using SWGProxy.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy.Utilities
{
	public class SOEPacket
	{
		private List<SWGPacket> childPackets = new List<SWGPacket>();
		private bool xorModified = false;

		public SOEPacket(byte[] data)
		{
			// ? decompress packet
			// decrypt packet
			// parse SWG packets and add to childPackets list
		}

		#region Packet Utility Methods
		public byte[] ToArray()
		{
			List<byte> soeBuffer = new List<byte>();
			// add SOE header
			List<byte> swgPacketBuffer = new List<byte>();
			foreach (SWGPacket packet in childPackets)
			{
				swgPacketBuffer.Add((byte)packet.ToArray().Length);
				swgPacketBuffer.AddRange(packet.ToArray());
			}
			// encrypt swgpacket buffer
			soeBuffer.AddRange(swgPacketBuffer);
			// add SOE footer
			// ? compress packet
			return soeBuffer.ToArray();
		}
		#endregion
	}
}