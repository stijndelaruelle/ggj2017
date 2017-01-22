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
	private AudioMixerSnapshot _victory;

	[SerializeField]
	private AudioMixerSnapshot _muted;

	[Space]
	[SerializeField]
	private AudioSource _victorySource;

	[SerializeField]
	private AudioSource _cheeringSource;

	[Space]
	[SerializeField]
	private AudioSource _loseSource;

	[Space]
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

	private bool _gameEnd = false;
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
		_muffled.TransitionTo(1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if (Input.GetKeyUp(KeyCode.O))
		//	TransitionToOriginal();

		//if (Input.GetKeyUp(KeyCode.P))
		//	TransitionToMuffled();
	}

	private void OnDisable()
	{
		CharacterManager.OnCharacterEnterConcert -= OpenDoor;
	}

	public void OpenDoor()
	{
		if (_gameEnd)
			return;

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
		while(_openTime > 0 && _gameEnd == false)
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

	public void TransitionToVictory()
	{
		_gameEnd = true;
		_victory.TransitionTo(.5f);

		_victorySource.Play();
		_cheeringSource.Play();
	}

	public void TransitionToLose()
	{
		_gameEnd = true;
		_victory.TransitionTo(.5f);

		_loseSource.Play();
	}
}
