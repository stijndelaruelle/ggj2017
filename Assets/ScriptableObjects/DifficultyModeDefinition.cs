using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModeDefinition : ScriptableObject
{
    [SerializeField]
    private int m_Tickets;
    public int Tickets
    {
        get { return m_Tickets; }
    }

    [SerializeField]
    private float m_SellTime;
    public float SellTime
    {
        get { return m_SellTime; }
    }

    [SerializeField]
    private float m_TimeGoneAfterHacked;
    public float TimeGoneAfterHacked
    {
        get { return m_TimeGoneAfterHacked; }
    }
}