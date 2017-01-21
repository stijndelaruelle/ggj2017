using System.Collections;
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

        if (m_Gender == Gender.Female)
        {
            transform.Rotate(0.0f, 0.0f, 180.0f);
        }
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
