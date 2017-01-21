using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;

public class CharacterManager : MonoBehaviour
{
    [Header("Spawn position")]
    [Space(5)]
    [SerializeField]
    private List<Transform> m_SpawnLocations;

    [SerializeField]
    private float m_MaxOffset;

    [Space(10)]
    [Header("Spawn Ratio")]
    [Space(5)]
    [SerializeField]
    private float m_MinSpawnRatio;

    [SerializeField]
    private float m_MaxSpawnRatio;

    private float m_SpawnTimer = 0.0f;

    [SerializeField]
    private RandomCharacter m_RandomCharacterPrefab;

    //Holds a reference for every single character in the scene
    [SerializeField]
    private List<Character> m_Characters;
    public List<Character> Characters
    {
        get { return m_Characters; }
    }

    private void Awake()
    {
        m_SpawnTimer = m_MinSpawnRatio;
    }

    private void Update()
    {
        //Spawn a random character every second
        m_SpawnTimer -= Time.deltaTime;

        if (m_SpawnTimer <= 0.0f)
        {
            Character character = SpawnRandomCharacter();

            character.MoveToPosition(new Vector3(character.GamePosition.x * -1.0f,
                                                 character.transform.position.y,
                                                 character.transform.position.z));

            m_SpawnTimer = UnityEngine.Random.Range(m_MinSpawnRatio, m_MaxSpawnRatio);
        }
    }

    public RandomCharacter SpawnRandomCharacter()
    {
        if (m_SpawnLocations.Count == 0)
            return null;

        //Random position
        int rand = UnityEngine.Random.Range(0, m_SpawnLocations.Count);
        Vector3 randSpawnPotion = new Vector3(m_SpawnLocations[rand].position.x,
                                              m_SpawnLocations[rand].position.y,
                                              m_SpawnLocations[rand].position.z);

        //Offset the main position
        float randOffset = UnityEngine.Random.Range(-m_MaxOffset, m_MaxOffset);
        randSpawnPotion += new Vector3(0.0f, randOffset, 0.0f);

        return SpawnRandomCharacterAtPosition(randSpawnPotion);
    }

    public RandomCharacter SpawnRandomCharacterAtPosition(Vector3 position)
    {
        RandomCharacter character = GameObject.Instantiate(m_RandomCharacterPrefab, position, Quaternion.identity);
        character.DestroyEvent += OnCharacterDestroy;

        m_Characters.Add(character);

        return character;
    }

    private void OnCharacterDestroy(Character character)
    {
        character.DestroyEvent -= OnCharacterDestroy;
        m_Characters.Remove(character);

        GameObject.Destroy(character.gameObject);
    }
}
