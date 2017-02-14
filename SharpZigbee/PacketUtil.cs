using System;
using System.Text;

namespace SharpZigbee
{
	public static class PacketUtil
	{
		public static byte[] PackageLocalAT(byte frameId, string atText, byte[] paramArray, bool immediate)
		{
			if (paramArray == null)
				paramArray = new byte[0];
			var data = new byte[4 + paramArray.Length];
			data[0] = (byte)(immediate ? 0x08 : 0x09);
			data[1] = frameId;
			data[2] = (byte)atText[0];
			data[3] = (byte)atText[1];
			for (int i = 0; i < paramArray.Length; i++) {
				data[4 + i] = paramArray[i];
			}
			return data;
		}
		
		public static byte[] PackageRemoteAT(byte frameId, string atText, ushort panid, ulong address, byte[] paramArray, bool immediate)
		{
			if (paramArray == null)
				paramArray = new byte[0];
			var data = new byte[15 + paramArray.Length];
			data[0] = (byte)(0x17);
			data[1] = frameId;
			for (int i = 0; i < 8; i++) {
				data[9 - i] = (byte)(address & 0xFF);
				address = address >> 8;
			}
			data[10] = (byte)(0xFF & (panid >> 8));
			data[11] = (byte)(0xFF & (panid >> 0));
			data[12] = (byte)(immediate ? 0x02 : 0x00);
			data[13] = (byte)atText[0];
			data[14] = (byte)atText[1];
			for (int i = 0; i < paramArray.Length; i++) {
				data[15 + i] = paramArray[i];
			}
			return data;
		}
		
		public static byte[] PackageTX64(byte frameId, ulong address, byte[] payload, bool ack)
		{
			var data = new byte[11 + payload.Length];
			data[0] = (byte)(0x00);
			data[1] = frameId;
			for (int i = 0; i < 8; i++) {
				data[9 - i] = (byte)(address & 0xFF);
				address = address >> 8;
			}
			data[10] = (byte)(ack ? 0x00 : 0x01);
			for (int i = 0; i < payload.Length; i++) {
				data[11 + i] = payload[i];
			}
			return data;
		}
		
		public static byte[] PackageTX16(byte frameId, ushort address, byte[] payload, bool ack)
		{
			var data = new byte[5 + payload.Length];
			data[0] = (byte)(0x01);
			data[1] = frameId;
			for (int i = 0; i < 2; i++) {
				data[3 - i] = (byte)(address & 0xFF);
				address = (ushort)(address >> 8);
			}
			data[4] = (byte)(ack ? 0x00 : 0x01);
			for (int i = 0; i < payload.Length; i++) {
				data[5 + i] = payload[i];
			}
			return data;
		}
		
		public static byte[] PackageData(byte[] data, bool escaped)
		{
			//START(1), LENGTH(2), CHECKSUM(1)
			var unescaped = new byte[data.Length + 4];
			unescaped[0] = (byte)(0x7E); 					//start
			unescaped[1] = (byte)(0xFF & (data.Length >> 8)); 	//msb
			unescaped[2] = (byte)(0xFF & (data.Length)); 		//lsb
			var checksum = 0;
			for (int i = 0; i < data.Length; i++) {
				unescaped[3 + i] = data[i];
				checksum += data[i];
			}
			unescaped[unescaped.Length - 1] = (byte)(0xFF - (0xFF & checksum));
			if (escaped) {
				var extras = 0;
				for (int i = 1; i < unescaped.Length; i++) {
					extras += EscapeNeeded(unescaped[i]) ? 1 : 0;
				}
				var buffer = new byte[unescaped.Length + extras];
				buffer[0] = 0x7E;
				var pos = 1;
				for (int i = 1; i < unescaped.Length; i++) {
					var b = unescaped[i];
					if (EscapeNeeded(b)) {
						buffer[pos++] = 0x7D;
						buffer[pos++] = (byte)(0xFF & (b ^ 0x20));
					} else {
						buffer[pos++] = b;
					}
				}
				return buffer;				
			}
			return unescaped;
		}
		
		public static bool EscapeNeeded(byte b)
		{
			//FRAME DELIMITER, ESCAPE, XON, XOFF
			return (b == 0x7E || b == 0x7D || b == 0x11 || b == 0x13);
		}
		
		public static string StatusCmd(byte b)
		{
			switch (b) {
				case 0:
					return "OK";
				case 1:
					return "ERROR";
				case 2:
					return "Invalid Command";
				case 3:
					return "Invalid Parameter";
				case 4:
					return "No response";
				default:
					return string.Format("Status {0:X2}", b);
			}
		}
		
		public static string StatusTX(byte b)
		{
			switch (b) {
				case 0:
					return "Success";
				case 1:
					return "No ACK";
				case 2:
					return "CCA failure";
				case 3:
					return "Purged";
				default:
					return string.Format("Status {0:X2}", b);
			}
		}
		
		public static object Income(byte[] packet)
		{
			switch (packet[0]) {
				case 0x80:
					return new APIRX64(packet);
				case 0x81:
					return new APIRX16(packet);
				case 0x88:
					return new APILocalCmdResponse(packet);
				case 0x89:
					return new APITXStatus(packet);
				case 0x97:
					return new APIRemoteCmdResponse(packet);
			}
			
			throw new Exception("Invalid income packet code " + packet[0].ToString("X2"));
		}
		
		public static byte[] ToArray(ushort value)
		{
			var data = new byte[2];
			data[0] = (byte)(0xFF & (value >> 8));
			data[1] = (byte)(0xFF & (value >> 0));
			return data;
		}
		
		public static byte[] ToArray(uint value)
		{
			var data = new byte[4];
			data[0] = (byte)(0xFF & (value >> 24));
			data[1] = (byte)(0xFF & (value >> 16));
			data[2] = (byte)(0xFF & (value >> 8));
			data[3] = (byte)(0xFF & (value >> 0));
			return data;
		}
		
		public static ushort ToUInt16(byte[] data, int start)
		{
			var v = (ushort)0;
			for (int i = 0; i < 2; i++)
				v = (ushort)((v << 8) | data[i + start]);
			return v;
		}
		
		public static ulong ToUInt64(byte[] data, int start)
		{
			var v = (ulong)0;
			for (int i = 0; i < 8; i++)
				v = (v << 8) | data[i + start];
			return v;
		}
		
		public static byte[] SubArray(byte[] data, int start, int length)
		{
			var subdata = new byte[length];
			Array.Copy(data, start, subdata, 0, length);
			return subdata;
		}
		
		public static string ToHexString(byte[] ba, char sep = ' ')
		{
			var sb = new StringBuilder();
			foreach (var b in ba) {
				if (sb.Length > 0)
					sb.Append(sep);
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}
	}
}
