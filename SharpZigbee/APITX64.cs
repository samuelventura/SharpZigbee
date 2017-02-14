using System;

namespace SharpZigbee
{
	public class APITX64 : APICmd
	{
		private readonly byte frameId;
		private readonly ulong address;
		private readonly byte[] payload;
		private readonly bool ack;
		
		public byte FrameId { get { return frameId; } }
		public ulong Address { get { return address; } }
		public byte[] Payload { get { return payload; } }
		public bool Ack { get { return ack; } }
		
		public APITX64(byte frameId, ulong address, byte[] payload, bool ack)
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
			return PacketUtil.PackageTX64(frameId, address, payload, ack);
		}
		
		public override string ToString()
		{
			return string.Format("API TX64 {0},{1:X16},{2},[{3}]", frameId, address, payload.Length, PacketUtil.ToHexString(payload));
		}
	}
}
