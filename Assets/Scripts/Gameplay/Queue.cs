﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    [Header("Queue properties")]
    [Space(5)]
    [SerializeField]
    private int m_NumberOfCharacters;

    [SerializeField]
    private CharacterManager m_CharacterManager;

    [Space(10)]
    [Header("Special characters")]
    [Space(5)]
    [SerializeField]
    private PlayerCharacter m_Player;

    [SerializeField]
    private CachierCharacter m_Cachier;

    [Space(10)]
    [Header("Special positions")]
    [Space(5)]
    [SerializeField]
    private Transform m_StartPosition;

    [SerializeField]
    private Transform m_EndPosition;
    
    [Space(10)]
    [Header("Game over windows")]
    [Space(5)]
    [SerializeField]
    private GameObject GameoverWin;
    [SerializeField]
    private GameObject GameoverLoss;

    private List<Character> m_Characters;
    private List<RandomCharacter> m_RandomCharacters;
    private bool m_GameEnd = false;

    private Coroutine m_RemoveMovementSequentaillyRoutine;
    public static Queue Instance; 
    private void Awake()
    {
		if (Instance == null)
			Instance = this;
		else
			Debug.LogWarning("Multiple instance of queue in this scene!");

        //Subscribe to the cachier events
        m_Cachier.SellTicketEvent += OnSellTicket;
    }

    private void Start()
    {
        m_Characters = new List<Character>();
        m_RandomCharacters = new List<RandomCharacter>();

        //Spawn characters
        for (int i = 0; i < m_NumberOfCharacters; ++i)
        {
            RandomCharacter character = m_CharacterManager.SpawnRandomCharacterAtPosition(m_StartPosition.position);
            if (character != null)
            {
                //Subscribe to events
                character.StartCallingEvent += OnCharacterStartCalling;
                character.CancelCallingEvent += OnCharacterCancelCalling;
                character.StopCallingEvent += OnCharacterStopCalling;

                Insert(m_Characters.Count, character, true);
                m_RandomCharacters.Add(character);
            }
        }

        //Add player to the back of the queue
        Insert(m_Characters.Count, m_Player, true);
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

        if (m_RandomCharacters != null)
        {
            foreach (RandomCharacter character in m_RandomCharacters)
            {
                character.StartCallingEvent -= OnCharacterStartCalling;
                character.CancelCallingEvent -= OnCharacterCancelCalling;
                character.StopCallingEvent -= OnCharacterStopCalling;
            }
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

                if (i < (m_Characters.Count - 1))
                {
                    targetPosition = m_Characters[i + 1].GamePosition;
                }

                m_Characters[i].MoveToPosition(targetPosition);
            }

            m_Characters.Insert(position, character);
        }

        //Start listening to his events
        character.RunAwayEvent += OnCharacterRunAway;
    }

    private void Remove(Character character)
    {
        if (character == m_Player)
            return;

        int index = m_Characters.IndexOf(character);

        if (index != -1)
        {
            Remove(index);

            //So dirty, I never want to look back
            RandomCharacter randomCharacter = (RandomCharacter)character;
            if (m_RandomCharacters.Contains(randomCharacter))
            {
                randomCharacter.StartCallingEvent -= OnCharacterStartCalling;
                randomCharacter.CancelCallingEvent -= OnCharacterCancelCalling;
                randomCharacter.StopCallingEvent -= OnCharacterStopCalling;

                m_RandomCharacters.Remove(randomCharacter);
            }
        }
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
        if (m_RemoveMovementSequentaillyRoutine != null)
            StopCoroutine(m_RemoveMovementSequentaillyRoutine);

        m_Characters.RemoveAt(position);
        m_RemoveMovementSequentaillyRoutine = StartCoroutine(RemoveMovementSequentailly(position, oldCharacter));
    }

    private void OnCharacterRunAway(Character character)
    {
        //Cheap fix
        if (m_Characters[0] == character)
        {
            m_Cachier.ResetSellTimer();
        }

        Remove(character);

        //The player is buying a ticket, he wins!
        if (m_Characters[0] == m_Player)
        {
            m_Cachier.SetSellTimer(1.0f);
        }
    }

    private void OnSellTicket(int ticketsLeft)
    {
        if (m_GameEnd)
            return;

        if (m_Characters == null || m_Characters.Count == 0)
            return;

        m_Characters[0].BuyTicket(m_EndPosition.position);

		// Play ticket sold sound.
		if (SoundPlayer.Instance != null)
			SoundPlayer.Instance.TicketSold.Play();
		else
			Debug.LogWarning("No sound player found, please place it in the scene to get sound.");

		//The player is buying a ticket, he wins!
		if (m_Characters[0] == m_Player)
        {
            Invoke("GameWin", 2f);
            Debug.Log("PLAYER WINS");
			MusicPlayer.Instance.TransitionToVictory();
            m_GameEnd = true;
            return;
        }

        //If this was the last ticket, the player loses
        if (ticketsLeft == 0)
        {
            //All players become sad until the scene resets
            for (int i = 1; i < m_Characters.Count; ++i)
            {
                m_Characters[i].IsSad = true;
            }

            //The cachier leaves
            m_Cachier.Leave();

            Invoke("GameLose", 1f);
            Debug.Log("PLAYER LOSES");
            MusicPlayer.Instance.TransitionToLose();
            m_GameEnd = true;
            return;
        }
    }

    private void GameWin()
    {
        GameoverWin.SetActive(true);
    }

    private void GameLose()
    {
        GameoverLoss.SetActive(true);

        for (int i = 1; i < m_Characters.Count; ++i)
        {
            m_Characters[i].Leave();
        }
    }

    //Calling events
    private void OnCharacterStartCalling()
    {
        //Make sure all the other characters can't start calling
        foreach (RandomCharacter character in m_RandomCharacters)
        {
            character.AllowCalling = false;
        }
    }

    private void OnCharacterCancelCalling()
    {
        //Make sure everyone can call again
        foreach (RandomCharacter character in m_RandomCharacters)
        {
            character.AllowCalling = true;
        }
    }

    private void OnCharacterStopCalling(Character character)
    {
        //Make sure everyone can call again
        foreach (RandomCharacter randomCharacter in m_RandomCharacters)
        {
            randomCharacter.AllowCalling = true;
        }

        //Call in a friend
        StartCoroutine(CallAFriendRoutine(character));
    }

    //Sequential movement (looks nicer)
    private IEnumerator RemoveMovementSequentailly(int position, Character oldCharacter)
    {
        //All characters have to move forwards
        List<Vector3> targetPositions = new List<Vector3>();

        //Collect all the positions
        targetPositions.Add(oldCharacter.GamePosition);

        for (int i = position + 1; i < m_Characters.Count; ++i)
        {
            targetPositions.Add(m_Characters[i - 1].GamePosition);
        }

        //Move the characters to the positions
        for (int i = position; i < m_Characters.Count; ++i)
        {
            m_Characters[i].MoveToPosition(targetPositions[i - position]);

            float randTime = UnityEngine.Random.Range(0.1f, 0.5f);
            yield return new WaitForSeconds(randTime);
        }
    }

    private IEnumerator CallAFriendRoutine(Character character)
    {
        //Spawn a new character
        RandomCharacter friendCharacter = m_CharacterManager.SpawnRandomCharacterAtPosition(m_StartPosition.position);

        //Make the character move to the current position
        Vector3 newPosition = new Vector3(character.GamePosition.x - character.Width, m_StartPosition.position.y, transform.position.z);

        friendCharacter.MoveToPosition(newPosition);

        //Wait for arrival
        while (friendCharacter.HasReachedDestination == false)
        {
            //Check if our desination changed, if so, move again!
            Vector3 tempPos = new Vector3(character.GamePosition.x - character.Width, m_StartPosition.position.y, transform.position.z);

            if (newPosition != tempPos)
            {
                newPosition = tempPos;
                friendCharacter.MoveToPosition(newPosition);
            }

            yield return new WaitForEndOfFrame();
        }

        //Once arrive, enter the queue
        int index = m_Characters.IndexOf(character);
        Insert(index + 1, friendCharacter);
        m_RandomCharacters.Add(friendCharacter);
    }

	public bool ContainsCharacter(Character character)
	{
		return m_Characters.Contains(character);
	}
}
