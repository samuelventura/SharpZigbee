using System;

namespace SharpZigbee
{
	public interface APICmd
	{
		byte FrameId { get; }
		byte[] Data();
	}
}
