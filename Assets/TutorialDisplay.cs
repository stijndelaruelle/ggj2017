using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplay : MonoBehaviour {



	// Use this for initialization
	void Awake () {
		if(PlayerPrefs.GetString("LastScene")!="main") {
            Time.timeScale = 0;
        }else Resume();
	}
	public void Resume() {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
