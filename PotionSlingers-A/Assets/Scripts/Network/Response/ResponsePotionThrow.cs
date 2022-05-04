using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsePotionThrowEventArgs : ExtendedEventArgs
{
    public int user_id { get; set; } // The user_id of whom who sent the request
    public int x { get; set; } // The x coordinate of the target location
    public int y { get; set; } // The y coordinate of the target location
    public int z { get; set; } // The z coordinate of the target location

    public ResponsePotionThrowEventArgs()
    {
        event_id = Constants.SMSG_P_THROW;
    }
}

public class ResponsePotionThrow : NetworkResponse
{
    private int user_id;
    private int x;
    private int y;
    private int z;

    public ResponsePotionThrow()
    {
    }

    public override void parse()
    {
        user_id = DataReader.ReadInt(dataStream);
        x = DataReader.ReadInt(dataStream);
        y = DataReader.ReadInt(dataStream);
        z = DataReader.ReadInt(dataStream);
    }

    public override ExtendedEventArgs process()
    {
        ResponsePotionThrowEventArgs args = new ResponsePotionThrowEventArgs
        {
            user_id = user_id,
            x = x,
            y = y,
            z = z
        };

        return args;
    }
}
