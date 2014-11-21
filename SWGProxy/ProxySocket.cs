using SWGProxy.Packets;
using SWGProxy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SWGProxy
{
	public class ProxySocket
	{
		public IPEndPoint destination;
		public IPEndPoint origin;

		private Socket udpSock;
		private byte[] buffer;

		public ProxySocket(int port)
		{
			//Setup the socket and message buffer
			udpSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			udpSock.Bind(new IPEndPoint(IPAddress.Any, port));
			buffer = new byte[1024];

			//Start listening for a new message.
			EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
			udpSock.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, udpSock);
		}

		private void DoReceiveFrom(IAsyncResult iar)
		{
			//Get the received message.
			Socket recvSock = (Socket)iar.AsyncState;
			EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);

			int msgLen = recvSock.EndReceiveFrom(iar, ref clientEP);
			byte[] localMsg = new byte[msgLen];
			Array.Copy(buffer, localMsg, msgLen);

			//Start listening for a new message.
			EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
			udpSock.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, udpSock);

			// Found our client
			if (origin == null || (origin.Port != ((IPEndPoint)clientEP).Port) && ((IPEndPoint)clientEP).Address.Equals(origin.Address))
			{
				if (origin != null) Console.WriteLine("[Info] Found new client, did the old one get disconnected?");
				else Console.WriteLine("[Info] Found new client");
				origin = (IPEndPoint)clientEP;
			}

			// Message is C -> S
			if(((IPEndPoint)clientEP).Address.Equals(origin.Address))
			{
				udpSock.SendTo(localMsg, destination);
			}
			// Message is S -> C
			else 
			{
				switch(localMsg[1]) 
				{
					case 2: // SOE_SESSION_REPLY
						byte[] crcSeed = new byte[4];
						Array.Copy(localMsg, 6, crcSeed, 0, 4);
						Program.session = new Session(crcSeed);
						break;

					case 9: // SOE_CHL_DATA_A
						SOEPacket packet = new SOEPacket(localMsg);
						Console.WriteLine("[Debug] Packet " + (localMsg.SequenceEqual(packet.ToArray()) ? "succesfully" : "unsuccessfully") + " rebuilt");
						localMsg = packet.ToArray();
						break;

					default:
						Console.WriteLine("[Debug] Unknown SOE opcode: {0}", localMsg[1]);
						break;
				}
				udpSock.SendTo(localMsg, origin);
			}
		}
	}
}