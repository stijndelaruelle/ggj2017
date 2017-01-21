using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gender
{
    Male,
    Female
}

public class Character : MonoBehaviour
{
    [Header("Random properties")]
    [Space(5)]

    [SerializeField]
    [Tooltip("Disable for debugging")]
    private bool m_RandomizeCharacteristics;
    [Space(10)]

    [SerializeField]
    private Gender m_Gender;

    [SerializeField]
    private float m_Width;
    public float Width
    {
        get { return m_Width; }
    }

    [SerializeField]
    private float m_Frequency;
    public float Frequency
    {
        get { return m_Frequency; }
    }

    [SerializeField]
    private float m_Amplitude;
    public float Amplitude
    {
        get { return m_Amplitude; }
    }

    [SerializeField]
    private bool m_UseLeftBrain;
    public bool UseLeftBrain
    {
        get { return m_UseLeftBrain; }
    }

    [SerializeField]
    private bool m_UseRightBrain;
    public bool UseRightBrain
    {
        get { return m_UseRightBrain; }
    }

    [SerializeField]
    private SpriteRenderer m_SpriteRenderer;

    //Movement (will later all be calculated according to the width
    [Space(10)]
    [Header("Movement")]
    [Space(5)]
    [Tooltip ("Seconds per unit")]
    [SerializeField]
    private float m_MoveSpeed = 1.0f;
    private float m_CurrentMoveSpeed; //caches distance calculations

    private Vector3 m_SpawnPosition;
    private Vector3 m_LastPosition;
    private Vector3 m_TargetPosition;
    public Vector3 GamePosition
    {
        get { return m_TargetPosition; }
    }

    private float m_LerpTimer = 0.0f;
    private bool m_HasReachedDestination = false;

    //Textballoon feedback
    [Space(10)]
    [Header("Thinking balloon")]
    [Space(5)]
    [SerializeField]
    private Text m_TextBalloon;

    [SerializeField]
    private float m_TextBalloonActivateTime;
    private float m_TextBalloonActivateTimer;
    private float m_TextBalloonDecativateTimer;

    //Events
    private Action<Character> m_RunAwayEvent;
    public Action<Character> RunAwayEvent
    {
        get { return m_RunAwayEvent; }
        set { m_RunAwayEvent = value; }
    }

    private void Awake()
    {
        m_SpawnPosition = transform.position;
        m_TargetPosition = transform.position;
        m_LastPosition = m_TargetPosition;

        m_TextBalloon.gameObject.SetActive(false);

        if (m_RandomizeCharacteristics)
        {
            RandomizeCharacter();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ExecuteNegativeCommand();
        }

        UpdateMovement();
        UpdateTextBalloon();
    }

    private void UpdateMovement()
    {
        m_HasReachedDestination = (m_LerpTimer >= 1.0f);

        if (m_LerpTimer < 1.0f)
        {
            //Handle walking
            m_LerpTimer += m_CurrentMoveSpeed * Time.deltaTime;
            if (m_LerpTimer > 1.0f) { m_LerpTimer = 1.0f; }

            transform.position = Vector3.Lerp(m_LastPosition, m_TargetPosition, m_LerpTimer);

            //We finished walking
            if (m_LerpTimer >= 1.0f)
            {
                m_LastPosition = m_TargetPosition;
            }
        }
    }

    private void UpdateTextBalloon()
    {
        //Activate
        if (m_TextBalloonActivateTimer > 0.0f)
        {
            m_TextBalloonActivateTimer -= Time.deltaTime;

            if (m_TextBalloonActivateTimer < 0.0f)
            {
                m_TextBalloon.gameObject.SetActive(true);
            }
        }

        //Deactivate
        if (m_TextBalloonDecativateTimer > 0.0f)
        {
            m_TextBalloonDecativateTimer -= Time.deltaTime;

            //Change alpha
            Color newAlphaColor = m_TextBalloon.color;
            newAlphaColor.a = (m_TextBalloonDecativateTimer / m_TextBalloonActivateTime);
            m_TextBalloon.color = newAlphaColor;

            if (m_TextBalloonDecativateTimer < 0.0f)
            {
                m_TextBalloon.gameObject.SetActive(false);
                m_TextBalloonActivateTimer = 0.0f;
                m_TextBalloonDecativateTimer = 0.0f;
            }
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        m_HasReachedDestination = false;

        m_LastPosition = transform.position;
        m_TargetPosition = position;
        m_LerpTimer = 0.0f;

        float distance = (m_TargetPosition - m_LastPosition).magnitude;
        m_CurrentMoveSpeed = (m_MoveSpeed / distance);
    }

    public void WarpToPosition(Vector3 position)
    {
        m_TargetPosition = position;
        m_LastPosition = m_TargetPosition;

        transform.position = position;
    }

    private void CheckIfInFrame()
    {

    }

    //Hacking feedback
    public void HalfHacked()
    {
        m_TextBalloon.text = "Half hacked.";

        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    public void FullyHacked()
    {
        m_TextBalloon.text = "Hacked!";

        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    //Brain commands
    public void SendBrainCommand(bool leftBrain, bool rightBrain)
    {
        if (leftBrain == false && rightBrain == false)
            return;

        RandomizeFrequency();

        //Negative commands
        if ((m_UseLeftBrain != leftBrain) || (m_UseRightBrain != rightBrain))
        {
            ExecuteNegativeCommand();
            return;
        }

        //Positive commands
        if ((m_UseLeftBrain == leftBrain) || (m_UseRightBrain == rightBrain))
        {
            ExecutePositiveCommand();
            return;
        }
    }

    private void ExecutePositiveCommand()
    {
        m_TextBalloon.text = "POSITIVE";
    }

    private void ExecuteNegativeCommand()
    {
        m_TextBalloon.text = "NEGATIVE";
        StartCoroutine(RunAwayRoutine());
    }

    private IEnumerator RunAwayRoutine()
    {
        //Jump in the air

        //Double dash!
        m_MoveSpeed *= 2.0f;

        //Move to the correct Y position
        Vector3 newPosition = new Vector3(transform.position.x, m_SpawnPosition.y, transform.position.z);
        MoveToPosition(newPosition);

        while (m_HasReachedDestination == false)
        {
            yield return new WaitForEndOfFrame();
        }

        //Look left & right

        //Get out of the queue (if we were in any)
        if (m_RunAwayEvent != null)
            m_RunAwayEvent(this);

        //Run towards where we came from
        newPosition = m_SpawnPosition;

        MoveToPosition(newPosition);

        yield return null;
    }


    //Randomize
    private void RandomizeCharacter()
    {
        RandomizeGender();
        RandomizeWidth();
        RandomizeFrequency();
    }

    private void RandomizeGender()
    {
        //Random gender
        m_Gender = (Gender)UnityEngine.Random.Range(0, 2);

        switch (m_Gender)
        {
            case Gender.Male:
                m_SpriteRenderer.color = Color.blue;
                break;

            case Gender.Female:
                m_SpriteRenderer.color = Color.red;
                break;

            default:
                break;
        }
    }

    private void RandomizeWidth()
    {
        //Random width
        m_Width = UnityEngine.Random.Range(0.5f, 2.0f);
        m_SpriteRenderer.transform.localScale = new Vector3(m_Width, 1.0f, 1.0f);
    }

    private void RandomizeFrequency()
    {
        m_Frequency = UnityEngine.Random.Range(0.0f, 1.0f);
        m_Amplitude = UnityEngine.Random.Range(0.0f, 1.0f);
    }
}
