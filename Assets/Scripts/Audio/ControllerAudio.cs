using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerAudio : MonoBehaviour
{
	#region Fields
	private audio_pitch _audioPitch;

	private BrainwaveDevice _brainWaveDevice;
	#endregion


	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		_audioPitch = GetComponent<audio_pitch>();
		_brainWaveDevice = GetComponent<BrainwaveDevice>();
	}

	// Update is called once per frame
	void Update()
	{
		SetPitch();
	}
	#endregion

	#region Methods
	void SetPitch()
	{
		Debug.Log(_brainWaveDevice.MinimalDifference);
		_audioPitch.pitch = 100 * (.25f - _brainWaveDevice.MinimalDifference);
	}
	#endregion
}
