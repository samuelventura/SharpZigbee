using System;

namespace SharpZigbee
{
	public class APITXStatus
	{
		private readonly byte type;
		private readonly byte frameId;
		private readonly byte status;
		
		public byte Type { get { return type; } }
		public byte FrameId { get { return frameId; } }
		public byte Status { get { return status; } }
		
		public APITXStatus(byte[] packet)
		{
			if (packet == null || packet.Length < 1 || packet[0] != 0x89)
				throw new Exception("Invalid packet");
			
			this.type = packet[0];
			this.frameId = packet[1];
			this.status = packet[2];
		}
		
		public override string ToString()
		{
			var statusTX = PacketUtil.StatusTX(status);
			
			return string.Format("API TX Status {0},{1}", frameId, statusTX);
		}
	}
}