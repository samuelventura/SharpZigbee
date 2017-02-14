using System;

namespace SharpZigbee
{
	public class APILocalCmd : APICmd
	{
		private readonly byte frameId;
		private readonly string aTCommand;
		private readonly byte[] paramArray;
		private readonly bool immediate;
		
		public byte FrameId { get { return frameId; } }
		public string ATCommand { get { return aTCommand; } }
		public byte[] ParamArray { get { return paramArray; } }
		public bool Immediate { get { return immediate; } }
		
		public APILocalCmd(byte frameId, string atCommand, byte[] paramArray, bool immediate)
		{
			this.frameId = frameId;
			this.aTCommand = atCommand;
			this.paramArray = paramArray;
			this.immediate = immediate;
		}
		
		public byte[] Data()
		{
			return PacketUtil.PackageLocalAT(frameId, aTCommand, paramArray, immediate);
		}
		
		public override string ToString()
		{
			if (paramArray == null)
				return string.Format("API Local Command {0},{1},{2}", aTCommand, frameId, immediate ? "Immediate" : "Queue");
			else
				return string.Format("API Local Command {0},{1},[{2}],{3}", aTCommand, frameId, PacketUtil.ToHexString(paramArray), immediate ? "Immediate" : "Queue");
		}
	}
}
