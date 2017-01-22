using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;
using System;

public class BrainwaveDevice : MonoBehaviour
{
    [SerializeField]
    private int m_DeviceID = -1;

    [SerializeField]
    private float m_ErrorMargin;

    [SerializeField]
    private CharacterManager m_CharacterManager;

    private float m_Frequency;
    private float m_FrequencyAngle;
    public float FrequencyAngle
    {
        get { return m_FrequencyAngle; }
    }

    private float m_Amplitude;
    private float m_AmplitudeAngle;
    public float AmplitudeAngle
    {
        get { return m_AmplitudeAngle; }
    }

    private float m_BrainPowerLeft;
    public float BrainPowerLeft
    {
        get { return m_BrainPowerLeft; }
    }

    private float m_BrainPowerRight;
    public float BrainPowerRight
    {
        get { return m_BrainPowerRight; }
    }

    private Character m_Target;

    //Prev input (fixes hold exploit)
    private float m_PrevBrainPowerLeft = 0.0f;
    private float m_PrevBrainPowerRight = 0.0f;

    //When the device get's hacked
    private bool m_InversedControls;
    public bool InversedControls
    {
        get { return m_InversedControls; }
        set { m_InversedControls = value; }
    }

    private bool m_SwitchedControls;
    public bool SwitchedControls
    {
        get { return m_SwitchedControls; }
        set { m_SwitchedControls = value; }
    }

    //Event
    private Action<float, float> m_UpdateValuesEvent;
    public Action<float, float> UpdateValuesEvent
    {
        get { return m_UpdateValuesEvent; }
        set { m_UpdateValuesEvent = value; }
    }

    private Action<Character> m_UpdateTargetEvent;
    public Action<Character> UpdateTargetEvent
    {
        get { return m_UpdateTargetEvent; }
        set { m_UpdateTargetEvent = value; }
    }

	public Action HackedEvent;

    //Cache
    private InputManager m_InputManager;

	// Approximation
	public float MinimalDifference = 1;


    private void Start()
    {
        m_InputManager = InputManager.Instance;

        m_InputManager.BindAxis("Frequency_X_" + m_DeviceID, m_DeviceID, ControllerAxisCode.LeftStickX);
        m_InputManager.BindAxis("Frequency_Y_" + m_DeviceID, m_DeviceID, ControllerAxisCode.LeftStickY);

        m_InputManager.BindAxis("Amplitude_X_" + m_DeviceID, m_DeviceID, ControllerAxisCode.RightStickX);
        m_InputManager.BindAxis("Amplitude_Y_" + m_DeviceID, m_DeviceID, ControllerAxisCode.RightStickY);

        m_InputManager.BindAxis("BrainPowerLeft_" + m_DeviceID, m_DeviceID, ControllerAxisCode.LeftTrigger);
        m_InputManager.BindAxis("BrainPowerRight_" + m_DeviceID, m_DeviceID, ControllerAxisCode.RightTrigger);
    }

