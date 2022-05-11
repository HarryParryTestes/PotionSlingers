using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPotionThrow : NetworkRequest
{
    public RequestPotionThrow()
    {
        request_id = Constants.CMSG_P_THROW;
    }

    public void send(int throwerId, int cardPosition, int targetId, int damage, bool isArtifact, bool isVessel)
    {
        packet = new GamePacket(request_id);
        packet.addInt32(throwerId);
        packet.addInt32(cardPosition);
        packet.addInt32(targetId);
        packet.addInt32(damage);
        packet.addBool(isArtifact);
        packet.addBool(isVessel);
    }
}

