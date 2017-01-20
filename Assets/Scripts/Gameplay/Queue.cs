using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [SerializeField]
    private Transform m_StartPosition;

    [SerializeField]
    private List<Character> m_Characters;

    private void Start()
    {
        //Set all the characters in the right place
        Vector3 nextPosition = m_StartPosition.position;

        foreach (Character character in m_Characters)
        {
            character.SetPosition(nextPosition);
            nextPosition.x -= character.Width;
        }
    }
}