    private void OnDestroy()
    {
        ControllerInput.SetVibration(m_DeviceID, 0.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        if (m_DeviceID == -1)
            return;

        UpdateFrequency();
        UpdateAmplitude();

        UpdateHacking();
        UpdateCommand();

        if (m_UpdateValuesEvent != null)
            m_UpdateValuesEvent(m_Frequency, m_Amplitude);
    }

    private void UpdateFrequency()
    {
        //Gather input
        float newFrequencyX = m_InputManager.GetAxis("Frequency_X_" + m_DeviceID);
        float newFrequencyY = m_InputManager.GetAxis("Frequency_Y_" + m_DeviceID);

        if (m_SwitchedControls)
        {
            newFrequencyX = m_InputManager.GetAxis("Amplitude_X_" + m_DeviceID);
            newFrequencyY = m_InputManager.GetAxis("Amplitude_Y_" + m_DeviceID);
        }

        Vector2 newFrequencyVector = new Vector2(newFrequencyX, newFrequencyY);
        float distance = newFrequencyVector.magnitude;

        if (distance < 0.5f)
        {
            m_Frequency = 0.5f;
            return;
        }

        newFrequencyVector.Normalize();

        if (m_InversedControls)
        {
            newFrequencyVector.x = 1.0f - newFrequencyVector.x;
            newFrequencyVector.y = 1.0f - newFrequencyVector.y;
        }

        float angle = Mathf.Atan2(newFrequencyVector.y, newFrequencyVector.x) * Mathf.Rad2Deg;
        m_FrequencyAngle = angle;

        angle += 90.0f;

        if (angle < 0.0f)   { angle += 360.0f; }
        if (angle > 360.0f) { angle -= 360.0f; }

        //Invert
        angle = 360.0f - angle;

        m_Frequency = angle / 360.0f;
    }

    private void UpdateAmplitude()
    {
        //Gather input
        float newAmplitudeX = m_InputManager.GetAxis("Amplitude_X_" + m_DeviceID);
        float newAmplitudeY = m_InputManager.GetAxis("Amplitude_Y_" + m_DeviceID);

        if (m_SwitchedControls)
        {
            newAmplitudeX = m_InputManager.GetAxis("Frequency_X_" + m_DeviceID);
            newAmplitudeY = m_InputManager.GetAxis("Frequency_Y_" + m_DeviceID);
        }

        Vector2 newAmplitudeVector = new Vector2(newAmplitudeX, newAmplitudeY);
        float distance = newAmplitudeVector.magnitude;

        if (distance < 0.5f)
        {
            m_Amplitude = 0.5f;
            return;
        }

        newAmplitudeVector.Normalize();

        if (m_InversedControls)
        {
            newAmplitudeVector.x = 1.0f - newAmplitudeVector.x;
            newAmplitudeVector.y = 1.0f - newAmplitudeVector.y;
        }

        float angle = Mathf.Atan2(newAmplitudeVector.y, newAmplitudeVector.x) * Mathf.Rad2Deg;
        m_AmplitudeAngle = angle;
        angle += 90.0f;

        if (angle < 0.0f) { angle += 360.0f; }
        if (angle > 360.0f) { angle -= 360.0f; }
        
        //Invert
        angle = 360.0f - angle;

        m_Amplitude = angle / 360.0f;
    }

    private void UpdateHacking()
    {
        //Look for a new target
        m_Target = null;

        //Get all the characters
        List<Character> characters = m_CharacterManager.Characters;

		// Reset the minimal difference.
		MinimalDifference = 1;

        float leftVibration = 0.0f;
        float rightVibration = 0.0f;

		foreach (Character character in characters)
        {
            if (character.gameObject == this.gameObject)
            {
                continue;
            }

            //Compare frequency
            bool frequencyHacked = false;
            if (character.Frequency > (m_Frequency - m_ErrorMargin) &&
                character.Frequency < (m_Frequency + m_ErrorMargin))
            {
                frequencyHacked = true;
                leftVibration = 0.25f;
            }

            //Compare amplitude
            bool amplitudeHacked = false;
            if (character.Amplitude > (m_Amplitude - m_ErrorMargin) &&
                character.Amplitude < (m_Amplitude + m_ErrorMargin))
            {
                amplitudeHacked = true;
                rightVibration = 0.25f;
            }

            //We hacked a character!
            if (frequencyHacked && amplitudeHacked)
            {
                character.FullyHacked();
                m_Target = character;

				if (HackedEvent != null)
					HackedEvent();

				leftVibration = 0.5f;
                rightVibration = 0.5f;
            }

            //Almost there, give feedback!
            else if (frequencyHacked || amplitudeHacked)
            {
                character.HalfHacked();

				if (HackedEvent != null)
					HackedEvent();
			}

            else
            {
                character.NotHacked();
            }

			// Get difference in amplitude and frequency with character.
			float difference = Mathf.Abs(m_Frequency - character.Frequency) + Mathf.Abs(m_Amplitude - character.Amplitude);

			if (difference < MinimalDifference)
				MinimalDifference = difference;
		}

		//Debug.Log("Smallest difference: " + m_MinimalDifference);

        if (m_UpdateTargetEvent != null)
            m_UpdateTargetEvent(m_Target);

        ControllerInput.SetVibration(m_DeviceID, leftVibration, rightVibration, 0.5f);
    }

    private void UpdateCommand()
    {
        //Gather input
        m_PrevBrainPowerLeft = m_BrainPowerLeft;
        m_PrevBrainPowerRight = m_BrainPowerRight;

        m_BrainPowerLeft = m_InputManager.GetAxis("BrainPowerLeft_" + m_DeviceID);
        m_BrainPowerRight = m_InputManager.GetAxis("BrainPowerRight_" + m_DeviceID);

        if (m_Target == null)
            return;

        float leftVibration = Mathf.Clamp01(0.5f + (m_BrainPowerLeft * 0.5f));
        float rightVibration = Mathf.Clamp01(0.5f + (m_BrainPowerRight * 0.5f));

        bool useLeftBrainPower = (m_BrainPowerLeft >= 1.0f && (m_BrainPowerLeft != m_PrevBrainPowerLeft));
        bool useRightBrainPower = (m_BrainPowerRight >= 1.0f && (m_BrainPowerRight != m_PrevBrainPowerRight));

        if (useLeftBrainPower == true || useRightBrainPower == true)
        {
            m_Target.SendBrainCommand(useLeftBrainPower, useRightBrainPower);

			// Play hacked sound.
			if (SoundPlayer.Instance != null)
				SoundPlayer.Instance.Hacked.Play();
			else
				Debug.LogWarning("No sound player found, please place it in the scene to get sound.");
		}

        ControllerInput.SetVibration(m_DeviceID, leftVibration, rightVibration, 0.5f);
    }
}
