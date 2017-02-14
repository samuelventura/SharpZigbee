using System;

namespace SharpZigbee
{
	public class APITX16 : APICmd
	{
		private readonly byte frameId;
		private readonly ushort address;
		private readonly byte[] payload;
		private readonly bool ack;
		
		public byte FrameId { get { return frameId; } }
		public ushort Address { get { return address; } }
		public byte[] Payload { get { return payload; } }
		public bool Ack { get { return ack; } }
		
		public APITX16(byte frameId, ushort address, byte[] payload, bool ack)
		{
			if (payload.Length > 100)
				throw new Exception("Payload too large " + payload.Length);
			this.frameId = frameId;
			this.address = address;
			this.payload = payload;
			this.ack = ack;
		}
		
		public byte[] Data()
		{
			return PacketUtil.PackageTX16(frameId, address, payload, ack);
		}
		
		public override string ToString()
		{
			return string.Format("API TX16 {0},{1:X4},{2}", frameId, address, payload.Length);
		}
	}
}