﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacter : Character
{
    [Header("Random properties")]
    [Space(5)]

    [SerializeField]
    [Tooltip("Disable for debugging")]
    private bool m_RandomizeCharacteristics;

    protected override void Awake()
    {
        base.Awake();

        if (m_RandomizeCharacteristics)
        {
            RandomizeCharacter();
        }
    }

    private void RandomizeCharacter()
    {
        RandomizeGender();
        RandomizeWidth();
        RandomizeFrequency();
    }

    private void RandomizeGender()
    {
        //Random gender
        m_Gender = (Gender)UnityEngine.Random.Range(0, 2);

        //switch (m_Gender)
        //{
        //    case Gender.Male:
        //        m_SpriteRenderer.color = Color.blue;
        //        break;

        //    case Gender.Female:
        //        m_SpriteRenderer.color = Color.red;
        //        break;

        //    default:
        //        break;
        //}
    }

    protected override void ExecuteNegativeCommand()
    {
        base.ExecuteNegativeCommand();

        m_MoveSpeed *= 3.0f;
        StartCoroutine(MoveToPositionSequentiallyRoutine(m_SpawnPosition));
    }

    private void RandomizeWidth()
    {
        //Random width
        m_Width = UnityEngine.Random.Range(0.75f * m_VisualWidth, 1.25f * m_VisualWidth);
        m_SkeletonAnimation.transform.localScale = new Vector3(m_Width / m_VisualWidth, 1.0f, 1.0f);
    }

}
