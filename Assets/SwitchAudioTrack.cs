using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAudioTrack : MonoBehaviour {

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    void OnMouseDown() {
        float time = source.time;
        source.clip = clip;
        source.Play();
        source.time = time;
    }
}
