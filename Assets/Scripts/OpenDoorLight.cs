using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OpenDoorLight : MonoBehaviour
{
	#region Inspector Fields
	[SerializeField]
	private float _transitionDuration = 1f;

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
		_spriteRenderer.enabled = false;

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
			_openTime += _openDoorDuration;

			// Start timer.
			StartCoroutine(OpenDoorCoroutine());
		}


	}

	IEnumerator OpenDoorCoroutine()
	{
		yield return new WaitForSeconds(_transitionDuration);

		// Open Door.
		_spriteRenderer.enabled = true;

		while (_openTime > 0)
		{
			_openTime -= Time.deltaTime;

			yield return null;
		}

		// Close door.
		_spriteRenderer.enabled = false;
	}
}
