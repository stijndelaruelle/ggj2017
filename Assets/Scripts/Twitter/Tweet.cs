using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweet
{
	public string Contents;
	public string ScrambledContents;

    private char[] ContentsCharArr;
	public Tweet(string contents)
	{
		//contents = ParseText.Utf16ToUtf8(contents);

		Contents = contents;
        ContentsCharArr = Contents.ToCharArray();

        ScrambledContents = contents;
        Scramble();
    }

    public void Scramble()
    {
        ContentsCharArr.Shuffle();
        ScrambledContents = new string(ContentsCharArr);

        byte[] bytes = System.Text.Encoding.Default.GetBytes(ScrambledContents);
        ScrambledContents = System.Text.Encoding.UTF8.GetString(bytes);
    }
}
