using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacter : Character
{
    [Header("Random properties")]
    [Space(5)]

    [SerializeField]
    [Tooltip("Disable for debugging")]
    private bool m_RandomizeCharacteristics;

    [SerializeField]
    protected SkeletonAnimation m_SkinnyGuyCharacter;

    [SerializeField]
    protected SkeletonAnimation m_SkinnyGirlCharacter;

    [Space(10)]
    [Header("Call a friend")]
    [Space(5)]
    [SerializeField]
    private float m_MinWaitToCallTimer;
    [SerializeField]
    private float m_MaxWaitToCallTimer;

    [SerializeField]
    private float m_MinCallTimer;
    [SerializeField]
    private float m_MaxCallTimer;

    private float m_WaitToCallTimer = 0.0f;
    private float m_CallTimer = 0.0f;

    private bool m_IsCalling = false;
    private bool m_AllowCalling = true;
    public bool AllowCalling
    {
        get { return m_AllowCalling; }
        set
        {
            m_AllowCalling = value;
            ResetWaitToCallTimer();
        }
    }

    //Events
    private Action m_StartCallingEvent;
    public Action StartCallingEvent
    {
        get { return m_StartCallingEvent; }
        set { m_StartCallingEvent = value; }
    }

    private Action m_CancelCallingEvent;
    public Action CancelCallingEvent
    {
        get { return m_CancelCallingEvent; }
        set { m_CancelCallingEvent = value; }
    }

    private Action<Character> m_StopCallingEvent;
    public Action<Character> StopCallingEvent
    {
        get { return m_StopCallingEvent; }
        set { m_StopCallingEvent = value; }
    }

    protected override void Awake()
    {
        base.Awake();

        ResetWaitToCallTimer();

        if (m_RandomizeCharacteristics)
        {
            RandomizeCharacter();
        }
    }

    protected override void Update()
    {
        base.Update();

        UpdateWaitToCall();
        UpdateCalling();
    }

    private void UpdateWaitToCall()
    {
        //Waiting to call
        if (!m_AllowCalling || m_IsCalling == true)
            return;

        m_WaitToCallTimer -= Time.deltaTime;

        if (m_WaitToCallTimer < 0.0f)
        {
            m_IsCalling = true;
            m_CallTimer = UnityEngine.Random.Range(m_MinCallTimer, m_MaxCallTimer);

            //Make sure noboy else start's calling
            if (m_StartCallingEvent != null)
                m_StartCallingEvent();

            //transform.Rotate(0.0f, 0.0f, 180.0f);
        }
    }

    private void UpdateCalling()
    {
        //Actually calling
        if (m_IsCalling)
        {
            m_CallTimer -= Time.deltaTime;

            if (m_CallTimer < 0.0f)
            {
                if (m_StopCallingEvent != null)
                    m_StopCallingEvent(this);

                m_IsCalling = false;
                ResetWaitToCallTimer();
            }
        }
    }


    private void CancelCalling()
    {
        m_IsCalling = false;

        if (m_CancelCallingEvent != null)
            m_CancelCallingEvent();
    }

    private void ResetWaitToCallTimer()
    {
        m_WaitToCallTimer = UnityEngine.Random.Range(m_MinWaitToCallTimer, m_MaxWaitToCallTimer);
    }

    public override void MoveToPositionSequentially(Vector3 position)
    {
        base.MoveToPositionSequentially(position);
        CancelCalling();
    }

    protected override void ExecuteNegativeCommand()
    {
        base.ExecuteNegativeCommand();
        CancelCalling();

        m_MoveSpeed *= 3.0f;
        StartCoroutine(MoveToPositionSequentiallyRoutine(m_SpawnPosition));
    }

    private void RandomizeCharacter()
    {
        RandomizeGender();
        RandomizeBodyType();
        RandomizeFrequency();
    }

    private void RandomizeGender()
    {
        //Random gender
        m_Gender = (Gender)UnityEngine.Random.Range(0, 2);

        switch (m_Gender)
        {
            case Gender.Male:
                m_SkeletonAnimation = GameObject.Instantiate(m_SkinnyGuyCharacter, transform);
                m_SkeletonAnimation.transform.localPosition = Vector3.zero;
                break;

            case Gender.Female:
                m_SkeletonAnimation = GameObject.Instantiate(m_SkinnyGirlCharacter, transform);
                m_SkeletonAnimation.transform.localPosition = Vector3.zero;
                break;

            default:
                break;
        }
    }

    private void RandomizeBodyType()
    {
        m_BodyType = (BodyType)UnityEngine.Random.Range(0, 2);

        //Visual width
        CharacterStatistics stats = m_SkeletonAnimation.GetComponent<CharacterStatistics>();
        if (stats != null) { m_Width = stats.Width; }
    }

}
