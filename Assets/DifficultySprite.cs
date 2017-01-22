using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySprite : MonoBehaviour
{
    [SerializeField]
    List<Sprite> SpriteList = new List<Sprite>();

    void Start()
    {
        GetComponent<Image>().sprite = SpriteList[PlayerPrefs.GetInt("DifficultyMode")];
    }
}
