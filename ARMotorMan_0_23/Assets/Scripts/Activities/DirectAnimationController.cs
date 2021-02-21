
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.MagicLeap;


namespace TSUXRLab
{
    public class DirectAnimationController : Activity
    {
        [SerializeField]
        Animator animatorController;
        [SerializeField]
        string nameOfNormTimeController = "DisassemblyTime";
        float normalizedTime;
        float normTimeDelta;

        [SerializeField]
        float minAnimTimePerSec = 0.5f;

        Vector3 currentHandPosition = new Vector3(0.0f, 0.0f, 0.0f);

        MLHand currentDissassemblingHand = null;
        Vector3 startingPosition;

        [SerializeField]
        float diapason;

        private void Update()
        {
            float currentValueOfNormTime = animatorController.GetFloat(nameOfNormTimeController);
            normTimeDelta = normalizedTime - currentValueOfNormTime;

            if(normTimeDelta > 0.01)
            {
                currentValueOfNormTime += Mathf.Clamp(normTimeDelta * Time.deltaTime, Time.deltaTime * minAnimTimePerSec, 1.0f);
            }
            else if(normTimeDelta < -0.01)
            {
                currentValueOfNormTime += Mathf.Clamp(normTimeDelta * Time.deltaTime, -1.0f, Time.deltaTime * (-minAnimTimePerSec));
            }

            animatorController.SetFloat(nameOfNormTimeController, currentValueOfNormTime);
        }

        protected override IEnumerator BehaviourWhenSelected()
        {
            coroutineIsStarted = true;
            Selector.selectionPermisson = true;

            while (!finish)
            {
                yield return new WaitForEndOfFrame();

                if (currentDissassemblingHand != null)
                {
                    //print("haveDisassemblingHand");
                    if (gestureInput.Gestures.Contains(currentDissassemblingHand.KeyPose))
                    {
                        //print("WithRightGesture");
                        currentHandPosition = currentDissassemblingHand.Center;

                        normalizedTime = Mathf.Clamp(Mathf.Abs((currentHandPosition - startingPosition).magnitude) / diapason, 0.0f, 1.0f);
                        //print(normalizedTime);
                        //animatorController.SetFloat(nameOfNormTimeController, normalizedTime);
                        if (normalizedTime == 0.0f)
                        {
                            Completed = false;
                        }
                        else
                        {
                            Completed = true;
                        }

                    }
                    else
                    {
                        currentDissassemblingHand = null;
                        normalizedTime = 0.0f;
                        Completed = false;
                    }
                    continue;
                }

                if (gestureInput.IsGestureRight(gestureInput.currentLeftHandPose))
                {
                    currentDissassemblingHand = MLHands.Left;
                    startingPosition = currentDissassemblingHand.Center;
                }
                else if (gestureInput.IsGestureRight(gestureInput.currentRightHandPose))
                {
                    currentDissassemblingHand = MLHands.Right;
                    startingPosition = currentDissassemblingHand.Center;
                }
            }
            coroutineIsStarted = false;
        }

    }
}
