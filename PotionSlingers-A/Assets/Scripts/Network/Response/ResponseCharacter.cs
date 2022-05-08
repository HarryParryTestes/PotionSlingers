using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseCharacterEventArgs : ExtendedEventArgs
{
	public int numPlayers { get; set; } // Number of players
	public int user_id1 { get; set; } // P1 user_id
	public string characterName1 { get; set; } // P1 character name
	public int user_id2 { get; set; } // P2 user_id
	public string characterName2 { get; set; } // P2 character name

	public ResponseCharacterEventArgs()
	{
		event_id = Constants.SMSG_CHARACTER;
	}
}

public class ResponseCharacter : NetworkResponse
{
	private int numPlayers;
	private int user_id1;
	private string characterName1;
	private int user_id2;
	private string characterName2;

	public ResponseCharacter()
	{
	}

	public override void parse()
	{
		numPlayers = DataReader.ReadInt(dataStream);
		user_id1 = DataReader.ReadInt(dataStream);
		characterName1 = DataReader.ReadString(dataStream);
		user_id2 = DataReader.ReadInt(dataStream);
		characterName2 = DataReader.ReadString(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseCharacterEventArgs args = new ResponseCharacterEventArgs
		{
			numPlayers = numPlayers,
			user_id1 = user_id1,
			characterName1 = characterName1,
			user_id2 = user_id2,
			characterName2 = characterName2
		};

		return args;
	}
}
