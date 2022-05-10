using UnityEngine;

using System;
using System.Collections.Generic;

public class NetworkRequestTable
{

	public static Dictionary<short, Type> requestTable { get; set; }

	public static void init()
	{
		requestTable = new Dictionary<short, Type>();
		add(Constants.CMSG_JOIN, "RequestJoin");
		add(Constants.CMSG_LEAVE, "RequestLeave");
		add(Constants.CMSG_SETNAME, "RequestSetName");
		add(Constants.CMSG_READY, "RequestReady");
		//add(Constants.CMSG_MOVE, "RequestMove");
		add(Constants.CMSG_INTERACT, "RequestInteract");
		add(Constants.CMSG_CHARACTER, "RequestCharacter");
        add(Constants.CMSG_P_THROW, "RequestPotionThrow");
        add(Constants.CMSG_END_TURN, "RequestEndTurn");
        add(Constants.CMSG_BUY, "RequestBuy");
        add(Constants.CMSG_SELL, "RequestSell");
        add(Constants.CMSG_CYCLE, "RequestCycle");
        add(Constants.CMSG_TRASH, "RequestTrash");
        add(Constants.CMSG_LOAD, "RequestLoad");
        Debug.Log("Inited");
	}

	public static void add(short request_id, string name)
	{
		requestTable.Add(request_id, Type.GetType(name));
	}

	public static NetworkRequest get(short request_id)
	{
		NetworkRequest request = null;

		if (requestTable.ContainsKey(request_id))
		{
			request = (NetworkRequest)Activator.CreateInstance(requestTable[request_id]);
			request.request_id = request_id;
		}
		else
		{
			Debug.Log("Request [" + request_id + "] Not Found");
		}

		return request;
	}
}
