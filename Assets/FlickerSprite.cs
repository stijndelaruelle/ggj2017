using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerSprite : MonoBehaviour {

    [SerializeField] Sprite SpriteA, SpriteB;
    [SerializeField] float FlickerIntervalMin=0, FlickerIntervalMax=1;
    [SerializeField] float FlickerTime=0.1f;

    [SerializeField] List<FlickerSprite> LinkedFlickers = new List<FlickerSprite>();

    bool LinkedSprite = false;
    float flickerTimer=0;
    SpriteRenderer spRen;
    
    void Start() {
        flickerTimer = Random.Range(FlickerIntervalMin, FlickerIntervalMax);
        spRen = GetComponent<SpriteRenderer>();
        foreach(FlickerSprite flsp in LinkedFlickers) {
            flsp.Link(FlickerTime);
        }
    }

    void Link(float flickerTime) {
        FlickerTime  = flickerTime;
        LinkedSprite = true;
    }

    void Update() {
        if(LinkedSprite) return;
        flickerTimer-=Time.deltaTime;

        if(flickerTimer<=0) {
            flickerTimer = Random.Range(FlickerIntervalMin, FlickerIntervalMax);
            Flicker();
            foreach(FlickerSprite flsp in LinkedFlickers) {
                flsp.Flicker();
            }
        }
    }

    void Flicker() {
        spRen.sprite = SpriteB;
        Invoke("RestoreInitialSprite", FlickerTime);
    }

    void RestoreInitialSprite() {
       spRen.sprite = SpriteA;
    }
    
}
