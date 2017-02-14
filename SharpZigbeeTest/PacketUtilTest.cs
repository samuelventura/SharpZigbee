using System;
using NUnit.Framework;
using SharpZigbee;

namespace SharpZigbeeTest
{
	[TestFixture]
	public class PacketUtilTest
	{
		[Test]
		public void PackageDataTest()
		{
			//taken from 90000982_A.pdf page 57 
			var data = new byte[] { 0x23, 0x11 };
			var unescaped = new byte[] { 0x7E, 0x00, 0x02, 0x23, 0x11, 0xCB };
			var escaped = new byte[] { 0x7E, 0x00, 0x02, 0x23, 0x7D, 0x31, 0xCB };
			Assert.AreEqual(unescaped, PacketUtil.PackageData(data, false));
			Assert.AreEqual(escaped, PacketUtil.PackageData(data, true));
		}
		
		[Test]
		public void UnescapedPacketBuilderTest()
		{
			var data = new byte[] { 0x23, 0x11 };
			var unescaped = new byte[] { 0x7E, 0x00, 0x02, 0x23, 0x11, 0xCB };
			
			var pb = new PacketBuilder(false);
			
			for(var i=1;i<unescaped.Length;i++)
			{
				pb.Add(unescaped[i]);
			}
			
			Assert.IsTrue(pb.IsDone());
			Assert.IsTrue(pb.IsValid());
			Assert.AreEqual(data, pb.Data());
		}
		
		[Test]
		public void EscapedPacketBuilderTest()
		{
			var data = new byte[] { 0x23, 0x11 };
			var escaped = new byte[] { 0x7E, 0x00, 0x02, 0x23, 0x7D, 0x31, 0xCB };
			
			var pb = new PacketBuilder(true);
			
			for(var i=1;i<escaped.Length;i++)
			{
				pb.Add(escaped[i]);
			}
			
			Assert.IsTrue(pb.IsDone());
			Assert.IsTrue(pb.IsValid());
			Assert.AreEqual(data, pb.Data());
		}
		
		[Test]
		public void LocalATTest()
		{
			var data = new byte[] { 0x08, 0x01, (byte)'A', (byte)'S', 0x06 };
			Assert.AreEqual(data, PacketUtil.PackageLocalAT(1, "AS", new byte[]{0x06}, true));
			data = new byte[] { 0x09, 0x01, (byte)'A', (byte)'S', 0x06 };
			Assert.AreEqual(data, PacketUtil.PackageLocalAT(1, "AS", new byte[]{0x06}, false));
			data = new byte[] { 0x08, 0x01, (byte)'A', (byte)'S' };
			Assert.AreEqual(data, PacketUtil.PackageLocalAT(1, "AS", new byte[]{}, true));
			data = new byte[] { 0x09, 0x01, (byte)'A', (byte)'S' };
			Assert.AreEqual(data, PacketUtil.PackageLocalAT(1, "AS", new byte[]{}, false));
		}
		
		[Test]
		public void RemoteATTest()
		{
			var data = new byte[] { 0x17, 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x01, 0x02, 0x02, (byte)'A', (byte)'S', 0x06 };
			Assert.AreEqual(data, PacketUtil.PackageRemoteAT(1, "AS", 0x0102, 0x030405060708090A, new byte[]{0x06}, true));
			data = new byte[] { 0x17, 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x01, 0x02, 0x00, (byte)'A', (byte)'S', 0x06 };
			Assert.AreEqual(data, PacketUtil.PackageRemoteAT(1, "AS", 0x0102, 0x030405060708090A, new byte[]{0x06}, false));
			data = new byte[] { 0x17, 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x01, 0x02, 0x02, (byte)'A', (byte)'S' };
			Assert.AreEqual(data, PacketUtil.PackageRemoteAT(1, "AS", 0x0102, 0x030405060708090A, new byte[]{}, true));
			data = new byte[] { 0x17, 0x01, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x01, 0x02, 0x00, (byte)'A', (byte)'S' };
			Assert.AreEqual(data, PacketUtil.PackageRemoteAT(1, "AS", 0x0102, 0x030405060708090A, new byte[]{}, false));
		}
		
