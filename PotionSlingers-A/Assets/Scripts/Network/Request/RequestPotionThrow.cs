using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPotionThrow : NetworkRequest
{
    public RequestPotionThrow()
    {
        request_id = Constants.CMSG_P_THROW;
    }

    public void send(int x, int y, int z)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(x);
        packet.addInt32(y);
        packet.addInt32(z);
    }
}

