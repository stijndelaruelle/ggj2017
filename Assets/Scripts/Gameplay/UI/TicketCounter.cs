using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicketCounter : MonoBehaviour
{
    [SerializeField]
    private CachierCharacter m_Cachier;

    [SerializeField]
    private Text m_Text;

    private void Awake()
    {
        //Subscribe to the cachier events
        m_Cachier.ChangeTicketsEvent += OnChangeTickets;
    }

    private void OnDestroy()
    {
        if (m_Cachier != null)
            m_Cachier.ChangeTicketsEvent -= OnChangeTickets;
    }

    private void OnChangeTickets(int ticket, int maxTickets)
    {
        m_Text.text = ticket + " / " + maxTickets;
    }
}
