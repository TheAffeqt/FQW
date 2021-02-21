using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;


namespace TSUXRLab
{
    public class ButtonPress : Activity
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
        Transform PressAxisMark;
        Vector3 PressAxis;
        float requiredPushVectorLength;

        [SerializeField]
        float minPressInterval = 2.0f;

        Vector3 leftHandStartPushPoint;
        Vector3 RightHandStartPushPoint;

        void Start()
        {
            PressAxis = gameObject.transform.position - PressAxisMark.position;
            requiredPushVectorLength = PressAxis.magnitude;

            //print("DistanceToPress = " + requiredPushVectorLength);
            //print(PressAxis);
            //print("PressAxis!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //axisLineRenderer.SetPosition(0, gameObject.transform.position);
            //axisLineRenderer.SetPosition(1, PressAxisMark.position);

            foreach (Animator animator in Animators)
            {
                animator.SetBool(nameOfParameter, defaultValueOfParameter);
            }
        }

        void Toggle()
        {
            foreach (Animator animator in Animators)
            {
                animator.SetBool(nameOfParameter, !animator.GetBool(nameOfParameter));
                Completed = !Completed;
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
                    Vector3 projection = Vector3.Project(currentPushVector, PressAxis);
                    

                    if ((projection.magnitude >= requiredPushVectorLength) && (-projection.normalized == PressAxis.normalized))
                    {
                        Toggle();
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
                    print("rightGestureCaught");
                    if (RightHandStartPushPoint == Vector3.zero)
                    {
                        RightHandStartPushPoint = MLHands.Right.Center;
                        print("for the firstTIme");
                        continue;
                    }


                    Vector3 currentPushVector = MLHands.Right.Center - RightHandStartPushPoint;
                    Vector3 projection = Vector3.Project(currentPushVector, PressAxis);
                    /*print(projection);
                    print(PressAxis);

                    renderLine.SetPosition(0, RightHandStartPushPoint);
                    renderLine.SetPosition(1, MLHands.Right.Center);

                    renderLine2.SetPosition(0, RightHandStartPushPoint);
                    renderLine2.SetPosition(1, RightHandStartPushPoint + projection);*/

                    if ((projection.magnitude >= requiredPushVectorLength) && (-projection.normalized == PressAxis.normalized))
                    {
                        print("changed");
                        Toggle();
                        RightHandStartPushPoint = Vector3.zero;
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
