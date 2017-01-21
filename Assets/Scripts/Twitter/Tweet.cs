using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweet
{
	public string Contents;
	public string ScrambledContents;

	public Tweet(string contents)
	{
		//contents = ParseText.Utf16ToUtf8(contents);

		Contents = contents;
		ScrambledContents = ParseText.Utf16ToUtf8(ParseText.Shuffle(contents));

		//Debug.Log(Contents);
		//Debug.Log(ScrambledContents);
	}
}
