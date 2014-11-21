using ComponentAce.Compression.Libs.zlib;
using SWGProxy;
using SWGProxy.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SWGProxy.Utilities
{
	public class PacketStream
	{
		public bool bigEndian = false;
		private bool xorModified = false;

		public List<byte> data;
		
		public PacketStream()
		{
			
		}

		public PacketStream(byte[] buffer)
		{
			this.data = buffer.ToList();
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

		// [Todo] Test
		public short getSOEOpcode()
		{
			return BitConverter.ToInt16(new byte[] { data[0], data[1] }, 0);
		}

		// [Todo] Test
		public short getSequence()
		{
			if (!xorModified) xorData();
			return BitConverter.ToInt16(new byte[] { data[2], data[3] }, 0);
		}

		// [Todo] Test
		public uint getSWGOpcode()
		{
			if (!xorModified) xorData();
			return BitConverter.ToUInt32(new byte[] { data[9], data[10], data[11], data[12] }, 0);
		}

		// [Done]
		public byte[] getFinalizedPacket()
		{
			if (xorModified)
			{
				WriteByte(0);
				xorData();
				// Don't touch below line or you will DIE!!
				writeByteArray(BitConverter.GetBytes( (short)(MessageCRC.GenerateCrc(data.ToArray(), MessageCRC.ParseNetByteInt(Program.session.crcSeed)) << 16 >> 16)).Reverse().ToArray());
			}

			return data.ToArray();
		}

		// [Todo] Test
		public byte[] getSWGPacket()
		{
			if (!xorModified) xorData();

			byte[] temp = new byte[data.Count - 6 - 3]; // Size = (SOEWrapper(9) - Footer(3))
			for(int i = 0; i < temp.Length; i++)
			{
				temp[i] = data[6 + i]; // Skip SOE Wrapper
			}
			return temp;
		}

		public void AnalyzePacket()
		{
			if (!xorModified) xorData();
			byte[] temp = getSWGPacket();
			int pos = 0;
			int packets = 0;

			while(pos < temp.Length)
			{
				byte length = temp[pos++]; // read length and move onto operand
				if (length == 0) break;
				pos += 2; // skip operand
				UInt32 opcode = BitConverter.ToUInt32(new byte[] { temp[pos], temp[pos + 1], temp[pos + 2], temp[pos + 3]}, 0);
				Console.WriteLine("[PacketAnalyzer] Packet found: {0:X}",opcode);
				pos += 4; // skip opcode

				if (string.Format("{0:X}", opcode) == "3436AEB6")
				{
					Console.WriteLine("test");
					byte[] testBuffer = new byte[length];
					for (int i = 0; i < length; i++) testBuffer[i] = temp[pos - 4 + i]; 
					LoginClusterStatus test = new LoginClusterStatus(testBuffer);
				}
				for(int i = 0; i + 6 < length; i++)
				{
					pos++;
				}
				packets++;
			}
			Console.WriteLine("[PacketAnalyzer] {0} packets found", packets);
		}

		// [Done]
		public void xorData()
		{
			List<byte> newData = new List<byte>();
			uint mask = BitConverter.ToUInt32(Program.session.crcSeed, 0);

			// If we haven't decrypted packet already, we should ommit the footer
			int blockCount = xorModified ? (data.Count - 2) / 4 : (data.Count - 5) / 4;
			int remainingBytes = xorModified ? (data.Count - 2) % 4 : (data.Count - 5) % 4;

			newData.Add(data[0]);
			newData.Add(data[1]);

			for (int i = 0; i < blockCount; i++)
			{
				byte[] temp = new byte[4];
				for (int b = 0; b < 4; b++)
				{
					temp[b] = data[(i * 4) + b + 2];
				}
				uint a = BitConverter.ToUInt32(temp, 0) ^ mask;
				mask = xorModified ? a : BitConverter.ToUInt32(temp, 0);
				newData.AddRange(BitConverter.GetBytes(a));
			}
			for (int i = 0; i < remainingBytes; i++ )
			{
				newData.Add((byte)(data[i + 2 + (4 * blockCount)] ^ BitConverter.GetBytes(mask)[0]));
			}

			xorModified = !xorModified;
			data = newData.ToList();
		}

		// Do we really need these? SWGEmu doesn't seem to compress...
		public void Compress()
		{
			byte[] numArray = new byte[this.data.Count];
			this.data.CopyTo(0, numArray, 0, this.data.Count);
			byte[] numArray1 = new byte[800];
			ZStream zStream = new ZStream()
			{
				avail_in = 0
			};
			zStream.deflateInit(6);
			zStream.next_in = numArray;
			zStream.next_in_index = 2;
			zStream.avail_in = (int)numArray.Length - 4;
			zStream.next_out = numArray1;
			zStream.avail_out = 800;
			if (zStream.deflate(4) != -3)
			{
				long totalOut = zStream.total_out;
				zStream.deflateEnd();
				zStream = null;
				this.data.Clear();
				this.data.Add(numArray[0]);
				this.data.Add(numArray[1]);
				for (int i = 0; (long)i < totalOut; i++)
				{
					this.data.Add(numArray1[i]);
				}
				this.data.Add(numArray[(int)numArray.Length - 3]);
				this.data.Add(numArray[(int)numArray.Length - 2]);
				this.data.Add(numArray[(int)numArray.Length - 1]);
			}
		}

		public void Decompress()
		{
			byte[] numArray = new byte[this.data.Count];
			this.data.CopyTo(0, numArray, 0, this.data.Count);
			byte[] numArray1 = new byte[800];
			ZStream zStream = new ZStream()
			{
				avail_in = 0
			};
			zStream.inflateInit();
			zStream.next_in = numArray;
			zStream.next_in_index = 2;
			zStream.avail_in = (int)numArray.Length - 4;
			zStream.next_out = numArray1;
			zStream.avail_out = 800;
			if (zStream.inflate(4) != -3)
			{
				long totalOut = zStream.total_out;
				zStream.inflateEnd();
				zStream = null;
				this.data.Clear();
				this.data.Add(numArray[0]);
				this.data.Add(numArray[1]);
				for (int i = 0; (long)i < totalOut; i++)
				{
					this.data.Add(numArray1[i]);
				}
				this.data.Add(numArray[(int)numArray.Length - 3]);
				this.data.Add(numArray[(int)numArray.Length - 2]);
				this.data.Add(numArray[(int)numArray.Length - 1]);
			}
		}
	}
}
