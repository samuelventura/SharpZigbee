using System;

namespace SharpZigbee
{
	public class APIRX16
	{
		private readonly byte type;
		private readonly ushort address;
		private readonly byte signalStrength;
		private readonly byte options;
		private readonly byte[] payload;
		
		public byte Type { get { return type; } }
		public ushort Address { get { return address; } }
		public byte SignalStrength { get { return signalStrength; } }
		public byte Options { get { return options; } }
		public byte[] Payload { get { return payload; } }
		
		public APIRX16(byte[] packet)
		{
			if (packet == null || packet.Length < 1 || packet[0] != 0x81)
				throw new Exception("Invalid packet");
			
			this.type = packet[0];
			this.address = PacketUtil.ToUInt16(packet, 1);
			this.signalStrength = packet[3];
			this.options = packet[4];
			this.payload = PacketUtil.SubArray(packet, 5, packet.Length - 5);
		}
		
		public override string ToString()
		{
			return string.Format("API RX 16 {0:X4},{1},{2:X2},{3}", address, signalStrength, options, payload.Length);
		}
	}
}
