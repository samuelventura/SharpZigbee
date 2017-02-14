using System;

namespace SharpZigbee
{
	public class PacketBuilder
	{
		private bool escaped;
		private bool xored;
		private int length;
		private int headerIndex;
		private int dataIndex;
		private int checksum;
		private bool done;
		private bool valid;
		private byte[] data;
		
		public PacketBuilder(bool escaped)
		{
			this.escaped = escaped;
		}
		
		public void Add(byte b)
		{
			if (done)
				throw new Exception("Package data overflow");

			if (escaped) {
				if (b == 0x7D) {
					xored = true;
					return;
				} else if (xored) {
					b = (byte)(b ^ 0x20);
					xored = false;
				}
			}
			
			if (data == null) {
				switch (headerIndex) {
					case 0: 
						length += (b << 8);
						headerIndex++;
						break;
					case 1: 
						length += (b);
						headerIndex++;
						data = new byte[length];
						break;
				}
			} else if (dataIndex < data.Length) {
				data[dataIndex] = b;
				checksum += b;
				dataIndex++;
			} else {
				done = true;
				checksum += b;
				valid = (0xFF == (checksum & 0xFF));
			}
		}
		
		public bool IsDone()
		{
			return done;
		}
		
		public bool IsValid()
		{
			return valid;
		}
		
		public byte[] Data()
		{
			return data;
		}

	}
}
