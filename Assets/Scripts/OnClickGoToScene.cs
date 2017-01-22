using Sjabloon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickGoToScene : MonoBehaviour {

    [SerializeField] string scene;
    [SerializeField] bool transition;
    string sceneToLoad;

    [SerializeField]
    private ControllerButtonCode m_ButtonCode;

    [SerializeField]
    private KeyCode m_KeyCode;

    private InputManager m_InputManger;

    //Chea fix
    private bool m_SkipFirstFrame = false;

    private void Start()
    {
        m_InputManger = InputManager.Instance;

        m_InputManger.BindButton("Controller_Load_" + scene, 0, m_ButtonCode, InputManager.ButtonState.OnRelease);
        m_InputManger.BindButton("Keyboard_Load_" + scene, m_KeyCode, InputManager.ButtonState.OnRelease);
    }

    private void OnDestroy()
    {
        if (m_InputManger == null)
            return;

        m_InputManger.UnbindButton("Controller_Load_" + scene);
        m_InputManger.UnbindButton("Keyboard_Load_" + scene);
    }

    private void Update()
    {
        if (m_SkipFirstFrame == false)
        {
            m_SkipFirstFrame = true;
            return;
        }

        bool controllerSubmit = InputManager.Instance.GetButton("Controller_Load_" + scene);
        bool keyboardSubmit = InputManager.Instance.GetButton("Keyboard_Load_" + scene);

        if (controllerSubmit || keyboardSubmit)
        {
            OnMouseUp();
        }
    }

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
