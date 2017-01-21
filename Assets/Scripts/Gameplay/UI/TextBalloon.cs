using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBalloon : MonoBehaviour
{
    [SerializeField]
    protected Character m_Character;

    [SerializeField]
    protected Text m_Text;

    [SerializeField]
    private float m_TextBalloonActivateTime;
    private float m_TextBalloonActivateTimer;
    private float m_TextBalloonDecativateTimer;

    private void Awake()
    {
        m_Text.gameObject.SetActive(false);
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
                m_Text.gameObject.SetActive(true);
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
                m_Text.gameObject.SetActive(false);
                m_TextBalloonActivateTimer = 0.0f;
                m_TextBalloonDecativateTimer = 0.0f;
            }
        }
    }


    private void OnHalfHacked()
    {
        if (m_Text == null)
            return;

        m_Text.text = "Half.";

        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    private void OnFullHacked()
    {
        if (m_Text == null)
            return;

        m_Text.text = "Hacked!";

        if (m_TextBalloonActivateTimer == 0.0f)
            m_TextBalloonActivateTimer = m_TextBalloonActivateTime;

        m_TextBalloonDecativateTimer = m_TextBalloonActivateTime;
    }

    private void OnNotHacked()
    {
        m_TextBalloonActivateTimer = 0.0f;
    }
}
