using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseEndTurnEventArgs : ExtendedEventArgs
{
    public int user_id { get; set; } // The user_id of whom who sent the request
    public int w { get; set; } // current player

    public ResponseEndTurnEventArgs()
    {
        event_id = Constants.SMSG_END_TURN;
    }
}

public class ResponseEndTurn : NetworkResponse
{
    private int user_id;
    private int w;

    public ResponseEndTurn()
    {

    }

    public override void parse()
    {
        user_id = DataReader.ReadInt(dataStream);
        w = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponseEndTurnEventArgs args = new ResponseEndTurnEventArgs
        {
            user_id = user_id,
            w = w,
        };

        return args;
    }
}
