using System;

namespace SharpZigbee
{	
	static class Disposer
	{
		//SerialPort, Socket, TcpClient, Streams, Writers, Readers, ...
		public static void Dispose(IDisposable closeable)
		{
			try {
				if (closeable != null)
					closeable.Dispose();
			} catch (Exception) {
			}
		}
	}
		
	static class Thrower
	{
		public static void Throw(string format, params object[] args)
		{
			var message = format;
			if (args.Length > 0) {
				message = string.Format(format, args);
			}
			throw new Exception(message);
		}
		
		public static void Throw(Exception inner, string format, params object[] args)
		{
			var message = format;
			if (args.Length > 0) {
				message = string.Format(format, args);
			}
			throw new Exception(message, inner);
		}
	}
}
