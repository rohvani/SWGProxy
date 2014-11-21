using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy.Utilities
{
	public class PacketWriter
	{
		public bool bigEndian = false;
		public List<byte> data;

		public PacketWriter()
		{
			data = new List<byte>();
		}

		public void WriteByte(byte data)
		{
			this.data.Add(data);
		}

		public void writeUShort(ushort data)
		{
			byte[] buffer = BitConverter.GetBytes(data);
			if (bigEndian) buffer = buffer.Reverse().ToArray();
			for (int i = 0; i < buffer.Length; i++) WriteByte(buffer[i]);
		}

		public void writeShort(short data)
		{
			byte[] buffer = BitConverter.GetBytes(data);
			if (bigEndian) buffer = buffer.Reverse().ToArray();
			for (int i = 0; i < buffer.Length; i++) WriteByte(buffer[i]);
		}

		public void writeInt(int data)
		{
			byte[] buffer = BitConverter.GetBytes(data);
			if (bigEndian) buffer = buffer.Reverse().ToArray();
			for (int i = 0; i < buffer.Length; i++) WriteByte(buffer[i]);
		}

		public void writeUInt(uint data)
		{
			byte[] buffer = BitConverter.GetBytes(data);
			if (bigEndian) buffer = buffer.Reverse().ToArray();
			for (int i = 0; i < buffer.Length; i++) WriteByte(buffer[i]);
		}

		public void writeLong(long data)
		{
			byte[] buffer = BitConverter.GetBytes(data);
			if (bigEndian) buffer = buffer.Reverse().ToArray();
			for (int i = 0; i < buffer.Length; i++) WriteByte(buffer[i]);
		}

		public void writeByteArray(byte[] data)
		{
			for (int i = 0; i < data.Length; i++) WriteByte(data[i]);
		}

		public void writeASCII(string data)
		{
			writeShort((short) data.Length);
			writeByteArray(System.Text.Encoding.ASCII.GetBytes(data));
		}

		public void writeUnicode(string data)
		{
			writeShort((short)data.Length);
			writeByteArray(System.Text.Encoding.Unicode.GetBytes(data));
		}

		public byte[] ToArray()
		{
			return data.ToArray();
		}
	}
}
