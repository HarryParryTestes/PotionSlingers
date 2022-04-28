using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseCharacterEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public string name { get; set; } // Their new name

	public ResponseCharacterEventArgs()
	{
		event_id = Constants.SMSG_CHARACTER;
	}
}

public class ResponseCharacter : NetworkResponse
{
	private int user_id;
	private string name;

	public ResponseCharacter()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		name = DataReader.ReadString(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseCharacterEventArgs args = new ResponseCharacterEventArgs
		{
			user_id = user_id,
			name = name
		};

		return args;
	}
}
