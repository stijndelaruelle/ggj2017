using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTweet : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	private bool _removeURLs;

	[SerializeField]
	private bool _removeUsernames;

	[SerializeField]
	private bool _removeHashtags;

	[SerializeField]
	private string _hashtag = "#GGJ17";
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		TwitterAPI.instance.SearchTwitter(_hashtag, GetRandomTweetCallBack);
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

	void GetRandomTweetCallBack(List<TweetSearchTwitterData> tweetList)
	{
		TweetSearchTwitterData randomTweetData = tweetList[Random.Range(0, tweetList.Count)];

		string tweetText = randomTweetData.tweetText;

		if (_removeURLs)
			tweetText = ParseText.RemoveURLs(tweetText);
		if (_removeUsernames)
			tweetText = ParseText.RemoveUsernames(tweetText);
		if (_removeHashtags)
			tweetText = ParseText.RemoveHashtags(tweetText);

		Tweet randomTweet = new Tweet(tweetText);

		Debug.Log("Random tweet scrambled: " + randomTweet.ScrambledContents);
		Debug.Log("Random tweet: " + randomTweet.Contents);
	}
}
