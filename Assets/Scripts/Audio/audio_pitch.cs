using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_pitch : MonoBehaviour {



	private AudioSource audio_test;
	[Range(1.0f, 20.0f)]
	public float pitch = 1.0f;


	private Object[] AudioArray_audio;


	// Use this for initialization
	void Start () {
		pitch = 1.0f;
		audio_test = (AudioSource)gameObject.AddComponent <AudioSource>();
		AudioArray_audio = Resources.LoadAll("Audio",typeof(AudioClip));
		audio_test.clip = AudioArray_audio[0] as AudioClip;
		audio_test.loop = true;
		audio_test.Play();
	}
	
	// Update is called once per frame
	void Update () {
		audio_test.pitch = pitch;
		//pitch = pitch + Time.deltaTime;
		
	}
}
