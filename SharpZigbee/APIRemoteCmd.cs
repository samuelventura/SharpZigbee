using System;

namespace SharpZigbee
{
	public class APIRemoteCmd : APICmd
	{
		private readonly byte frameId;
		private readonly string aTCommand;
		private readonly byte[] paramArray;
		private readonly bool immediate;
		private readonly ulong address64;
		private readonly ushort address16;
		
		public byte FrameId { get { return frameId; } }
		public string ATCommand { get { return aTCommand; } }
		public byte[] ParamArray { get { return paramArray; } }
		public bool Immediate { get { return immediate; } }
		public ulong Address64 { get { return address64; } }
		public ushort Address16 { get { return address16; } }
		
		public APIRemoteCmd(byte frameId, string atCommand, ushort address16, ulong address64, byte[] paramArray, bool immediate)
		{
			this.frameId = frameId;
			this.aTCommand = atCommand;
			this.address16 = address16;
			this.address64 = address64;
			this.paramArray = paramArray;
			this.immediate = immediate;
		}
		
		public byte[] Data()
		{
			return PacketUtil.PackageRemoteAT(frameId, aTCommand, address16, address64, paramArray, immediate);
		}
		
		public override string ToString()
		{
			if (paramArray == null)
				return string.Format("API Remote Command {0},{1},{2:X4},{3:X16},{4}", aTCommand, frameId, address16, address64, immediate ? "Immediate" : "Queue");
			else
				return string.Format("API Remote Command {0},Id:{1},{2:X4},{3:X16},[{4}],{5}", aTCommand, frameId, address16, address64, PacketUtil.ToHexString(paramArray), immediate ? "Immediate" : "Queue");
		}
	}
}
