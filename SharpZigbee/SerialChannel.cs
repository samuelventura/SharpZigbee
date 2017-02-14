using System;
using System.Threading;
using System.Text;
using System.IO.Ports;

namespace SharpZigbee
{
	public class SerialChannel : IChannel
	{
		private readonly SerialPort serialPort;
		private readonly Action<char, byte[]> monitor;
		
		public SerialChannel(SerialPort serialPort, Action<char, byte[]> monitor = null)
		{
			this.serialPort = serialPort;
			this.monitor = monitor;
		}
		
		public void Dispose()
		{
			Disposer.Dispose(serialPort);
		}
		
		public void Write(string cmd)
		{
			serialPort.DiscardInBuffer();
			if (monitor != null)
				monitor('>', ASCIIEncoding.ASCII.GetBytes(cmd));
			serialPort.Write(cmd);
			serialPort.BaseStream.Flush();
		}
		
		public string ReadLine(int millis)
		{
			var line = string.Empty;
			var sb = new StringBuilder();
			var dl = DateTime.Now + TimeSpan.FromMilliseconds(millis);
			while (dl > DateTime.Now) {
				while (serialPort.BytesToRead > 0) {
					var b = serialPort.ReadByte();
					sb.Append((char)b);
					if (b == 0x0D) {
						line = sb.ToString();
						if (monitor != null)
							monitor('<', ASCIIEncoding.ASCII.GetBytes(line));
						return line;
					}
				}
				Thread.Sleep(1);
			}
			line = sb.ToString();
			if (monitor != null)
				monitor('<', ASCIIEncoding.ASCII.GetBytes(line));
			return line;
		}
		
		public byte[] ReadAll()
		{
			var data = new byte[serialPort.BytesToRead];
			serialPort.Read(data, 0, data.Length);
			if (monitor != null && data.Length > 0)
				monitor('<', data);
			return data;
		}
		
		public void WriteAll(byte[] data)
		{
			if (monitor != null)
				monitor('>', data);
			serialPort.DiscardOutBuffer();
			serialPort.DiscardInBuffer();
			serialPort.Write(data, 0, data.Length);
			serialPort.BaseStream.Flush();
		}
	}
}
