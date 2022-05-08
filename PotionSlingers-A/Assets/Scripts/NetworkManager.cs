using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	private ConnectionManager cManager;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		gameObject.AddComponent<MessageQueue>();
		gameObject.AddComponent<ConnectionManager>();

		NetworkRequestTable.init();
		NetworkResponseTable.init();
	}

	// Start is called before the first frame update
	void Start()
	{
		cManager = GetComponent<ConnectionManager>();

		if (cManager)
		{
			cManager.setupSocket();

			StartCoroutine(RequestHeartbeat(0.1f));
		}
	}

	public bool SendJoinRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestJoin request = new RequestJoin();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendLeaveRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestLeave request = new RequestLeave();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendSetNameRequest(string Name)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestSetName request = new RequestSetName();
			request.send(Name);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendCharacterRequest(string character)
    {
		if (cManager && cManager.IsConnected())
		{
			RequestCharacter request = new RequestCharacter();
			request.send(character);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendReadyRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestReady request = new RequestReady();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendMoveRequest(int x, int y, int z)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestMove request = new RequestMove();
			request.send(x, y, z);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendInteractRequest(int pieceIndex, int targetIndex)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestInteract request = new RequestInteract();
			request.send(pieceIndex, targetIndex);
			cManager.send(request);
			return true;
		}
		return false;
	}

    public bool SendThrowPotionRequest(int damage, int player, int card, int op)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestPotionThrow request = new RequestPotionThrow();
            request.send(damage, player, card, op);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendEndTurnRequest(int playerInt)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestEndTurn request = new RequestEndTurn();
            request.send(playerInt);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendBuyRequest(int x, int y)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestBuy request = new RequestBuy();
            request.send(x, y);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendSellRequest(int x, int y)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestSell request = new RequestSell();
            request.send(x, y);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public IEnumerator RequestHeartbeat(float time)
	{
		yield return new WaitForSeconds(time);

		if (cManager)
		{
			RequestHeartbeat request = new RequestHeartbeat();
			request.send();
			cManager.send(request);
		}

		StartCoroutine(RequestHeartbeat(time));
	}
}
