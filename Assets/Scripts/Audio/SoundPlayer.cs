using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
	#region Inspector Fields
	public AudioSource Hacked;

	public AudioSource TicketSold;
	#endregion

	#region Properties
	public static SoundPlayer Instance;
	#endregion


	#region Life Cycle 
	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion
}
