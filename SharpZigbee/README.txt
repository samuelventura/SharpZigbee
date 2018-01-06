For usage sample see SharpHive in https://github.com/samuelventura/sandbox/

Quick Reference

http://www.science.smith.edu/~jcardell/Courses/EGR328/Readings/XBeeCookbook.pdf (Unofficial XBee Cookbook)
http://www.mouser.com/ds/2/111/90000982_A-230816.pdf (API Operation Pag 56)
http://www.digi.com/support/kbase/kbaseresultdetl?id=2184 (What is API Mode?)
http://www.digi.com/support/kbase/kbaseresultdetl?id=3220 (Changing Channels with XBee)
https://support.metageek.com/hc/en-us/articles/203845040-ZigBee-and-WiFi-Coexistence

Series
- Series1 supports both AT and API modes on same firmware
- Series2 requires firmware change to support either AT or API mode.

Modes
- AT or transparente mode.
- AT command mode allows reconfiguring a device attached to a local serial port.
- API packet mode allows:
	Transmiting to multiple destinations
	Receive status of transmitted packets
	Identify the sender
	Configure remote modules

Unicast works in both AT and API mode.
	Device1			Device2
	ATID = 1234		ATID = 1234
	ATMY = 1		ATMY = 2
	ATDL = 2		ATDL = 1
	ATDH = 0		ATDH = 0
	
Multicast is when DH=0 and DL=0xFFFF
	
16-bit addressing is when DH=0 and MY=DL. To disable it set MY=0xFFFF or MY=0xFFFE.
64-bit addressing uses the fixed device MAC found on SH, SL (where DH=SH & DL=SL).

Roles
- Coordinator CE=1
- End device CE=0

Peer‐to‐Peer Architecture

A peer-to-peer network can be established by configuring each module to operate as an End Device (CE = 0), 
disabling End Device Association on all modules (A1 = 0) and setting ID and CH parameters to be identical across the network.
