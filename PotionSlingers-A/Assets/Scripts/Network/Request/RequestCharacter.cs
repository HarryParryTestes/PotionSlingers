using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestCharacter : NetworkRequest
{
	public RequestCharacter()
	{
		request_id = Constants.CMSG_CHARACTER;
	}

	public void send(string name)
	{
		packet = new GamePacket(request_id);
		packet.addString(name);
	}
}
