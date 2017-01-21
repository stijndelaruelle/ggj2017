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
    private float m_Tickets = 20;
    private float m_TicketsLeft = 1;

    [SerializeField]
    private float m_TimeGoneAfterHacked = 2;
    private bool m_IsGone = false;

    //Events
    private Action m_SellTicketEvent;
    public Action SellTicketEvent
    {
        get { return m_SellTicketEvent; }
        set { m_SellTicketEvent = value; }
    }

    protected override void Update()
    {
        base.Update();

        UpdateSelling();
    }

    private void UpdateSelling()
    {
        if (m_IsGone)
            return;

        m_SellTimer -= Time.deltaTime;

        if (m_SellTimer <= 0.0f)
        {
            SellTicket();
            m_SellTimer = UnityEngine.Random.Range(m_MinSellTime, m_MaxSellTime);
        }
    }

    private void SellTicket()
    {
        m_Tickets -= 1;

        if (m_SellTicketEvent != null)
            m_SellTicketEvent();
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
