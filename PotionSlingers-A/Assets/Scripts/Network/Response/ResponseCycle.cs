using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseCycleEventArgs : ExtendedEventArgs
{
    public int user_id { get; set; } // The user_id of whom who sent the request
    public int x { get; set; } // The x coordinate of the target location
    public int y { get; set; } // The y coordinate of the target location

    public ResponseCycleEventArgs()
    {
        event_id = Constants.SMSG_CYCLE;
    }
}

public class ResponseCycle : NetworkResponse
{
    private int user_id;
    private int x;
    private int y;

    public ResponseCycle()
    {
    }

    public override void parse()
    {
        user_id = DataReader.ReadInt(dataStream);
        x = DataReader.ReadInt(dataStream);
        y = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponseCycleEventArgs args = new ResponseCycleEventArgs
        {
            user_id = user_id,
            x = x,
            y = y
        };

        return args;
    }
}
