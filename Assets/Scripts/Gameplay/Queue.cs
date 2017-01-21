﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [SerializeField]
    private int m_NumberOfCharacters;

    [SerializeField]
    private Transform m_SpawnLocation;

    [SerializeField]
    private CharacterManager m_CharacterManager;
    private List<Character> m_Characters;

    private void Start()
    {
        m_Characters = new List<Character>();

        for (int i = 0; i < m_NumberOfCharacters; ++i)
        {
            Character character = m_CharacterManager.SpawnCharacter(m_SpawnLocation);
            if (character != null) { Insert(m_Characters.Count, character, true); }
        }
    }

    private void OnDestroy()
    {
        if (m_Characters == null)
            return;

        foreach (Character character in m_Characters)
        {
            character.RunAwayEvent -= OnCharacterRunAway;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Spawn a new random character
            Character character = m_CharacterManager.SpawnCharacter(m_SpawnLocation);
            if (character != null)
                Insert(0, character);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Character character = m_CharacterManager.SpawnCharacter(m_SpawnLocation);
            if (character != null)
                Insert((int)(m_Characters.Count * 0.5f), character);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Remove(m_Characters.Count - 1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Remove(0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Remove((int)(m_Characters.Count * 0.5f));
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

        //Get the current character at this position
        Character oldCharacter = m_Characters[position];

        //All characters have to move forwards (with the width of the old character)
        for (int i = position + 1; i < m_Characters.Count; ++i)
        {
            Vector3 targetPosition = m_Characters[i].GamePosition;
            targetPosition.x += oldCharacter.Width;

            m_Characters[i].MoveToPosition(targetPosition);
        }

        //Stop listening to his events
        m_Characters[position].RunAwayEvent -= OnCharacterRunAway;

        //All characters behind this character move forward with the width
        m_Characters.RemoveAt(position);
    }

    private void OnCharacterRunAway(Character character)
    {
        Remove(character);
    }
}
