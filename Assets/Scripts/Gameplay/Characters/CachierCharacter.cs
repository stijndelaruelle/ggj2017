using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachierCharacter : Character
{
    [Header("Cachier properties")]
    [Space(5)]

    [SerializeField]
    private DifficultyModeDefinition m_EasyGameMode;

    [SerializeField]
    private DifficultyModeDefinition m_HardGameMode;
    private DifficultyModeDefinition m_CurrentGameMode;

    private float m_SellTimer;
    private int m_TicketsLeft = 1;
    private bool m_IsGone = false;
    public bool IsGone
    {
        get { return m_IsGone; }
    }

    [SerializeField]
    private Transform m_StandPosition;

    [SerializeField]
    List<ParticleSystem> CashMoneyEmitters = new List<ParticleSystem>();

    //Events
    private Action<int, int> m_ChangeTicketEvent;
    public Action<int, int> ChangeTicketsEvent
    {
        get { return m_ChangeTicketEvent; }
        set { m_ChangeTicketEvent = value; }
    }

    private Action<int> m_SellTicketEvent;
    public Action<int> SellTicketEvent
    {
        get { return m_SellTicketEvent; }
        set { m_SellTicketEvent = value; }
    }

    private void Start()
    {
        m_CurrentGameMode = m_EasyGameMode;
        if (PlayerPrefs.GetInt("DifficultyMode") > 0)
        {
            m_CurrentGameMode = m_HardGameMode;
        }

        m_TicketsLeft = m_CurrentGameMode.Tickets;
        FireChangeTicketEvent();

        RandomizeFrequency();

        StartCoroutine(ComeBackRoutine(m_StandPosition.position));
    }

    protected override void Update()
    {
        base.Update();

        UpdateSelling();
    }

    private void UpdateSelling()
    {
        if (m_IsGone || m_TicketsLeft <= 0)
            return;

        m_SellTimer -= Time.deltaTime;

        if (m_SellTimer <= 0.0f)
        {
            SellTicket();
            ResetSellTimer();
        }
    }

    public float GetNormalizedSellTimer()
    {
        return (m_SellTimer / m_CurrentGameMode.SellTime);
    }

    private void SellTicket()
    {
        if (m_TicketsLeft <= 0)
            return;

        m_TicketsLeft -= 1;

        if (m_SellTicketEvent != null)
            m_SellTicketEvent(m_TicketsLeft);

        FireChangeTicketEvent();
        foreach(ParticleSystem sys in CashMoneyEmitters) {
            sys.Emit(5);
        }
    }

    public void ResetSellTimer()
    {
        m_SellTimer = m_CurrentGameMode.SellTime; //UnityEngine.Random.Range(m_MinSellTime, m_MaxSellTime);
    }

    public void SetSellTimer(float value)
    {
        m_SellTimer = value;
    }

    private void FireChangeTicketEvent()
    {
        if (m_ChangeTicketEvent != null)
            m_ChangeTicketEvent(m_TicketsLeft, m_CurrentGameMode.Tickets);
    }

    protected override void ExecuteNegativeCommand()
    {
        if(m_IsGone) return;
        base.ExecuteNegativeCommand();
        StartCoroutine(MoveAndComeBackRoutine(m_SpawnPosition));
    }

    public void Leave()
    {
        m_IsGone = true;

        //Move to the correct Y position
        Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        MoveToPosition(m_SpawnPosition);
    }

    private IEnumerator ComeBackRoutine(Vector3 position)
    {
        m_IsGone = true;

        MoveToPosition(position);

        while (m_HasReachedDestination == false)
        {
            yield return new WaitForEndOfFrame();
        }

        m_IsGone = false;
        ResetSellTimer();

        yield return null;
    }

    private IEnumerator MoveAndComeBackRoutine(Vector3 position)
    {
        m_IsGone = true;
        //Jump in the air

        //Move to the correct Y position
        Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        MoveToPosition(position);

        while (m_HasReachedDestination == false)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(m_CurrentGameMode.TimeGoneAfterHacked);

        MoveToPosition(oldPosition);

        while (m_HasReachedDestination == false)
        {
            yield return new WaitForEndOfFrame();
        }

        m_IsGone = false;
        ResetSellTimer();

        yield return null;
    }
}
