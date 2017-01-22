using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierProgressLamps : MonoBehaviour {

	[SerializeField] CachierCharacter cachier;
    [SerializeField] List<SpriteRenderer> Lamps = new List<SpriteRenderer>();
    [SerializeField] Sprite LampOn, LampOff;
    void Update() {

        int amountOn = (int)((1- cachier.GetNormalizedSellTimer()) * Lamps.Count);

        Debug.Log("Lamps on "+amountOn);

        for(int i=0;i<Lamps.Count;i++) {
            if(i<=amountOn)
                Lamps[i].sprite = LampOn;
            else
                Lamps[i].sprite = LampOff;
        }
    }
}
