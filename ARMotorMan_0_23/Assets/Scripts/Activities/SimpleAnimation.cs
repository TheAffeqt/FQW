using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace TSUXRLab
{
    public class SimpleAnimation : Activity
    {
        [SerializeField]
        bool defaultValueOfParameter = false;

        [SerializeField]
        List<Animator> Animators;

        [SerializeField]
        string nameOfParameter;

        void Start()
        {
            foreach (Animator animator in Animators)
            {
                animator.SetBool(nameOfParameter, defaultValueOfParameter);
            }
        }

        void ToggleAnimation()
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
            //===============================

            Selector.selectionPermisson = true;
            print("selectionPermisson = true");

            if(Hint == null)
            {
                hint = HintCreator.Instance.CreateHint(GetComponent<Activity>());
            }

            hint.Toggle(true);

            while (!finish)
            {
                yield return new WaitForEndOfFrame();

                if (gestureInput.IsGestureRight(gestureInput.currentLeftHandPose))
                {
                    if (gestureInput.currentLeftHandPose != gestureInput.previousLeftHandPose)
                    {
                        ToggleAnimation();
                        continue;
                    }
                }

                if (gestureInput.IsGestureRight(gestureInput.currentRightHandPose))
                {
                    if (gestureInput.currentRightHandPose != gestureInput.previousRightHandPose)
                    {
                        ToggleAnimation();
                    }
                }
            }
            hint.Toggle(false);

            //===============================
            coroutineIsStarted = false;
        }
    }

}