using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OldNetworkManager : MonoBehaviour
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

	public bool SendReadyRequest(int readyStatus)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestReady request = new RequestReady();
			request.send(readyStatus);
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

    public bool SendThrowPotionRequest(int throwerId, int cardPosition, int targetId, int damage, bool isArtifact, bool isVessel)
    {
		// Args Potion: throwerId, cardPosition, targetId, damage, isArtifact (T/F), isVessel (T/F), vesselSpot (1 or 2, 0 if not Vessel)
        if (cManager && cManager.IsConnected())
        {
            RequestPotionThrow request = new RequestPotionThrow();
            // request.send(damage, player, card, op);
            request.send(throwerId, cardPosition, targetId, damage, isArtifact, isVessel);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendEndTurnRequest(int newCurrentPlayerId)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestEndTurn request = new RequestEndTurn();
            request.send(newCurrentPlayerId);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendBuyRequest(int x, int y, int z)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestBuy request = new RequestBuy();
            request.send(x, y, z);
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

    public bool sendCycleRequest(int x, int y)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestCycle request = new RequestCycle();
            request.send(x, y);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendTrashRequest(int x, int y)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestTrash request = new RequestTrash();
            request.send(x, y);
            cManager.send(request);
            return true;
        }
        return false;
    }

    public bool sendLoadRequest(int x, int y)
    {
        if (cManager && cManager.IsConnected())
        {
            RequestLoad request = new RequestLoad();
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
