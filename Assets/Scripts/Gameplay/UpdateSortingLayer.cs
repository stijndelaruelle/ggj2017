using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class UpdateSortingLayer : MonoBehaviour
{
    private MeshRenderer m_MeshRenderer;

    private void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    private void LateUpdate()
    {
        m_MeshRenderer.sortingOrder = (int)(transform.position.y * 100 * -1);
    }
}
