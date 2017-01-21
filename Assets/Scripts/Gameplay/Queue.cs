using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [Header("Queue properties")]
    [Space(5)]
    [SerializeField]
    private int m_NumberOfCharacters;

    [Space(10)]
    [Header("Required references")]
    [Space(5)]
    [SerializeField]
    private Transform m_StartPosition;

    [SerializeField]
    private Transform m_EndPosition;

    [SerializeField]
    private Transform m_CachierPosition;
    private CachierCharacter m_Cachier;

    [SerializeField]
    private CharacterManager m_CharacterManager;
    private List<Character> m_Characters;

    private void Start()
    {
        m_Characters = new List<Character>();

        //Spawn characters
        for (int i = 0; i < m_NumberOfCharacters; ++i)
        {
            Character character = m_CharacterManager.SpawnRandomCharacterAtPosition(m_StartPosition);
            if (character != null) { Insert(m_Characters.Count, character, true); }
        }

        //Spawn cachier
        m_Cachier = m_CharacterManager.SpawnCachier(m_EndPosition);
        m_Cachier.WarpToPosition(m_CachierPosition.position);
        m_Cachier.SellTicketEvent += OnSellTicket;
    }

    private void OnDestroy()
    {
        if (m_Characters != null)
        {
            foreach (Character character in m_Characters)
            {
                character.RunAwayEvent -= OnCharacterRunAway;
            }
        }

        if (m_Cachier != null)
        {
            m_Cachier.SellTicketEvent -= OnSellTicket;
        }
    }

    private void Insert(int position, Character character, bool warp = false)
    {
        //If we insert the character at the back
        if (position >= m_Characters.Count)
        {
            Vector3 targetPosition = transform.position; //Spawn location
            if (m_Characters.Count > 0)
            {
                targetPosition = m_Characters[m_Characters.Count - 1].GamePosition;
                targetPosition.x -= (m_Characters[m_Characters.Count - 1].Width * 0.5f);
                targetPosition.x -= (character.Width * 0.5f);
            }

            if (warp) { character.WarpToPosition(targetPosition); }
            else      { character.MoveToPosition(targetPosition); }

            m_Characters.Add(character);
        }

        //If we insert the character at the beginning or in the middle
        else
        {
            //Get the current character at this position
            Character oldCharacter = m_Characters[position];

            //Move the new character to his position
            character.MoveToPosition(oldCharacter.GamePosition);

            //All characters have to move backwards (with the width of the new character)
            for (int i = position; i < m_Characters.Count; ++i)
            {
                Vector3 targetPosition = m_Characters[i].GamePosition;
                targetPosition.x -= character.Width;

                m_Characters[i].MoveToPosition(targetPosition);
            }

            m_Characters.Insert(position, character);
        }

        //Start listening to his events
        character.RunAwayEvent += OnCharacterRunAway;
    }

    private void Remove(Character character)
    {
        int index = m_Characters.IndexOf(character);

        if (index != -1)
            Remove(index);
    }

    private void Remove(int position)
    {
        if (m_Characters.Count == 0)
            return;

        if (position >= m_Characters.Count)
            position = m_Characters.Count - 1;

        //Stop listening to his events
        Character oldCharacter = m_Characters[position];
        oldCharacter.RunAwayEvent -= OnCharacterRunAway;

        //All characters behind this character move forward with the width
        m_Characters.RemoveAt(position);

        StartCoroutine(RemoveMovementSequentailly(position, oldCharacter));
    }

    private void OnCharacterRunAway(Character character)
    {
        Remove(character);
    }

    private void OnSellTicket()
    {
        m_Characters[0].MoveToPositionSequentially(m_EndPosition.position);
    }

    //Sequential movement (looks nicer
    private IEnumerator RemoveMovementSequentailly(int position, Character oldCharacter)
    {
        //All characters have to move forwards (with the width of the old character)
        for (int i = position; i < m_Characters.Count; ++i)
        {
            Vector3 targetPosition = m_Characters[i].GamePosition;
            targetPosition.x += oldCharacter.Width;

            m_Characters[i].MoveToPosition(targetPosition);

            float randTime = UnityEngine.Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(randTime);
        }
    }
}
