using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindwaveProjectile : MonoBehaviour {

    bool Move = false;
    Vector3 StartPos, TargetPos;
    float TravelTime;
    float TravelTimer;
    float Phase2height;
    float Frequency, Amplitude;
    bool DistanceBasedFrequency;
    static Transform TargetTransform;
    bool FollowObject;
    float TargetHeightOffset;
    

    public void Initialize(Transform targetTransform, float speed, float phase2height, float freq, float amp, bool distancebasedfreq, bool followObj, float targetHeightOffset) {
        //receive the information needed to create the path
        TargetPos = targetTransform.position;
        FollowObject = followObj;
        if(followObj) TargetTransform = targetTransform;
        StartPos = transform.position;
        Phase2height = phase2height;
        Amplitude = amp;
        Frequency = freq;
        DistanceBasedFrequency = distancebasedfreq;
        TargetHeightOffset = targetHeightOffset;
        
        //calculate time it should take
        TravelTime = Vector3.Distance(TargetPos, transform.position)/speed;

        
        if(DistanceBasedFrequency) {
            Frequency = freq * TravelTime;
        }

        Move = true;
    }

    void Update() {
        if(!Move) return;

        //    __^_ _^_   
        //   / .  v . \   < hoot
        //  / /\/\/\/\ \ 

        //take care of horizontal movement
        TravelTimer += Time.deltaTime;
        float travelPercentage = TravelTimer/TravelTime;
        if(FollowObject) TargetPos = TargetTransform.transform.position;
        Vector3 pos = (TargetPos - StartPos) * travelPercentage + StartPos;
        //float distance = Vector3.Distance(pos, transform.position);


        if(travelPercentage<0.05f) {
            //phase one
            //upwards move + move towards target
            //15%
            pos.y +=  Phase2height * (travelPercentage/0.05f);

        }else if(travelPercentage>=0.05f && travelPercentage<0.95f) {
            //phase two
            //sine waving at height
            //60%

            //calculate a sine wave based on the provided frequency and amplitude
            pos.y = Mathf.Sin(Frequency*travelPercentage) * Amplitude + Phase2height;

        }else {
            //phase three
            //move downwards + toward target
            //15%
            pos.y +=  Phase2height-Phase2height * ((travelPercentage-0.95f)/0.05f)+TargetHeightOffset; 

        }

        transform.position = pos;
        
        if(travelPercentage>1) {
            Move = false;
            Destroy(gameObject, GetComponent<TrailRenderer>().time);
        }
        
    }
}
