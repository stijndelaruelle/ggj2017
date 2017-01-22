using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OpenDoorLight : MonoBehaviour
{
	#region Inspector Fields
	[Space]
	[SerializeField]
	private float _transitionDuration = .5f;

	[SerializeField]
	private float _openDoorDuration = 5;
	#endregion

	#region Fields
	private float _openTime = 0;

	private SpriteRenderer _spriteRenderer;
	#endregion

	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();

		CharacterManager.OnCharacterEnterConcert += OpenDoor;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
			_spriteRenderer.enabled = true;

			_openTime += _openDoorDuration;

			// Start timer.
			StartCoroutine(OpenDoorCoroutine());
		}


	}

	IEnumerator OpenDoorCoroutine()
	{
		while (_openTime > 0)
		{
			_openTime -= Time.deltaTime;

			yield return null;
		}

		// Close door.
		_spriteRenderer.enabled = false;
	}
}
