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
    [SerializeField]
    protected Gender m_Gender;

    [SerializeField]
    protected float m_Width;
    public float Width
    {
        get { return m_Width; }
    }

    [SerializeField]
    protected float m_Frequency = -1;
    public float Frequency
    {
        get { return m_Frequency; }
    }

    [SerializeField]
    protected float m_Amplitude = -1;
    public float Amplitude
    {
        get { return m_Amplitude; }
    }

    [SerializeField]
    protected bool m_UseLeftBrain;
    public bool UseLeftBrain
    {
        get { return m_UseLeftBrain; }
    }

    [SerializeField]
    protected bool m_UseRightBrain;
    public bool UseRightBrain
    {
        get { return m_UseRightBrain; }
    }

    [SerializeField]
    protected SpriteRenderer m_SpriteRenderer;

    //Movement (will later all be calculated according to the width
    [Space(10)]
    [Header("Movement")]
    [Space(5)]
    [Tooltip ("Seconds per unit")]
    [SerializeField]
    protected float m_MoveSpeed = 1.0f;
    private float m_CurrentMoveSpeed; //caches distance calculations

    protected Vector3 m_SpawnPosition;
    protected Vector3 m_LastPosition;
    protected Vector3 m_TargetPosition;
    public Vector3 GamePosition
    {
        get { return m_TargetPosition; }
    }

    private float m_LerpTimer = 0.0f;
    protected bool m_HasReachedDestination = false;

    //Textballoon feedback
    [Space(10)]
    [Header("Thinking balloon")]
    [Space(5)]
    [SerializeField]
    protected Text m_TextBalloon;

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

    protected virtual void Awake()
    {
        m_SpawnPosition = transform.position;
        m_TargetPosition = transform.position;
        m_LastPosition = m_TargetPosition;

        m_TextBalloon.gameObject.SetActive(false);
    }

    protected virtual void Update()
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

    public void MoveToPositionSequentially(Vector3 position)
    {
        m_MoveSpeed *= 2.0f;
        StartCoroutine(MoveToPositionSequentiallyRoutine(position));
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

    public void NotHacked()
    {
        m_TextBalloonActivateTimer = 0.0f;
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

    protected virtual void ExecuteNegativeCommand()
    {
        m_TextBalloon.text = "NEGATIVE";

        m_MoveSpeed *= 3.0f;
        StartCoroutine(MoveToPositionSequentiallyRoutine(m_SpawnPosition));
    }

    private IEnumerator MoveToPositionSequentiallyRoutine(Vector3 position)
    {
        //Jump in the air

        //Move to the correct Y position
        Vector3 newPosition = new Vector3(transform.position.x, position.y, transform.position.z);
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
        newPosition = position;

        MoveToPosition(newPosition);

        yield return null;
    }


    //Randomize
    protected void RandomizeFrequency()
    {
        //Frequency is dependant on the width
        //Big characters have a lower range than thin characters

        m_Frequency = UnityEngine.Random.Range(0.05f, 1.0f);

        //Amplitude is dependant on the gender
        float minRadius = 0.05f;
        float maxRange = 0.95f;

        switch (m_Gender)
        {
            case Gender.Male:

                break;

            case Gender.Female:
                break;

            default:
                break;
        }

        m_Amplitude = UnityEngine.Random.Range(0.05f, 1.0f);
    }
}
