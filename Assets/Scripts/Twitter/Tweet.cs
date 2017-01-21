using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweet
{
	public string Contents;
	public string ScrambledContents;

	public Tweet(string contents)
	{
		Contents = contents;
		ScrambledContents = contents.Shuffle();
	}
}
