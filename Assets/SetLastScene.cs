using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLastScene : MonoBehaviour {

    [SerializeField] string scene = "main";
	// Use this for initialization
	void Start () {
		
		PlayerPrefs.SetString("LastScene",scene);
	}
	
}
