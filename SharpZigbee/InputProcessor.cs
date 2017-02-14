using System;

namespace SharpZigbee
{
	public class InputProcessor
	{
		private PacketBuilder packetBuilder;
		
		public void ProcessInput(bool escaped, IChannel channel, Action<IncomePacket> handler)
		{
			var data = channel.ReadAll();
			for (var i = 0; i < data.Length; i++) {
				var b = data[i];
				if (b == 0x7E)
					packetBuilder = new PacketBuilder(escaped);
				else if (packetBuilder != null) {
					packetBuilder.Add(b);
					if (packetBuilder.IsDone()) {
						if (packetBuilder.IsValid()) {
							var packet = packetBuilder.Data();
							handler(new IncomePacket(packet));
						}
						packetBuilder = null;
					}
				}
			}
		}
		
		public IncomePacket ProcessInput(bool escaped, IChannel channel)
		{
			IncomePacket income = null;
			ProcessInput(escaped, channel, (ip) => {
				income = ip;
			});
			return income;
		}
	}
}
