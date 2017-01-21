using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickGoToScene : MonoBehaviour {

    [SerializeField] string scene;
    [SerializeField] bool transition;
    string sceneToLoad;
    
    void OnMouseDown() {
        iTween.Stop();
        transform.position+=new Vector3(0,-0.2f,0);
    }
    void OnMouseUp() {
        if(transition) {
            Camera.main.gameObject.GetComponent<TransitionScript>().MoveCamera();
            Invoke("ChangeScene", 0.5f);
        }else ChangeScene(); 
    }

    void ChangeScene() {
        SceneManager.LoadScene(scene);
    }
}
