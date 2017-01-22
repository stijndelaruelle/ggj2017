using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateSortingLayer : MonoBehaviour
{
    [SerializeField]
    private int m_Offset = 0;

    private Renderer m_Renderer;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        if (m_Offset> 0)
        {
            //Debug.Log("");
        }
        m_Renderer.sortingOrder = (int)(transform.position.y * 100 * -1) + m_Offset;
    }
}
