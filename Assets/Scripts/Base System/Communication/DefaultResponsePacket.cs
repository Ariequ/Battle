using System;
using Communication;

internal class DefaultResponsePacket : IResponsePacket
{
	private int messageID;
	
	private int orderID;
	
	private object data;
	
	public DefaultResponsePacket (int messageID, int orderID, object data)
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
	
	public object GetData ()
	{
		return this.data;
	}
}