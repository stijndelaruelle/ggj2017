using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineParticleController : MonoBehaviour {

    
    [SerializeField] float Amplitude = 1;
    [SerializeField] float Frequency = 1;

    ParticleSystem PS;
    AnimationCurve curveX;
    ParticleSystem.VelocityOverLifetimeModule vel;

	// Use this for initialization
	void Start () {
		//CreateCircle();
        PS =  GetComponent<ParticleSystem>();
        curveX  = new AnimationCurve();
         vel = PS.velocityOverLifetime;
		CreateCircle();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetFreqAmp( float amp, float freq) {
        Amplitude = amp;
        Frequency = freq;
		CreateCircle();
    }


    
    [ContextMenu("UpdateCircle")]
     void CreateCircle(){
 
         float points = 10 * Frequency;
         for (int i = 0; i < points; i++){
             float newtime = i / (points - 1);
             float myvalue = Mathf.Sin(i*360) * Amplitude;
 
             curveX.AddKey(newtime,myvalue);
             Debug.Log(newtime);
         }
         vel.x = new ParticleSystem.MinMaxCurve(10.0f, curveX);
     }
}
