using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;


public class FootstepScript : MonoBehaviour {

   // The [SpineEvent] attribute makes the inspector for this MonoBehaviour
   // draw the field as a dropdown list of existing event names in your SkeletonData.
   [SpineEvent] public string footstepEventName = "footstep"; 
    [SerializeField] ParticleSystem Footsteps;

    void Start () {
      var skeletonAnimation = GetComponent<SkeletonAnimation>();
      if (skeletonAnimation == null) return;   // told you to add this to SkeletonAnimation's GameObject.

      // This is how you subscribe via a declared method. The method needs the correct signature.
      skeletonAnimation.state.Event += HandleEvent;


      skeletonAnimation.state.End += delegate {
         // ... or choose to ignore its parameters.
         Debug.Log("An animation ended!");
      };
   }

   void HandleEvent (TrackEntry trackEntry, Spine.Event e) {
      // Play some sound if the event named "footstep" fired.
      if (e.Data.Name == footstepEventName) {         
         Debug.Log("Play a footstep sound!");
            Footsteps.Emit(1);
      }
   }

    //remember to set the particlesystem renderorder to the character's
}
