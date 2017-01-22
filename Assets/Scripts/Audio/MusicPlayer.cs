﻿using System.Collections;
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

	[SerializeField]
	private float _openDoorDuration = 5;
	#endregion

	#region Properties
	public static MusicPlayer Instance;
	#endregion

	#region Fields
	private float _openTime = 0;
	#endregion


	private void Awake()
	{
		if (Instance == null)
			Instance = this;

		CharacterManager.OnCharacterEnterConcert += OpenDoor;
	}

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

	private void OnDisable()
	{
		CharacterManager.OnCharacterEnterConcert -= OpenDoor;
	}

	public void OpenDoor()
	{
		if (_openTime > 0)
			_openTime += _openDoorDuration;
		else
		{
			// Open Door.
			TransitionToOriginal();

			_openTime += _openDoorDuration;

			// Start timer.
			StartCoroutine(OpenDoorCoroutine());
		}


	}

	IEnumerator OpenDoorCoroutine()
	{
		while(_openTime > 0)
		{
			_openTime -= Time.deltaTime;

			yield return null;
		}

		// Close door.
		TransitionToMuffled();
	}


	void TransitionToOriginal()
	{
		_original.TransitionTo(_transitionDuration);
	}

	void TransitionToMuffled()
	{
		_muffled.TransitionTo(_transitionDuration * 2);
	}
}
