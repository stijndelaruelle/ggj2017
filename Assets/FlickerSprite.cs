using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerSprite : MonoBehaviour {

    [SerializeField] Sprite SpriteA, SpriteB;
    [SerializeField] float FlickerIntervalMin=0, FlickerIntervalMax=1;
    [SerializeField] float FlickerTime=0.1f;

    float flickerTimer=0;
    SpriteRenderer spRen;
    
    void Start() {
        flickerTimer = Random.Range(FlickerIntervalMin, FlickerIntervalMax);
        spRen = GetComponent<SpriteRenderer>();
    }

    void Update() {
        flickerTimer-=Time.deltaTime;

        if(flickerTimer<=0) {
            flickerTimer = Random.Range(FlickerIntervalMin, FlickerIntervalMax);
            spRen.sprite = SpriteB;
            Invoke("RestoreInitialSprite", FlickerTime);
        }
    }

    void RestoreInitialSprite() {
       spRen.sprite = SpriteA;
    }
    
}
