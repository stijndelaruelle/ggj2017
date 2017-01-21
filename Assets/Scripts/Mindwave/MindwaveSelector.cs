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
    [SerializeField] float Frequency = 1;
                     float BaseFrequency = 1;
    [SerializeField] float Amplitude = 1;
                     float BaseAmplitude = 1;
    [SerializeField] bool DistanceBasedFrequency = true;
    [SerializeField] bool FollowObj = true;


    float EmitIntervalTimer = 0;

    private void Awake()
    {
        BaseFrequency = Frequency;
        BaseAmplitude = Amplitude;
    }

    public void SetTarget(Transform target) {
        Target.transform.position = target.position;
    }

    public void SetFrequency(float frequency)
    {
        Frequency = frequency * BaseFrequency;
    }

    public void SetAmplitude(float amplitude)
    {
        Amplitude = amplitude * BaseAmplitude;
    }

    void Update() {
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

        projectile.GetComponent<MindwaveProjectile>().Initialize(Target.transform, Speed, ProjectilePhase2Height, Frequency, Amplitude, DistanceBasedFrequency, FollowObj);
    }

}