		[Test]
		public void PackageTX16Test()
		{
			var data1 = new byte[] { 0x01, 0x01, 0x02, 0x03, 0x01, 0x04, 0x05};
			Assert.AreEqual(data1, PacketUtil.PackageTX16(1, 0x0203, new byte[]{0x04, 0x05}, false));
			var data2 = new byte[] { 0x01, 0x01, 0x02, 0x03, 0x00, 0x04, 0x05};
			Assert.AreEqual(data2, PacketUtil.PackageTX16(1, 0x0203, new byte[]{0x04, 0x05}, true));
		}
		
		[Test]
		public void PackageTX64Test()
		{
			var data1 = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x01, 0x04, 0x05};
			Assert.AreEqual(data1, PacketUtil.PackageTX64(1, 0x0203060708090001, new byte[]{0x04, 0x05}, false));
			var data2 = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x06, 0x07, 0x08, 0x09, 0x00, 0x01, 0x00, 0x04, 0x05};
			Assert.AreEqual(data2, PacketUtil.PackageTX64(1, 0x0203060708090001, new byte[]{0x04, 0x05}, true));
		}
		
		[Test]
		public void APIRX64Test()
		{
			var income = (APIRX64)PacketUtil.Income(new byte[]{0x80, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0xAA, 0xFF, 0x09});
			Assert.AreEqual(0x80, income.Type);
			Assert.AreEqual(0x0102030405060708, income.Address);
			Assert.AreEqual(0xAA, income.SignalStrength);
			Assert.AreEqual(0xFF, income.Options);
			Assert.AreEqual(new byte[] {0x09}, income.Payload);
		}
		
		[Test]
		public void APIRX16Test()
		{
			var income = (APIRX16)PacketUtil.Income(new byte[]{0x81, 0x01, 0x02, 0xAA, 0xFF, 0x09});
			Assert.AreEqual(0x81, income.Type);
			Assert.AreEqual(0x0102, income.Address);
			Assert.AreEqual(0xAA, income.SignalStrength);
			Assert.AreEqual(0xFF, income.Options);
			Assert.AreEqual(new byte[] {0x09}, income.Payload);
		}
		
		[Test]
		public void APITXStatusTest()
		{
			var income = (APITXStatus)PacketUtil.Income(new byte[]{0x89, 0x01, 0x02});
			Assert.AreEqual(0x89, income.Type);
			Assert.AreEqual(0x01, income.FrameId);
			Assert.AreEqual(0x02, income.Status);
		}
		
		[Test]
		public void APIRemoteCmdResponseTest()
		{
			var income = (APIRemoteCmdResponse)PacketUtil.Income(new byte[]{0x97, 0x09, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0xFA, 0xFE, 0x44, 0x4C, 0xFF, 0xAA});
			Assert.AreEqual(0x97, income.Type);
			Assert.AreEqual(0x09, income.FrameId);
			Assert.AreEqual(0x0102030405060708, income.Address64);
			Assert.AreEqual(0xFAFE, income.Address16);
			Assert.AreEqual("DL", income.Command);
			Assert.AreEqual(0xFF, income.Status);
			Assert.AreEqual(new byte[]{0xAA}, income.Value);
		}
		
		[Test]
		public void APILocalCmdResponseTest()
		{
			var income = (APILocalCmdResponse)PacketUtil.Income(new byte[]{0x88, 0x01, 0x44, 0x4C, 0xFF, 0xAA});
			Assert.AreEqual(0x88, income.Type);
			Assert.AreEqual(0x01, income.FrameId);
			Assert.AreEqual("DL", income.Command);
			Assert.AreEqual(0xFF, income.Status);
			Assert.AreEqual(new byte[]{0xAA}, income.Value);
		}
		
		[Test]
		public void MiscTest()
		{
			Assert.AreEqual(new byte[]{2,3}, PacketUtil.SubArray(new byte[]{1,2,3,4}, 1, 2));
			Assert.AreEqual(0x0203, PacketUtil.ToUInt16(new byte[]{1,2,3,4}, 1));
			Assert.AreEqual(0x0203040506070809, PacketUtil.ToUInt64(new byte[]{1,2,3,4,5,6,7,8,9,0}, 1));			
			Assert.AreEqual("01 02 03 04", PacketUtil.ToHexString(new byte[]{1,2,3,4}));
		}
	}
}
