using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should be a scriptable object, but no time!
public class CharacterStatistics : MonoBehaviour
{
    [SerializeField]
    private float m_Width;
    public float Width
    {
        get { return m_Width; }
    }

    [SerializeField]
    private float m_Speed;
    public float Speed
    {
        get { return m_Speed; }
    }
}
