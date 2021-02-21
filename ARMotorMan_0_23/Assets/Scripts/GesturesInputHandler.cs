using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace TSUXRLab
{
    public class GesturesInputHandler : MonoBehaviour
    {
        [SerializeField]
        List<MLHandKeyPose> _gestures;
        public List<MLHandKeyPose> Gestures
        {
            get
            {
                return _gestures;
            }
            private set
            {
                _gestures = value;
            }
        }
    

        [SerializeField]
        float lowBorderOfConfidence = 0.90f;

        public MLHandKeyPose currentLeftHandPose { get; private set; }
        public MLHandKeyPose currentRightHandPose { get; private set; }
        public MLHandKeyPose previousLeftHandPose { get; private set; }
        public MLHandKeyPose previousRightHandPose { get; private set; }

        private void Update()
        {
            previousRightHandPose = currentRightHandPose;
            previousLeftHandPose = currentLeftHandPose;

            if (MLHands.Left.KeyPoseConfidence > lowBorderOfConfidence)
            {
                if (Gestures.Contains(MLHands.Left.KeyPose))
                    currentLeftHandPose = MLHands.Left.KeyPose;
                else
                    currentLeftHandPose = MLHandKeyPose.NoPose;
            }

            if (MLHands.Right.KeyPoseConfidence > lowBorderOfConfidence)
            {
                if (Gestures.Contains(MLHands.Right.KeyPose))
                    currentRightHandPose = MLHands.Right.KeyPose;
                else
                    currentRightHandPose = MLHandKeyPose.NoPose;
            }
        }

        public bool IsGestureRight(MLHandKeyPose keyPose)
        {
            if (Gestures.Contains(keyPose))
                return true;
            else
                return false;

        }
    }
}
