using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseSetNameEventArgs : ExtendedEventArgs
{
	public int numPlayers { get; set; }
	public int user_id1 { get; set; } // The user_id of P1
	public string name1 { get; set; } // Their new name P1
	public int user_id2 { get; set; } // The user_id of P2
	public string name2 { get; set; } // Their new name P2

	public ResponseSetNameEventArgs()
	{
		event_id = Constants.SMSG_SETNAME;
	}
}

public class ResponseSetName : NetworkResponse
{
	private int numPlayers;
	private int user_id1;
	private string name1;
	private int user_id2;
	private string name2;

	public ResponseSetName()
	{
	}

	public override void parse()
	{
		numPlayers = DataReader.ReadInt(dataStream);
		user_id1 = DataReader.ReadInt(dataStream);
		name1 = DataReader.ReadString(dataStream);
		user_id2 = DataReader.ReadInt(dataStream);
		name2 = DataReader.ReadString(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseSetNameEventArgs args = new ResponseSetNameEventArgs
		{
			numPlayers = numPlayers,
			user_id1 = user_id1,
			user_id2 = user_id2,
			name1 = name1,
			name2 = name2
		};

		return args;
	}
}
