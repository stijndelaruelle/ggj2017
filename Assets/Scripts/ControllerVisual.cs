using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerVisual : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	private Transform _frequencyKnob;

	[SerializeField]
	private Transform _amplitudeKnob;

	[Space]
	[SerializeField]
	private BrainwaveDevice _brainWaveDevice;
	#endregion

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		_frequencyKnob.localEulerAngles = new Vector3(0, 0, _brainWaveDevice.FrequencyAngle - 90);
		_amplitudeKnob.localEulerAngles = new Vector3(0, 0, _brainWaveDevice.AmplitudeAngle - 90);
	}
}
