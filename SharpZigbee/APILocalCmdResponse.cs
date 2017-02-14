using System;
using System.Text;

namespace SharpZigbee
{
	public class APILocalCmdResponse
	{
		private readonly byte type;
		private readonly byte frameId;
		private readonly string command;
		private readonly byte status;
		private readonly byte[] value;
		
		public byte Type { get { return type; } }
		public byte FrameId { get { return frameId; } }
		public string Command { get { return command; } }
		public byte Status { get { return status; } }
		public byte[] Value { get { return value; } }
		
		public APILocalCmdResponse(byte[] packet)
		{
			if (packet == null || packet.Length < 1 || packet[0] != 0x88)
				throw new Exception("Invalid packet");
			
			this.type = packet[0];
			this.frameId = packet[1];
			this.command = Encoding.ASCII.GetString(packet, 2, 2);
			this.status = packet[4];
			this.value = PacketUtil.SubArray(packet, 5, packet.Length - 5);
		}
		
		public override string ToString()
		{
			var statusCmd = PacketUtil.StatusCmd(status);
			
			if (value.Length > 0)
				return string.Format("API Local Response {0},{1},{2},[{3}]", command, frameId, statusCmd, PacketUtil.ToHexString(value));
			else
				return string.Format("API Local Response {0},{1},{2}", command, frameId, statusCmd);
		}
	}
}
