using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {

	
    [SerializeField] List<Sprite> SpriteList = new List<Sprite>();

    void Start() {
        GetComponent<SpriteRenderer>().sprite = SpriteList[Random.Range(0,SpriteList.Count-1)];
    }
}
