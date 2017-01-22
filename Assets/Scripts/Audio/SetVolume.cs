using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetVolume : MonoBehaviour
{
	public float Volume;

	private AudioSource _audioSource;


	// Use this for initialization
	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_audioSource.volume = Volume;
	}
}
