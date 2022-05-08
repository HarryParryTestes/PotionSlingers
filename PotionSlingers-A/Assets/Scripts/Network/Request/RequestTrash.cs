using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTrash : NetworkRequest
{
    public RequestTrash()
    {
        request_id = Constants.CMSG_TRASH;
    }

    // card and buy price
    public void send(int x, int y)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(x);
        packet.addInt32(y);

    }
}

