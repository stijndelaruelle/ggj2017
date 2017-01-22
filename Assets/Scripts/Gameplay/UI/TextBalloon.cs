using System.Collections;
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
    private bool m_IsHacking = false;
    private float m_ScrambleTimer;

    [SerializeField]
    private float m_ScrambleFrequency;

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
    }

    private void OnDestroy()
    {
        if (m_Character == null)
            return;

        m_Character.HalfHackedEvent -= OnHalfHacked;
        m_Character.FullHackedEvent -= OnFullHacked;
    }

    private void LateUpdate()
    {
        if (m_IsHacking == false)
        {
            m_TextBalloonActivateTimer = 0.0f;
        }
        m_IsHacking = false;

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

        //SCramble
        m_ScrambleTimer -= Time.deltaTime;
    }


    private void OnHalfHacked()
    {
        m_IsHacking = true;

        if (m_Text == null)
            return;

		if(m_CharacterTweet == null)
        {
            if (Twitter.Instance != null)
            {
                m_CharacterTweet = Twitter.Instance.GetRandomHashTag().GetRandomTweet();
            }
        }


        if (m_ScrambleTimer < 0.0f)
        {
            m_CharacterTweet.Scramble();
            m_ScrambleTimer = m_ScrambleFrequency;
        }

        m_Text.text = m_CharacterTweet.ScrambledContents;


        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    private void OnFullHacked()
    {
        m_IsHacking = true;

        if (m_Text == null)
            return;

        m_Text.text = m_CharacterTweet.Contents;

        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }
}
