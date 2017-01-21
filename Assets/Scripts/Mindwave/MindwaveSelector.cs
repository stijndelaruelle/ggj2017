using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MindwaveSelector : MonoBehaviour {

    //draws a line between this object and the target.
    
    [SerializeField] GameObject Target;
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] float Speed = 1;
    [SerializeField] float EmitInterval = 1;
    [SerializeField] float ProjectilePhase2Height = 5;
    [SerializeField] float TargetHeightOffset;
    [SerializeField] float Frequency = 1;
                     float BaseFrequency = 1;
    [SerializeField] float Amplitude = 1;
                     float BaseAmplitude = 1;
    [SerializeField] bool DistanceBasedFrequency = true;
    [SerializeField] bool FollowObj = true;

    [SerializeField] BrainwaveDevice connectedDevice;

    bool Emitting = false;

    float EmitIntervalTimer = 0;

    private void Awake(){
        //UpdateTargetEvent<character>
        //UpdateValuesEvent<float float>

        BaseFrequency = Frequency;
        BaseAmplitude = Amplitude;

        if(connectedDevice==null){
            connectedDevice = GetComponentInParent<BrainwaveDevice>();
        }
        
        connectedDevice.UpdateValuesEvent+=UpdateValues;
        connectedDevice.UpdateTargetEvent+=SetTarget;

    }

    public void SetTarget(Character target) {
         Emitting = target!=null;
        if(!Emitting) {
            return;
        }
        Debug.Log("Set target that's not null");
        Target.transform.position = target.transform.position;
    }

    public void UpdateValues(float frequency, float amplitude){
        Frequency = frequency * BaseFrequency;
        Amplitude = amplitude * BaseAmplitude;
    }
    
    void Update() {
        if(!Emitting) return;
        EmitIntervalTimer+= Time.deltaTime;
        if(EmitIntervalTimer>EmitInterval) {
            EmitIntervalTimer = 0;
            Emit();
        }
    }

    void Emit() {
        //shoot off a trailrenderer object to the target.
        GameObject projectile = Instantiate(ProjectilePrefab, transform);
        projectile.transform.position = transform.position;

        projectile.GetComponent<MindwaveProjectile>().Initialize(Target.transform, Speed, ProjectilePhase2Height, Frequency, Amplitude, DistanceBasedFrequency, FollowObj, TargetHeightOffset);
    }
}