using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachierCharacter : Character
{
    [Header("Cachier properties")]
    [Space(5)]

    [SerializeField]
    private float m_MinSellTime;

    [SerializeField]
    private float m_MaxSellTime;
    private float m_SellTimer;

    [SerializeField]
    private int m_Tickets = 20;
    private int m_TicketsLeft = 1;

    [SerializeField]
    private float m_TimeGoneAfterHacked = 2;
    private bool m_IsGone = false;

    //Events
    private Action<int, int> m_ChangeTicketEvent;
    public Action<int, int> ChangeTicketsEvent
    {
        get { return m_ChangeTicketEvent; }
        set { m_ChangeTicketEvent = value; }
    }

    private void Start()
    {
        ResetSellTimer();

        m_TicketsLeft = m_Tickets;
        FireChangeTicketEvent();
    }

    protected override void Update()
    {
        base.Update();

        UpdateSelling();
    }

    private void UpdateSelling()
    {
        if (m_IsGone || m_Tickets <= 0)
            return;

        m_SellTimer -= Time.deltaTime;

        if (m_SellTimer <= 0.0f)
        {
            SellTicket();
            ResetSellTimer();
        }
    }

    private void SellTicket()
    {
        m_TicketsLeft -= 1;
        FireChangeTicketEvent();
    }

    private void ResetSellTimer()
    {
        m_SellTimer = UnityEngine.Random.Range(m_MinSellTime, m_MaxSellTime);
    }

    private void FireChangeTicketEvent()
    {
        if (m_ChangeTicketEvent != null)
            m_ChangeTicketEvent(m_TicketsLeft, m_Tickets);
    }

    protected override void ExecuteNegativeCommand()
    {
        m_TextBalloon.text = "NEGATIVE";
        StartCoroutine(MoveAndComeBackRoutine(m_SpawnPosition));
    }

    private IEnumerator MoveAndComeBackRoutine(Vector3 position)
    {
        m_IsGone = true;
        //Jump in the air

        //Move to the correct Y position
        Vector3 oldPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        MoveToPosition(position);

        yield return new WaitForSeconds(m_TimeGoneAfterHacked);

        MoveToPosition(oldPosition);

        m_IsGone = false;

        yield return null;
    }
}
