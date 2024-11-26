using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestCycle : NetworkRequest
{
    public RequestCycle()
    {
        request_id = Constants.CMSG_CYCLE;
    }

    // card and buy price
    public void send(int x, int y)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(x);
        packet.addInt32(y);

    }
}

