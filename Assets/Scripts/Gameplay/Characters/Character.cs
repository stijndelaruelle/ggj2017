using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

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
    protected SkeletonAnimation m_SkeletonAnimation;

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
    private bool m_WasOnScreen = false;

    //Events
    private Action<Character> m_RunAwayEvent;
    public Action<Character> RunAwayEvent
    {
        get { return m_RunAwayEvent; }
        set { m_RunAwayEvent = value; }
    }

    private Action<Character> m_DestroyEvent;
    public Action<Character> DestroyEvent
    {
        get { return m_DestroyEvent; }
        set { m_DestroyEvent = value; }
    }

    //Visual events (dave)
    private Action m_NotHackedEvent;
    public Action NotHackedEvent
    {
        get { return m_NotHackedEvent; }
        set { m_NotHackedEvent = value; }
    }

    private Action m_HalfHackedEvent;
    public Action HalfHackedEvent
    {
        get { return m_HalfHackedEvent; }
        set { m_HalfHackedEvent = value; }
    }

    private Action m_FullHackedEvent;
    public Action FullHackedEvent
    {
        get { return m_FullHackedEvent; }
        set { m_FullHackedEvent = value; }
    }

    private Action m_PositiveBrainCommandEvent;
    public Action PositiveBrainCommandEvent
    {
        get { return m_PositiveBrainCommandEvent; }
        set { m_PositiveBrainCommandEvent = value; }
    }

    private Action m_NegativeBrainCommandEvent;
    public Action NegativeBrainCommandEvent
    {
        get { return m_NegativeBrainCommandEvent; }
        set { m_NegativeBrainCommandEvent = value; }
    }

    protected virtual void Awake()
    {
        m_SpawnPosition = transform.position;
        m_TargetPosition = transform.position;
        m_LastPosition = m_TargetPosition;
    }

    protected virtual void Start()
    {
        if (m_SkeletonAnimation.AnimationName == null)
            UpdateAnimation("idle");
    }

    protected virtual void Update()
    {
        UpdateMovement();
        CheckIfInFrame();
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
                UpdateAnimation("idle");
            }
        }
    }

    private void UpdateAnimation(string name)
    {
        m_SkeletonAnimation.AnimationName = name;

        //Facing direction
        Vector3 diff = m_TargetPosition - m_LastPosition;
        float sign = Mathf.Sign(diff.x);

        Vector3 scale = m_SkeletonAnimation.transform.localScale;
        m_SkeletonAnimation.transform.localScale = new Vector3(sign * scale.x, scale.y, scale.z);
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

        UpdateAnimation("walk");
    }

    public void WarpToPosition(Vector3 position)
    {
        m_TargetPosition = position;
        m_LastPosition = m_TargetPosition;

        transform.position = position;
    }

    private void CheckIfInFrame()
    {
        //Transform to viewportspace
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPos.x < -0.1f || viewportPos.x > 1.1f ||
            viewportPos.y < -0.1f || viewportPos.y > 1.1f)
        {
            if (m_WasOnScreen == true)
            {
                if (m_DestroyEvent != null)
                    m_DestroyEvent(this);
            }
        }
        else
        {
            m_WasOnScreen = true;
        }
    }

    protected IEnumerator MoveToPositionSequentiallyRoutine(Vector3 position)
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

    //Hacking feedback
    public void HalfHacked()
    {
        if (m_HalfHackedEvent != null)
            m_HalfHackedEvent();
    }

    public void FullyHacked()
    {
        if (m_FullHackedEvent != null)
            m_FullHackedEvent();
    }

    public void NotHacked()
    {
        if (m_NotHackedEvent != null)
            m_NotHackedEvent();
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

    protected virtual void ExecutePositiveCommand()
    {
        if (m_PositiveBrainCommandEvent != null)
            m_PositiveBrainCommandEvent();
    }

    protected virtual void ExecuteNegativeCommand()
    {
        if (m_NegativeBrainCommandEvent != null)
            m_NegativeBrainCommandEvent();
    }

    //Randomize
    protected void RandomizeFrequency()
    {
        //Frequency is dependant on the width
        //Big characters have a lower range than thin characters
        m_Frequency = UnityEngine.Random.Range(0.05f, 1.0f);

        //Amplitude is dependent on the gender
        switch (m_Gender)
        {
            case Gender.Male:
                m_Amplitude = UnityEngine.Random.Range(0.05f, 0.49f);
                break;

            case Gender.Female:
                m_Amplitude = UnityEngine.Random.Range(0.51f, 0.95f);
                break;

            default:
                break;
        }
    }
}
