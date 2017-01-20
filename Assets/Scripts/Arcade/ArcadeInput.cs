using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeInput : MonoBehaviour
{
	#region Life Cycle
	// Use this for initialization
	void Start()
	{
		// Player One
		InputManager.Instance.BindButton("PlayerOneUp", KeyCode.UpArrow, InputManager.ButtonState.OnPress);
		InputManager.Instance.BindButton("PlayerOneDown", KeyCode.DownArrow, InputManager.ButtonState.OnPress);
		InputManager.Instance.BindButton("PlayerOneRight", KeyCode.RightArrow, InputManager.ButtonState.OnPress);
		InputManager.Instance.BindButton("PlayerOneLeft", KeyCode.LeftArrow, InputManager.ButtonState.OnPress);

		InputManager.Instance.BindButton("PlayerOneButtonOne", KeyCode.Z, InputManager.ButtonState.OnRelease);
		InputManager.Instance.BindButton("PlayerOneButtonTwo", KeyCode.X, InputManager.ButtonState.OnRelease);

		// Player Two
		InputManager.Instance.BindButton("PlayerTwoUp", KeyCode.R, InputManager.ButtonState.OnPress);
		InputManager.Instance.BindButton("PlayerTwoDown", KeyCode.F, InputManager.ButtonState.OnPress);
		InputManager.Instance.BindButton("PlayerTwoRight", KeyCode.G, InputManager.ButtonState.OnPress);
		InputManager.Instance.BindButton("PlayerTwoLeft", KeyCode.D, InputManager.ButtonState.OnPress);

		InputManager.Instance.BindButton("PlayerTwoButtonOne", KeyCode.I, InputManager.ButtonState.OnRelease);
		InputManager.Instance.BindButton("PlayerTwoButtonTwo", KeyCode.K, InputManager.ButtonState.OnRelease);
	}

	// Update is called once per frame
	void Update()
	{
		ArcadeInputs();
	}
	#endregion


	#region Input
	/// <summary>
	/// Handles all the inputs for the arcade.
	/// </summary>
	void ArcadeInputs()
	{
		// Player One
		bool playerOneUp = InputManager.Instance.GetButton("PlayerOneUp");
		bool playerOneDown = InputManager.Instance.GetButton("PlayerOneDown");
		bool playerOneRight = InputManager.Instance.GetButton("PlayerOneRight");
		bool playerOneLeft = InputManager.Instance.GetButton("PlayerOneLeft");

		bool playerOneButtonOne = InputManager.Instance.GetButton("PlayerOneButtonOne");
		bool playerOneButtonTwo = InputManager.Instance.GetButton("PlayerOneButtonTwo");

		// Player Two
		bool playerTwoUp = InputManager.Instance.GetButton("PlayerTwoUp");
		bool playerTwoDown = InputManager.Instance.GetButton("PlayerTwoDown");
		bool playerTwoRight = InputManager.Instance.GetButton("PlayerTwoRight");
		bool playerTwoLeft = InputManager.Instance.GetButton("PlayerTwoLeft");

		bool playerTwoButtonOne = InputManager.Instance.GetButton("PlayerTwoButtonOne");
		bool playerTwoButtonTwo = InputManager.Instance.GetButton("PlayerTwoButtonTwo");

		// Inputs Player One
		if (playerOneUp)
			Debug.Log("playerOneUp");
		if (playerOneDown)
			Debug.Log("playerOneDown");
		if (playerOneRight)
			Debug.Log("playerOneRight");
		if (playerOneLeft)
			Debug.Log("playerOneLeft");

		if (playerOneButtonOne)
			Debug.Log("playerOneButtonOne");
		if (playerOneButtonTwo)
			Debug.Log("playerOneButtonTwo");

		// Inputs Player Two
		if (playerTwoUp)
			Debug.Log("playerTwoUp");
		if (playerTwoDown)
			Debug.Log("playerTwoDown");
		if (playerTwoRight)
			Debug.Log("playerTwoRight");
		if (playerTwoLeft)
			Debug.Log("playerTwoLeft");

		if (playerTwoButtonOne)
			Debug.Log("playerTwoButtonOne");
		if (playerTwoButtonTwo)
			Debug.Log("playerTwoButtonTwo");
	}
	#endregion
}
