using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTweet : MonoBehaviour
{
	#region Inspector Fields
	//[SerializeField]
	//private string _hashtag = "#GGJ17";
	#endregion

	#region Fields
	private Text _characterText;
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		_characterText = GetComponentInChildren<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Space))
		{
			Tweet requestedTweet = Twitter.Instance.GetRandomHashTag().GetRandomTweet();

			_characterText.text = requestedTweet.ScrambledContents;

			Debug.Log(requestedTweet.ScrambledContents);
			Debug.Log(requestedTweet.Contents);
		}
	}
	#endregion
}
