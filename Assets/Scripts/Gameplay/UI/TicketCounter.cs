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

    [SerializeField]
    private GameObject m_GreenBackground;

    [SerializeField]
    private GameObject m_RedBackground;

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
        m_Text.text = ticket.ToString();

        //Calculate percentage, less than 40% = red
        float percentage = ((float)ticket / (float)maxTickets);
        bool lightRed = (percentage <= 0.4f);

        m_GreenBackground.SetActive(!lightRed);
        m_RedBackground.SetActive(lightRed);
    }
}
