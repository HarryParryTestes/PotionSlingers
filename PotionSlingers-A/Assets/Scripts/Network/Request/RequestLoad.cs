using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestLoad : NetworkRequest
{
    public RequestLoad()
    {
        request_id = Constants.CMSG_LOAD;
    }

    public void send(int x, int y)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(x);
        packet.addInt32(y);

    }
}

