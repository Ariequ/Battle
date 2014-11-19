using System;
using Communication;

internal class DefaultRequestPacket : IRequestPacket
{
	private int messageID;

	private int orderID;

	private byte[] data;

	public DefaultRequestPacket (int messageID, int orderID, byte[] data)
	{
		this.messageID = messageID;
		this.orderID = orderID;
		this.data = data;
	}

	public int GetMessageID ()
	{
		return this.messageID;
	}
	
	public int GetOrderID ()
	{
		return this.orderID;
	}

	public byte[] GetData ()
	{
		return this.data;
	}
}