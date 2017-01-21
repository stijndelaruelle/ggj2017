using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour {


    
    [SerializeField] Material           TransitionMaterial;
    [SerializeField] float              TransitionTime = 1;
    [SerializeField] Texture            TransInTex; 
    [SerializeField] Texture            TransOutTex; 
    [SerializeField] Color              FadeColor = new Color();

    [SerializeField] bool TransitionOutOnAwake = true;

    
    bool    TransitioningIn   = false;
    bool    TransitioningOut  = false;
    bool    FinishOff         = false;
    float   TransitionTimer   = 0;

    void Start() {
        TransitionMaterial.SetColor("_Color", FadeColor);
        
        TransitioningOut = true;
        TransitioningIn = false;
        TransitionTimer = TransitionTime;
        TransitionMaterial.SetTexture("_TransitionTex", TransOutTex);
    }


    void OnRenderImage(RenderTexture src, RenderTexture dst){
         if(TransitionMaterial != null) {
            if(TransitioningIn) {
                TransitionMaterial.SetFloat("_Cutoff", 1-TransitionTimer/(TransitionTime/2));
                Graphics.Blit(src, dst, TransitionMaterial);
            }
            if(TransitioningOut) {
                TransitionMaterial.SetFloat("_Cutoff", TransitionTimer/(TransitionTime/2));
                Graphics.Blit(src, dst, TransitionMaterial);
            }
        }

        if(FinishOff) {
                Graphics.Blit(src, dst, TransitionMaterial);
            FinishOff = false;
        }
    }


     public void MoveCamera() {
        Debug.Log("Move cam");
        TransitionTimer = TransitionTime/2;
        TransitioningIn = true;
        TransitionMaterial.SetTexture("_TransitionTex", TransInTex);
    }

    void Update() {
        if(TransitioningIn) { 
            TransitionTimer -= Time.deltaTime;
            if(TransitionTimer<=0) {
                TransitioningOut = true;
                TransitioningIn = false;
                TransitionTimer = TransitionTime/2;
                TransitionMaterial.SetTexture("_TransitionTex", TransOutTex);
            }
        }else
        if(TransitioningOut) { 
            TransitionTimer -= Time.deltaTime;
            if(TransitionTimer<=0) {
                TransitioningOut = false;
                FinishOff = true;
                TransitionMaterial.SetFloat("_Cutoff", 0);
            }
        }
    }

}
