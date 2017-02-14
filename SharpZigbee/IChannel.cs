
using System;

namespace SharpZigbee
{
	public interface IChannel : IDisposable
	{
		void Write(string cmd);
		string ReadLine(int millis);
		void WriteAll(byte[] data);
		byte[] ReadAll();
	}
}
