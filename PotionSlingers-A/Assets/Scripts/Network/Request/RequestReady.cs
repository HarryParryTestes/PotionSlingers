using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestReady : NetworkRequest
{
	public RequestReady()
	{
		request_id = Constants.CMSG_READY;
	}

	public void send(int readyStatus)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(readyStatus);
	}
}
