using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float m_Frequency;

    [SerializeField]
    private string m_HashTag;

    [SerializeField]
    private float m_Width;
    public float Width
    {
        get { return m_Width; }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
