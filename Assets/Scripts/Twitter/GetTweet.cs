using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTweet : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	private string _hashtag = "#GGJ17";
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Space))
		{
			Tweet requestedTweet = Twitter.Instance.GetHashTag(_hashtag).GetRandomTweet();

			Debug.Log(requestedTweet.ScrambledContents);
			Debug.Log(requestedTweet.Contents);
		}
	}
	#endregion
}
