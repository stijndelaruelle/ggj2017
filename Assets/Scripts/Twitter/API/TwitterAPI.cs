using UnityEngine;

using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;

using MiniJSON;

public class TwitterAPI : MonoBehaviour {	
	public string oauthConsumerKey = "";
	public string oauthConsumerSecret = "";
	public string oauthToken = "";
	public string oauthTokenSecret = "";
	
	private string oauthNonce = "";
	private string oauthTimeStamp = "";
	
	public static TwitterAPI instance = null;
	
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		}
		else {
			Debug.LogError("More then one instance of TwitterAPI: " + this.transform.name);
		}
	}

	public void SearchTwitter(string keywords, Action<List<TweetSearchTwitterData> > callback) 
	{	
		PrepareOAuthData();
		StartCoroutine(SearchTwitter_Coroutine(keywords, callback));
	}

	public void FindTrendsByLocationTwitter(string location, Action<List<TrendByLocationTwitterData> > callback) 
	{		
		PrepareOAuthData();
		StartCoroutine(FindTrendsByLocation_Coroutine(location, callback));
	}
	
	private IEnumerator SearchTwitter_Coroutine(string keywords, Action<List<TweetSearchTwitterData> > callback) 
	{
		// Fix up hashes to be webfriendly
		keywords = Uri.EscapeDataString(keywords);

		string twitterUrl = "https://api.twitter.com/1.1/search/tweets.json";
		
		SortedDictionary<string, string> twitterParamsDictionary = new SortedDictionary<string, string>
        {
            {"q", keywords},
			{"count", "100"},
			{"result_type", "popular"},
		};

		WWW query = CreateTwitterAPIQuery(twitterUrl, twitterParamsDictionary);
		yield return query;

		callback(ParseResultsFromSearchTwitter(query.text));
	}

	// Use of MINI JSON http://forum.unity3d.com/threads/35484-MiniJSON-script-for-parsing-JSON-data
	private List<TweetSearchTwitterData> ParseResultsFromSearchTwitter(string jsonResults) {
		//Debug.Log(jsonResults);
		
		List<TweetSearchTwitterData> twitterDataList = new List<TweetSearchTwitterData>();
		object jsonObject = Json.Deserialize(jsonResults);
		IDictionary search = (IDictionary) jsonObject;
		IList tweets = (IList) search["statuses"];
		foreach (IDictionary tweet in tweets) {
			IDictionary userInfo = tweet["user"] as IDictionary;			
			
			TweetSearchTwitterData twitterData = new TweetSearchTwitterData();			
			twitterData.tweetText = tweet["text"] as string;
			twitterData.screenName = userInfo["screen_name"] as string;
			twitterData.retweetCount = (Int64)tweet["retweet_count"];
			twitterData.profileImageUrl = userInfo["profile_image_url"] as string;
			
			twitterDataList.Add(twitterData);
		} 
		
		return twitterDataList;
	}

	private IEnumerator FindTrendsByLocation_Coroutine(string location, Action<List<TrendByLocationTwitterData> > callback)
	{
		location = Uri.EscapeDataString(location);
		string twitterUrl = "https://api.twitter.com/1.1/trends/place.json";
		SortedDictionary<string, string> twitterParamsDictionary = new SortedDictionary<string, string>
		{
			{"id", location}
		};

		WWW query = CreateTwitterAPIQuery(twitterUrl, twitterParamsDictionary);
		yield return query;
		
		callback(ParseResultsFromFindTrendsByLocationTwitter(query.text));
	}

	private List<TrendByLocationTwitterData> ParseResultsFromFindTrendsByLocationTwitter(string jsonResults) {
		Debug.Log(jsonResults);
		
		List<TrendByLocationTwitterData> twitterDataList = new List<TrendByLocationTwitterData>();
		object jsonObject = Json.Deserialize(jsonResults);
		IList jsonList = jsonObject as IList;
		IDictionary search = jsonList[0] as IDictionary;
		IList trends = (IList) search["trends"];
		foreach (IDictionary trend in trends) {
			TrendByLocationTwitterData twitterData = new TrendByLocationTwitterData();			
			twitterData.name = trend["name"] as string;
			twitterData.query = trend["query"] as string;
			twitterData.url = trend["url"] as string;
			
			twitterDataList.Add(twitterData);
		} 
		
		return twitterDataList;
	}

	private WWW CreateTwitterAPIQuery(string twitterUrl, SortedDictionary<string, string> twitterParamsDictionary)
	{
		string signature = CreateSignature(twitterUrl, twitterParamsDictionary);
		//Debug.Log("OAuth Signature: " + signature);
		
		string authHeaderParam = CreateAuthorizationHeaderParameter(signature, this.oauthTimeStamp);
		//Debug.Log("Auth Header: " + authHeaderParam);
		
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers["Authorization"] = authHeaderParam;
		
		string twitterParams = ParamDictionaryToString(twitterParamsDictionary);
		
		WWW query = new WWW(twitterUrl + "?" + twitterParams, null, headers);		
		return query;
	}


	private void PrepareOAuthData() {
		oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));
		TimeSpan _timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);		
		oauthTimeStamp = Convert.ToInt64(_timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);

		// Override the nounce and timestamp here if troubleshooting with Twitter's OAuth Tool
		//oauthNonce = "69db07d069ac50cd673f52ee08678596";
		//oauthTimeStamp = "1442419142";
	}
		
	// Taken from http://www.i-avington.com/Posts/Post/making-a-twitter-oauth-api-call-using-c
	private string CreateSignature(string url, SortedDictionary<string, string> searchParamsDictionary)
    {
        //string builder will be used to append all the key value pairs
        StringBuilder signatureBaseStringBuilder = new StringBuilder();
        signatureBaseStringBuilder.Append("GET&");
        signatureBaseStringBuilder.Append(Uri.EscapeDataString(url));
        signatureBaseStringBuilder.Append("&");

        //the key value pairs have to be sorted by encoded key
        SortedDictionary<string, string> urlParamsDictionary = new SortedDictionary<string, string>
                             {
                                 {"oauth_version", "1.0"},
                                 {"oauth_consumer_key", this.oauthConsumerKey},
                                 {"oauth_nonce", this.oauthNonce},
                                 {"oauth_signature_method", "HMAC-SHA1"},
                                 {"oauth_timestamp", this.oauthTimeStamp},
                                 {"oauth_token", this.oauthToken}
                             };
        
		foreach (KeyValuePair<string, string> keyValuePair in searchParamsDictionary)
        {
			urlParamsDictionary.Add(keyValuePair.Key, keyValuePair.Value);
		}    
		
		signatureBaseStringBuilder.Append(Uri.EscapeDataString(ParamDictionaryToString(urlParamsDictionary)));
       	string signatureBaseString = signatureBaseStringBuilder.ToString();

		//Debug.Log("Signature Base String: " + signatureBaseString);
		
        //generation the signature key the hash will use
        string signatureKey =
            Uri.EscapeDataString(this.oauthConsumerSecret) + "&" +
            Uri.EscapeDataString(this.oauthTokenSecret);

        HMACSHA1 hmacsha1 = new HMACSHA1(
            new ASCIIEncoding().GetBytes(signatureKey));

        //hash the values
        string signatureString = Convert.ToBase64String(
            hmacsha1.ComputeHash(
                new ASCIIEncoding().GetBytes(signatureBaseString)));
		
        return signatureString;
    }
	
	private string CreateAuthorizationHeaderParameter(string signature, string timeStamp)
    {
        string authorizationHeaderParams = String.Empty;
        authorizationHeaderParams += "OAuth ";
       
		authorizationHeaderParams += "oauth_consumer_key="
                                     + "\"" + Uri.EscapeDataString(this.oauthConsumerKey) + "\", ";
		
		authorizationHeaderParams += "oauth_nonce=" + "\"" +
                                     Uri.EscapeDataString(this.oauthNonce) + "\", ";
		
		authorizationHeaderParams += "oauth_signature=" + "\""
                                     + Uri.EscapeDataString(signature) + "\", ";
       
		authorizationHeaderParams += "oauth_signature_method=" + "\"" +
            Uri.EscapeDataString("HMAC-SHA1") +
            "\", ";

        authorizationHeaderParams += "oauth_timestamp=" + "\"" +
                                     Uri.EscapeDataString(timeStamp) + "\", ";        

        authorizationHeaderParams += "oauth_token=" + "\"" +
                                     Uri.EscapeDataString(this.oauthToken) + "\", ";

        authorizationHeaderParams += "oauth_version=" + "\"" +
                                     Uri.EscapeDataString("1.0") + "\"";
        return authorizationHeaderParams;
    }
	
	private string ParamDictionaryToString(IDictionary<string, string> paramsDictionary) {
		StringBuilder dictionaryStringBuilder = new StringBuilder();       
		foreach (KeyValuePair<string, string> keyValuePair in paramsDictionary)
        {
            //append a = between the key and the value and a & after the value
            dictionaryStringBuilder.Append(string.Format("{0}={1}&", keyValuePair.Key, keyValuePair.Value));
        }

		// Get rid of the extra & at the end of the string
		string paramString = dictionaryStringBuilder.ToString().Substring(0, dictionaryStringBuilder.Length - 1);
		return paramString;
	}
}
