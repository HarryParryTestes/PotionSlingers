using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMove : NetworkRequest
{
	public RequestMove()
	{
		request_id = Constants.CMSG_MOVE;
	}

	public void send(int x, int y, int z)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(x);
		packet.addInt32(y);
		packet.addInt32(z);
	}
}
