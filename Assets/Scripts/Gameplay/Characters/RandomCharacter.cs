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
    protected SkeletonAnimation m_BuffGuyCharacter;

    [SerializeField]
    protected SkeletonAnimation m_SkinnyGirlCharacter;

    [SerializeField]
    protected SkeletonAnimation m_BuffGirlCharacter;

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

    private bool m_IsSad;

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

    protected override void UpdateAnimation()
    {
        if (m_HasReachedDestination == false)
        {
            if (m_SkeletonAnimation.AnimationName != "walk")
                m_SkeletonAnimation.AnimationName = "walk";

            return;
        }

        if (m_IsCheering == true)
        {
            if (m_SkeletonAnimation.AnimationName != "cheer")
                m_SkeletonAnimation.AnimationName = "cheer";

            return;
        }

        if (m_IsSad == true)
        {
            if (m_SkeletonAnimation.AnimationName != "sad")
                m_SkeletonAnimation.AnimationName = "sad";

            return;
        }

        if (m_IsCalling == true)
        {
            if (m_SkeletonAnimation.AnimationName != "phonecall")
                m_SkeletonAnimation.AnimationName = "phonecall";

            return;
        }

        if (m_SkeletonAnimation.AnimationName == null || m_SkeletonAnimation.AnimationName != "idle")
        {
            m_SkeletonAnimation.AnimationName = "idle";
        }
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

            if (m_Gender == Gender.Female)
            {
                m_CallTimer *= 1.2f;
            }

            //Make sure noboy else start's calling
            if (m_StartCallingEvent != null)
                m_StartCallingEvent();
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

        if (m_Gender == Gender.Male)
        {
            m_WaitToCallTimer *= 1.2f;
        }
    }

    public override void MoveToPositionSequentially(Vector3 position)
    {
        base.MoveToPositionSequentially(position);
        CancelCalling();
    }

    protected override void ExecuteNegativeCommand()
    {
        base.ExecuteNegativeCommand();
        StartCoroutine(NegativeBehaviourCommand());
    }

    private IEnumerator NegativeBehaviourCommand()
    {
        m_IsSad = true;
        yield return new WaitForSeconds(0.5f);
        m_IsSad = false;

        CancelCalling();

        m_MoveSpeed *= 5.0f;
        StartCoroutine(MoveToPositionSequentiallyRoutine(m_SpawnPosition));
    }

    private void RandomizeCharacter()
    {
        m_Gender = (Gender)UnityEngine.Random.Range(0, 2);
        m_BodyType = (BodyType)UnityEngine.Random.Range(0, 2);

        SkeletonAnimation prefab = m_SkinnyGuyCharacter;

        if (m_Gender == Gender.Male && m_BodyType == BodyType.Small)   { prefab = m_SkinnyGuyCharacter; }
        if (m_Gender == Gender.Male && m_BodyType == BodyType.Big)     { prefab = m_BuffGuyCharacter; }
        if (m_Gender == Gender.Female && m_BodyType == BodyType.Small) { prefab = m_SkinnyGirlCharacter; }
        if (m_Gender == Gender.Female && m_BodyType == BodyType.Big)   { prefab = m_BuffGirlCharacter; }

        m_SkeletonAnimation = GameObject.Instantiate(prefab, transform);
        m_SkeletonAnimation.transform.localPosition = Vector3.zero;

        //Visual width
        CharacterStatistics stats = m_SkeletonAnimation.GetComponent<CharacterStatistics>();
        if (stats != null) { m_Width = stats.Width; }

        //Random animation speed
        float rand = UnityEngine.Random.Range(0.5f, 1.5f);
        m_SkeletonAnimation.timeScale = rand;

        RandomizeFrequency();
    }
}
