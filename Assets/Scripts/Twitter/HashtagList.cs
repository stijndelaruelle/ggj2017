using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashtagList
{
	#region Properties
	public string Tag;

	public List<Tweet> Tweets = new List<Tweet>();
	#endregion

	public HashtagList(string tag, List<TweetSearchTwitterData> twitterData, bool removeURLs = true, bool removeUsernames = true, bool removeHashtags = true)
	{
		Tag = tag;

		for(int i = 0; i < twitterData.Count; i++)
		{
			string tweetText = twitterData[i].tweetText;

			tweetText = ParseText.CleanUp(tweetText);
			if (removeURLs)
				tweetText = ParseText.RemoveURLs(tweetText);
			if (removeUsernames)
				tweetText = ParseText.RemoveUsernames(tweetText);
			if (removeHashtags)
				tweetText = ParseText.RemoveHashtags(tweetText);

			Tweets.Add(new Tweet(tweetText));
		}
	}

	/// <summary>
	/// Returns a random tweet from this hashtagList.
	/// </summary>
	/// <returns></returns>
	public Tweet GetRandomTweet()
	{
		return Tweets[Random.Range(0, Tweets.Count)];
	}
}
