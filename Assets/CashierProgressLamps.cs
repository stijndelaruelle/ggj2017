using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierProgressLamps : MonoBehaviour {

	[SerializeField] CachierCharacter cachier;
    [SerializeField] List<SpriteRenderer> Lamps = new List<SpriteRenderer>();
    [SerializeField] Sprite LampOn, LampOff;

    void Update()
    {
        if (cachier.IsGone)
        {
            for (int i = 0; i < Lamps.Count; i++) { Lamps[i].sprite = LampOff; }
            return;
        }

        int amountOn = Mathf.FloorToInt((1.0f - cachier.GetNormalizedSellTimer()) * Lamps.Count);

        //Debug.Log("Lamps on "+amountOn);

        for(int i=0;i<Lamps.Count;i++) {
            if(i<=amountOn)
                Lamps[i].sprite = LampOn;
            else
                Lamps[i].sprite = LampOff;
        }
    }
}
