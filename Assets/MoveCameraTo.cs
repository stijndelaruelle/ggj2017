using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTo : MonoBehaviour {

	[SerializeField] GameObject target;

    void OnMouseUp() {
        iTween.MoveTo(Camera.main.gameObject, iTween.Hash("y", target.transform.position.y, "easetype", "easeInOutQuad", "time", 0.5f));
    }
}
