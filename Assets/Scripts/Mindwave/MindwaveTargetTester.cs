using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MindwaveTargetTester : MonoBehaviour {

	[SerializeField] List<GameObject> gos = new List<GameObject>();
    [SerializeField] MindwaveSelector selector;

    int currentGO = 0;

    public void NextTarget() {
        currentGO++;
        if(currentGO>=gos.Count)currentGO = 0;
        selector.SetTarget(gos[currentGO].transform);
    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(MindwaveTargetTester))]
public class MindwaveTargetTesterEditor : Editor {
    
    public override void OnInspectorGUI(){
        MindwaveTargetTester myTarget = (MindwaveTargetTester)target;
        DrawDefaultInspector();
        if(GUILayout.Button("Toggle next")) {
            myTarget.NextTarget();
        }
    }
}
#endif
