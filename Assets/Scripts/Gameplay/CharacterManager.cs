using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sjabloon;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private Character m_RandomCharacterPrefab;

    [SerializeField]
    private List<Transform> m_SpawnLocations;

    [SerializeField]
    private float m_MaxOffset;

    //Holds a reference for every single character in the scene
    [SerializeField]
    private List<Character> m_Characters;
    public List<Character> Characters
    {
        get { return m_Characters; }
    }

    private void Update()
    {

    }

    public Character SpawnRandomCharacter()
    {
        if (m_SpawnLocations.Count == 0)
            return null;

        //Random position
        int rand = UnityEngine.Random.Range(0, m_SpawnLocations.Count);
        Transform randTransform = m_SpawnLocations[rand];

        //Offset the main position
        float randOffset = UnityEngine.Random.Range(-m_MaxOffset, m_MaxOffset);
        randTransform.position += new Vector3(randTransform.position.x,
                                              randTransform.position.y + randOffset,
                                              randTransform.position.z);

        return SpawnRandomCharacterAtPosition(randTransform);
    }

    public Character SpawnRandomCharacterAtPosition(Transform transform)
    {
        Character character = GameObject.Instantiate(m_RandomCharacterPrefab, transform.position, Quaternion.identity);
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
