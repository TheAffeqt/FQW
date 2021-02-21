using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;


namespace TSUXRLab
{
    public class Switch : Activity
    {
        #region  - animation

        [SerializeField]
        bool defaultValueOfParameter = false;
        [SerializeField]
        List<Animator> Animators;
        [SerializeField]
        string nameOfParameter;

        #endregion

        [SerializeField]
        Transform SwitchAxisMark;
        Vector3 SwitchAxis;
        float requiredPushVectorLength;

        [SerializeField]
        float minPressInterval = 2.0f;

        Vector3 leftHandStartPushPoint;
        Vector3 RightHandStartPushPoint;

        void Start()
        {
            SwitchAxis = SwitchAxisMark.position - gameObject.transform.position; 
            requiredPushVectorLength = SwitchAxis.magnitude;

            foreach (Animator animator in Animators)
            {
                animator.SetBool(nameOfParameter, defaultValueOfParameter);
            }
        }

        void Toggle(bool complete)
        {
            foreach (Animator animator in Animators)
            {
                animator.SetBool(nameOfParameter, complete);
                Completed = complete;
            }
        }

        protected override IEnumerator BehaviourWhenSelected()
        {
            coroutineIsStarted = true;
            Selector.selectionPermisson = true;

            while (!finish)
            {
                yield return new WaitForEndOfFrame();

                if (gestureInput.IsGestureRight(gestureInput.currentLeftHandPose))
                {
                    if (leftHandStartPushPoint == Vector3.zero)
                    {
                        leftHandStartPushPoint = MLHands.Left.Center;
                        continue;
                    }
                    Vector3 currentPushVector = MLHands.Left.Center - leftHandStartPushPoint;
                    Vector3 projection = Vector3.Project(currentPushVector, SwitchAxis);
                    

                    if (projection.magnitude >= requiredPushVectorLength) 
                    {
                        if(projection.normalized == SwitchAxis.normalized)
                        {
                            Toggle(true);
                        }
                        else
                        {
                            Toggle(false);
                        }
                        leftHandStartPushPoint = Vector3.zero;
                        yield return new WaitForSeconds(minPressInterval);
                        continue;
                    }
                }
                else
                {
                    leftHandStartPushPoint = Vector3.zero; 
                }

                if (gestureInput.IsGestureRight(gestureInput.currentRightHandPose))
                {
                    if (RightHandStartPushPoint == Vector3.zero)
                    {
                        RightHandStartPushPoint = MLHands.Right.Center;
                        continue;
                    }

                    Vector3 currentPushVector = MLHands.Right.Center - RightHandStartPushPoint;
                    Vector3 projection = Vector3.Project(currentPushVector, SwitchAxis);

                    if (projection.magnitude >= requiredPushVectorLength)
                    {
                        if (projection.normalized == SwitchAxis.normalized)
                        {
                            Toggle(true);
                        }
                        else
                        {
                            Toggle(false);
                        }
                        leftHandStartPushPoint = Vector3.zero;
                        yield return new WaitForSeconds(minPressInterval);
                        continue;
                    }
                }
                else
                {
                    RightHandStartPushPoint = Vector3.zero;
                }
            }
            coroutineIsStarted = false;
        }
    }
}
