using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickGoToScene : MonoBehaviour {

    [SerializeField] string scene;
    
    void OnMouseDown() {
        iTween.Stop();
        transform.position+=new Vector3(0,-0.2f,0);
    }
    void OnMouseUp() {
        SceneManager.LoadScene(scene);
    }
}
