using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twitter : MonoBehaviour
{
	#region Inspector Fields
	//[SerializeField]
	//private bool _removeURLs;

	//[SerializeField]
	//private bool _removeUsernames;

	//[SerializeField]
	//private bool _removeHashtags;

	public List<string> Hashtags = new List<string>();
	#endregion

	#region Properties
	public static Twitter Instance = null;
	#endregion

	#region Fields
	private TweetSearchTwitterData _lastFoundTweet;

	public List<HashtagList> _hashTagLists = new List<HashtagList>();
	#endregion


	#region Life Cycle
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Debug.LogWarning("There already exists a Twitter object in this scene!", this);
	}

	private void Start()
	{
		// Cache the needed hashtags.
		for (int i = 0; i < Hashtags.Count; i++)
		{
			RequestPopularTweets(Hashtags[i]);
		}
	}
	#endregion

	#region Twitter API
	void RequestPopularTweets(string hashtag)
	{
		TwitterAPI.instance.SearchTwitter(hashtag, CachePopularTweetsCallBack);
	}

	void CachePopularTweetsCallBack(List<TweetSearchTwitterData> tweetList)
	{
		if(tweetList.Count == 0)
		{
			Debug.LogWarning("No popular tweets found.", this);
			return;
		}

		HashtagList newHashtagList = new HashtagList(tweetList[0].hashtag, tweetList);
		_hashTagLists.Add(newHashtagList);

		Debug.Log("Cached hashtag(" + newHashtagList.Tag + "). " + _hashTagLists.Count + " hashtags cached.");
	}
	#endregion

	public HashtagList GetHashTag(string hashTag)
	{
		hashTag = hashTag.Replace("#", "%23");
		HashtagList hashTagToFind = _hashTagLists.Find(h => h.Tag == hashTag);

		if (hashTagToFind == null)
			Debug.LogWarning("No hashtag found.");

		return hashTagToFind;
	}

	public HashtagList GetRandomHashTag()
	{
		if(_hashTagLists.Count == 0)
		{
			Debug.LogWarning("No hash tag lists found!", this);
			return null;
		}

		return _hashTagLists[Random.Range(0, _hashTagLists.Count)];
	}
}
