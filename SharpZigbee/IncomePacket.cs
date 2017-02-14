using System;

namespace SharpZigbee
{
	public class IncomePacket
	{
		private readonly byte type;
		private readonly byte[] packet;
		private readonly object parsed;
		
		public byte Type { get { return type; } }
		public byte[] Packet { get { return packet; } }
		public object Parsed { get { return parsed; } }
		
		public IncomePacket(byte[] packet)
		{
			this.type = packet[0];
			this.packet = packet;
			this.parsed = PacketUtil.Income(packet);
		}
	}
}
