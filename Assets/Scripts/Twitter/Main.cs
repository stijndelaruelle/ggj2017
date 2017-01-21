using UnityEngine;

using System.Collections.Generic;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TwitterAPI.instance.SearchTwitter("#GGJ17", SearchTweetsResultsCallBack);
		TwitterAPI.instance.FindTrendsByLocationTwitter("1", FindTrendsByLocatiobResultsCallBack);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SearchTweetsResultsCallBack(List<TweetSearchTwitterData> tweetList) {
		Debug.Log("====================================================");
		foreach(TweetSearchTwitterData twitterData in tweetList) {
			//Debug.Log("Tweet: " + twitterData.ToString());
			Debug.Log("Tweet text: " + twitterData.tweetText);
		}
	}

	void FindTrendsByLocatiobResultsCallBack(List<TrendByLocationTwitterData> trendList) {
		Debug.Log("====================================================");
		foreach(TrendByLocationTwitterData twitterData in trendList) {
			Debug.Log("Trend: " + twitterData.ToString());
		}
	}
}
