using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetSortingLayer : MonoBehaviour {
    

    [SerializeField] SpriteRenderer ParentSpriteRenderer;
    [SerializeField] int Offset = 0;
    private void Awake(){
        if(!ParentSpriteRenderer) { 
            GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * 100 * -1) + Offset;
        }
    }

    void Start() {
        if(ParentSpriteRenderer) { 
            GetComponent<SpriteRenderer>().sortingOrder = ParentSpriteRenderer.GetComponent<SpriteRenderer>().sortingOrder + Offset;
        }
    }
    
}
