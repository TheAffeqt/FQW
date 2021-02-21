using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace TSUXRLab
{


    public class GestureIconContainer : MonoBehaviour
    {

        [SerializeField]
        Sprite gestureC;
        [SerializeField]
        Sprite gestureFinger;
        [SerializeField]
        Sprite gestureFist;
        [SerializeField]
        Sprite gestureL;
        [SerializeField]
        Sprite gestureOk;
        [SerializeField]
        Sprite gestureOpenHandBack;
        [SerializeField]
        Sprite gesturePinch;
        [SerializeField]
        Sprite gestureThumb;

        public static Dictionary<MLHandKeyPose, Sprite> gesturesIcons = new Dictionary<MLHandKeyPose, Sprite>();
        private void Start()
        {
            gesturesIcons.Add(MLHandKeyPose.C, gestureC);
            gesturesIcons.Add(MLHandKeyPose.Finger, gestureFinger);
            gesturesIcons.Add(MLHandKeyPose.Fist, gestureFist);
            gesturesIcons.Add(MLHandKeyPose.L, gestureL);
            gesturesIcons.Add(MLHandKeyPose.Ok, gestureOk);
            gesturesIcons.Add(MLHandKeyPose.OpenHand, gestureOpenHandBack);
            gesturesIcons.Add(MLHandKeyPose.Pinch, gesturePinch);
            gesturesIcons.Add(MLHandKeyPose.Thumb, gestureThumb);
        }

    }
}
