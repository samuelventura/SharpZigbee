using System;

namespace SharpZigbee
{
	public class APIRX64
	{
		private readonly byte type;
		private readonly ulong address;
		private readonly byte signalStrength;
		private readonly byte options;
		private readonly byte[] payload;
		
		public byte Type { get { return type; } }
		public ulong Address { get { return address; } }
		public byte SignalStrength { get { return signalStrength; } }
		public byte Options { get { return options; } }
		public byte[] Payload { get { return payload; } }
		
		public APIRX64(byte[] packet)
		{
			if (packet == null || packet.Length < 1 || packet[0] != 0x80)
				throw new Exception("Invalid packet");
			
			this.type = packet[0];
			this.address = PacketUtil.ToUInt64(packet, 1);
			this.signalStrength = packet[9];
			this.options = packet[10];
			this.payload = PacketUtil.SubArray(packet, 11, packet.Length - 11);
		}
		
		public override string ToString()
		{
			return string.Format("API RX 64 {0:X16},{1},{2:X2},{3},[{4}]", address, signalStrength, options, payload.Length, PacketUtil.ToHexString(payload));
		}
	}
}
