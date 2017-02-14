using System;
using System.Text;

namespace SharpZigbee
{
	public class APIRemoteCmdResponse
	{
		private readonly byte type;
		private readonly byte frameId;
		private readonly ulong address64;
		private readonly ushort address16;
		private readonly string command;
		private readonly byte status;
		private readonly byte[] value;
		
		public byte Type { get { return type; } }
		public byte FrameId { get { return frameId; } }
		public ulong Address64 { get { return address64; } }
		public ushort Address16 { get { return address16; } }
		public string Command { get { return command; } }
		public byte Status { get { return status; } }
		public byte[] Value { get { return value; } }
		
		public APIRemoteCmdResponse(byte[] packet)
		{
			if (packet == null || packet.Length < 1 || packet[0] != 0x97)
				throw new Exception("Invalid packet");
			
			this.type = packet[0];
			this.frameId = packet[1];
			this.address64 = PacketUtil.ToUInt64(packet, 2);
			this.address16 = PacketUtil.ToUInt16(packet, 10);
			this.command = Encoding.ASCII.GetString(packet, 12, 2);
			this.status = packet[14];
			this.value = PacketUtil.SubArray(packet, 15, packet.Length - 15);
		}
		
		public override string ToString()
		{
			var statusCmd = PacketUtil.StatusCmd(status);
			
			if (value.Length > 0)
				return string.Format("API Remote Response {0},{1},{2:X4},{3:X16},{4},[{5}]", command, frameId, address16, address64, statusCmd, PacketUtil.ToHexString(value));
			else
				return string.Format("API Remote Response {0},{1},{2:X4},{3:X16},{4}", command, frameId, address16, address64, statusCmd);
		}
	}
}
