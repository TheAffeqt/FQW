using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace TSUXRLab
{
    public class CrossSection : Activity
    {

        public GameObject plane;

        MLHand currentHand = null;

        Vector3 previousPosition;
        Vector3 currentPosition;

        Vector3 handNormal;
        Vector3 planeNormal;
        Vector3 planePosition;
        Vector3 p1, p2, p3;

        private new void Awake()
        {
            plane.SetActive(false);
            planeNormal = plane.transform.TransformVector(new Vector3(0, 0, -1));
            planePosition = plane.transform.position;
        }

        void Start()
        {
            StartCoroutine(BehaviourWhenSelected());
            finish = false;
        }

        Vector3 getPos()
        {
            planePosition = plane.transform.position;
            return planePosition;
        }

        Vector3 getNormal()
        {
            p1 = currentHand.Index.KeyPoints[1].Position;
            p2 = currentHand.Pinky.KeyPoints[1].Position;
            p3 = currentHand.Wrist.KeyPoints[0].Position;
            Plane hand = new Plane(p1, p2, p3);
            handNormal = hand.normal;
            return handNormal;
        }

        Vector3 getAxis()
        {
            Vector3 currentPlaneNormal = plane.transform.TransformVector(new Vector3(0, 0, -1));
            return Vector3.Cross(getNormal(), currentPlaneNormal).normalized;
        }

        float getAngle()
        {
            Vector3 currentPlaneNormal = plane.transform.TransformVector(new Vector3(0, 0, -1));
            return Vector3.SignedAngle(getNormal(), currentPlaneNormal, getAxis());
        }

        protected override IEnumerator BehaviourWhenSelected()
        {
            coroutineIsStarted = true;
            Selector.selectionPermisson = true;

            while (!finish)
            {
                yield return new WaitForEndOfFrame();

                print("CorutineIsStarted");

                if (currentHand != null)
                {

                    print("RightHand");
                    if (gestureInput.Gestures.Contains(currentHand.KeyPose))
                    {
                        print("RightGesture");
                        currentPosition = currentHand.Center;
                        plane.SetActive(true);
                        Vector3 tempPosVect = (currentPosition - previousPosition) * 2f * Time.deltaTime;
                        tempPosVect.y = -tempPosVect.y;
                        plane.transform.Translate(tempPosVect);
                        plane.transform.RotateAround(getPos(), getAxis(), -getAngle());
                        previousPosition = currentPosition;
                    }

                    else
                    {
                        currentHand = null;
                        plane.SetActive(false);
                    }

                    continue;
                }

                if (gestureInput.IsGestureRight(gestureInput.currentLeftHandPose))
                {
                    currentHand = MLHands.Left;
                    previousPosition = currentHand.Center;
                }

                else if (gestureInput.IsGestureRight(gestureInput.currentRightHandPose))
                {
                    currentHand = MLHands.Right;
                    previousPosition = currentHand.Center;
                }

            }

            coroutineIsStarted = false;
        }

    }

}