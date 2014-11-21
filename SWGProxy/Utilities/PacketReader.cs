using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGProxy.Utilities
{
	public class PacketReader
	{
		private List<byte> data;
		public int Position { get; set; }

		public PacketReader(byte[] data)
		{
			this.data = data.ToList();
			this.Position = 0;
		}

		public byte ReadByte()
		{
			return data[Position++];
		}

		public short ReadShort()
		{
			byte[] buffer = new byte[2];
			data.CopyTo(Position, buffer, 0, buffer.Length);
			buffer.Reverse();
			Position += buffer.Length;
			return BitConverter.ToInt16(buffer, 0);
		}
		
		public int ReadInt()
		{
			byte[] buffer = new byte[4];
			data.CopyTo(Position, buffer, 0, buffer.Length);
			buffer.Reverse();
			Position += buffer.Length;
			return BitConverter.ToInt32(buffer, 0);
		}

		public long ReadLong()
		{
			byte[] buffer = new byte[8];
			data.CopyTo(Position, buffer, 0, buffer.Length);
			buffer.Reverse();
			Position += buffer.Length;
			return BitConverter.ToInt64(buffer, 0);
		}

		public float ReadFloat()
		{
			byte[] buffer = new byte[4];
			data.CopyTo(Position, buffer, 0, buffer.Length);
			buffer.Reverse();
			Position += buffer.Length;
			return BitConverter.ToSingle(buffer, 0);
		}

		public string ReadASCII()
		{
			short length = ReadShort();
			byte[] buffer = new byte[length];
			data.CopyTo(Position, buffer, 0, buffer.Length);
			Position += length;
			return ASCIIEncoding.ASCII.GetString( buffer);
		}

		public string ReadUnicode()
		{
			throw new Exception("ReadUnicode() is not implemented.");
			return null;
		}
		
	}
}
