using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientMove : MonoBehaviour {

    [SerializeField] Vector3 MoveBy = new Vector3(0,1,0);
    [SerializeField] float MoveTime = 1;
    [SerializeField] float DelayTime = 0;
    
    Vector3 OriginalPos;
    bool up = false;
    
	void Start () {
		OriginalPos = transform.position;
       Invoke("RepeatAnimation", DelayTime);
	}

    public void RepeatAnimation() {
        up = !up;
        iTween.MoveBy(gameObject, iTween.Hash("amount", MoveBy*(up?-1:1), "time", MoveTime, "oncomplete", "RepeatAnimation", "easetype", "easeInOutQuad"));

    }
	
}
