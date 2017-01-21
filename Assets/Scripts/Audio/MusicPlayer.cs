using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	private AudioMixerSnapshot _original;

	[SerializeField]
	private AudioMixerSnapshot _muffled;

	[SerializeField]
	private float _transitionDuration = .5f;
	#endregion

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.O))
			TransitionToOriginal();

		if (Input.GetKeyUp(KeyCode.P))
			TransitionToMuffled();
	}

	void TransitionToOriginal()
	{
		_original.TransitionTo(_transitionDuration);
	}

	void TransitionToMuffled()
	{
		_muffled.TransitionTo(_transitionDuration);
	}
}
