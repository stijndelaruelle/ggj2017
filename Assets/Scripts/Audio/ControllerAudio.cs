using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerAudio : MonoBehaviour
{
	#region Fields
	private audio_pitch _audioPitch;
	private AudioSource _audioSource;

	private BrainwaveDevice _brainWaveDevice;

	//private Character _character;
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		_audioPitch = GetComponent<audio_pitch>();
		_brainWaveDevice = GetComponent<BrainwaveDevice>();
		_audioSource = GetComponent<AudioSource>();
		//_character = GetComponent<Character>();

		_brainWaveDevice.HackedEvent += PlayTune;
	}

	// Update is called once per frame
	void Update()
	{
		//SetPitch();
		SetVolume();
	}

	private void OnDisable()
	{
		_brainWaveDevice.HackedEvent += PlayTune;
	}
	#endregion

	#region Methods
	void SetPitch()
	{
		//Debug.Log(_brainWaveDevice.MinimalDifference);
		_audioPitch.pitch = 50 * (.25f - _brainWaveDevice.MinimalDifference);
	}

	void SetVolume()
	{
		_audioSource.volume = (.5f - _brainWaveDevice.MinimalDifference) * 2;
	}

	void PlayTune()
	{
		//if(!_audioSource.isPlaying)
		//	_audioSource.Play();
	}
	#endregion
}
