using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseReadyEventArgs : ExtendedEventArgs
{
	public int numPlayers { get; set; } // Number of players
	public int user_id1 { get; set; } // P1 user_id
	public bool player1Ready { get; set; } // P1 ready status
	public int user_id2 { get; set; } // P1 user_id
	public bool player2Ready { get; set; } // P1 ready status

	public ResponseReadyEventArgs()
	{
		event_id = Constants.SMSG_READY;
	}
}

public class ResponseReady : NetworkResponse
{
	private int numPlayers;
	private int user_id1;
	private bool player1Ready;
	private int user_id2;
	private bool player2Ready;

	public ResponseReady()
	{
	}

	public override void parse()
	{
		numPlayers = DataReader.ReadInt(dataStream);
		user_id1 = DataReader.ReadInt(dataStream);
		player1Ready = DataReader.ReadBool(dataStream);
		user_id2 = DataReader.ReadInt(dataStream);
		player2Ready = DataReader.ReadBool(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseReadyEventArgs args = new ResponseReadyEventArgs
		{
			numPlayers = numPlayers,
			user_id1 = user_id1,
			player1Ready = player1Ready,
			user_id2 = user_id2,
			player2Ready = player2Ready
		};

		return args;
	}
}
