using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsePotionThrowEventArgs : ExtendedEventArgs
{
    public int throwerId { get; set; } // The user_id of whom who sent the request (aka the player who threw card)
    public int cardPosition { get; set; } // The position of thrower's card in their Holster
    public int targetId { get; set; } // The user_id of the target opponent that thrower threw card at.
    public int damage { get; set; } // The damage that target opponent will take.

    public ResponsePotionThrowEventArgs()
    {
        event_id = Constants.SMSG_P_THROW;
    }
}

public class ResponsePotionThrow : NetworkResponse
{
    private int throwerId;
    private int cardPosition;
    private int targetId;
    private int damage;

    public ResponsePotionThrow()
    {
    }

    public override void parse()
    {
        throwerId = DataReader.ReadInt(dataStream);
        cardPosition = DataReader.ReadInt(dataStream);
        targetId = DataReader.ReadInt(dataStream);
        damage = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponsePotionThrowEventArgs args = new ResponsePotionThrowEventArgs
        {
            throwerId = throwerId,
            cardPosition = cardPosition,
            targetId = targetId,
            damage = damage
        };

        return args;
    }
}
