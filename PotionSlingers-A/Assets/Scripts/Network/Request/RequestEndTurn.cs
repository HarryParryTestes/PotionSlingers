using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestEndTurn : NetworkRequest
{
    public RequestEndTurn()
    {
        request_id = Constants.CMSG_END_TURN;
    }

    public void send(int player)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(player);
    }
}
