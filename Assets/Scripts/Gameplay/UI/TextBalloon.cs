﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBalloon : MonoBehaviour
{
    [SerializeField]
    protected Character m_Character;

	[SerializeField]
	protected GameObject m_TextPanel;

    protected Text m_Text;

    [SerializeField]
    private float m_TextBalloonActivateTime;
    private float m_TextBalloonActivateTimer;
    private float m_TextBalloonDecativateTimer;

	private Tweet m_CharacterTweet;

    private void Awake()
    {
		m_Text = GetComponentInChildren<Text>();
		if (m_Text == null)
			Debug.LogWarning("No text component found!", this);

		if(m_TextPanel == null)
			Debug.LogWarning("No text panel set!", this);

		m_TextPanel.SetActive(false);
    }

    private void Start()
    {
        m_Character.HalfHackedEvent += OnHalfHacked;
        m_Character.FullHackedEvent += OnFullHacked;
        m_Character.NotHackedEvent  += OnNotHacked;
    }

    private void OnDestroy()
    {
        if (m_Character == null)
            return;

        m_Character.HalfHackedEvent -= OnHalfHacked;
        m_Character.FullHackedEvent -= OnFullHacked;
        m_Character.NotHackedEvent  -= OnNotHacked;
    }

    private void Update()
    {
        //Activate
        if (m_TextBalloonActivateTimer > 0.0f)
        {
            m_TextBalloonActivateTimer -= Time.deltaTime;

            if (m_TextBalloonActivateTimer < 0.0f)
            {
				 m_TextPanel.SetActive(true);
            }
        }

        //Deactivate
        if (m_TextBalloonDecativateTimer > 0.0f)
        {
            m_TextBalloonDecativateTimer -= Time.deltaTime;

            //Change alpha
            Color newAlphaColor = m_Text.color;
            newAlphaColor.a = (m_TextBalloonDecativateTimer / m_TextBalloonActivateTime);
            m_Text.color = newAlphaColor;

            if (m_TextBalloonDecativateTimer < 0.0f)
            {
				m_TextPanel.SetActive(false);
                m_TextBalloonActivateTimer = 0.0f;
                m_TextBalloonDecativateTimer = 0.0f;
            }
        }
    }


    private void OnHalfHacked()
    {
        if (m_Text == null)
            return;

		if(m_CharacterTweet == null)
			m_CharacterTweet = Twitter.Instance.GetRandomHashTag().GetRandomTweet();

        m_Text.text = m_CharacterTweet.ScrambledContents;


        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    private void OnFullHacked()
    {
        if (m_Text == null)
            return;

        m_Text.text = m_CharacterTweet.Contents;

        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    private void OnNotHacked()
    {
        m_TextBalloonActivateTimer = 0.0f;
    }
}