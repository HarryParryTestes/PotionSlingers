using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseBuyEventArgs : ExtendedEventArgs
{
    public int user_id { get; set; } // The user_id of whom who sent the request
    public int x { get; set; } // The x coordinate of the target
    public int y { get; set; } // The y coordinate of the target 
    public int z { get; set; } // The z coordinate of the target

    public ResponseBuyEventArgs()
    {
        event_id = Constants.SMSG_BUY;
    }
}

public class ResponseBuy : NetworkResponse
{
    private int user_id;
    private int x;
    private int y;
    private int z;

    public ResponseBuy()
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
        ResponseBuyEventArgs args = new ResponseBuyEventArgs
        {
            user_id = user_id,
            x = x,
            y = y,
            z = z
        };

        return args;
    }
}
