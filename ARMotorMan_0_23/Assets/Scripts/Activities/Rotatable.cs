using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;


namespace TSUXRLab
{
    public class Rotatable : Activity
    {

        MLHand CurrentRotatingHand = null;

        Vector3 currentHandProjection = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 previousHandProjection = new Vector3(0.0f, 0.0f, 0.0f);

        [SerializeField]
        float minProjectionLength = 0.00001f;

        [HeaderAttribute("Setting axis of rotation")]
        public Transform manualSettingMark;

        List<Vector3> startinglocalPosition = new List<Vector3>();


        [SerializeField]
        float boltsExtensionRange = 0.002f;
        [SerializeField]
        float RotationSpeedMultiplier = 1.2f;


        #region - угловые значения;

        [SerializeField]
        protected float _currentDegree;

        [SerializeField]
        protected float _maxRightRotateDegree;
        [SerializeField]
        protected float _maxLeftRotateDegree;
        [SerializeField]
        protected float _rightRequiredRotateDegree;
        [SerializeField]
        protected float _leftRequiredRotateDegree;
        #endregion // конец переменных

        #region - свойства;
        public virtual float currentDegree
        {
            get => _currentDegree;

            protected set
            {
                float tempDegree = Mathf.Clamp(value, maxLeftRotateDegree, maxRightRotateDegree);

                for (int i = 0; i < activityObjects.Count; i ++)
                {
                    activityObjects[i].transform.RotateAround(activityObjects[i].transform.position, GetPlaneNormal(), tempDegree - _currentDegree);

                    Vector3 temp = GetLocalPlaneNormal() * boltsExtensionRange * ((currentDegree + Mathf.Abs(maxLeftRotateDegree)) / (maxRightRotateDegree + Mathf.Abs(maxLeftRotateDegree)));
                    activityObjects[i].transform.localPosition = startinglocalPosition[i] + temp;
                }

                _currentDegree = tempDegree;
                if ((_currentDegree >= leftRequiredRotateDegree) && (_currentDegree <= rightRequiredRotateDegree))
                {
                    if (!Completed)
                    {
                        Completed = true;
                    }
                }
                else if (Completed)
                {
                    Completed = false;
                }
            }
        }
        public float maxRightRotateDegree
        {
            get
            {
                return _maxRightRotateDegree;
            }
        }
        public float maxLeftRotateDegree
        {
            get
            {
                return _maxLeftRotateDegree;
            }
        }
        public float rightRequiredRotateDegree
        {
            get
            {
                return _rightRequiredRotateDegree;
            }
        }
        public float leftRequiredRotateDegree
        {
            get
            {
                return _leftRequiredRotateDegree;
            }
        }
        #endregion // конец свойств

        private Vector3 GetLocalPlaneNormal()
        {
            return manualSettingMark.localPosition.normalized;
        }

        private Vector3 GetPlaneNormal()
        {
            return (manualSettingMark.position - transform.position).normalized;
        }

        protected virtual void Awake()
        {
            base.Awake();
            SetRotationParams();
        }

        /*public void OnPlace()
        {
            SetRotationParams();
        }*/

        protected virtual void SetRotationParams()
        {
            for (int i = 0; i < activityObjects.Count; i++)
            {
                startinglocalPosition.Add(activityObjects[i].transform.localPosition);
            }
        }

        Vector3 GetCurrentHandVector(MLHandKeyPose trackedKeyPose)
        {
            Vector3 handVectorProjection = Vector3.zero;

            if (trackedKeyPose == MLHandKeyPose.Ok)
            {
                Vector3 meanPoint = (CurrentRotatingHand.Index.KeyPoints[1].Position + CurrentRotatingHand.Thumb.KeyPoints[1].Position) / 2;
                Vector3 toProject = meanPoint - CurrentRotatingHand.Wrist.KeyPoints[0].Position;

                handVectorProjection = Vector3.ProjectOnPlane(toProject, GetPlaneNormal());
            }
            else if (trackedKeyPose == MLHandKeyPose.Finger)
            {
                handVectorProjection = CurrentRotatingHand.Pinky.KeyPoints[2].Position - CurrentRotatingHand.Index.KeyPoints[2].Position;
            }

            if (handVectorProjection.magnitude < minProjectionLength)
            {
                return new Vector3(0.0f, 0.0f, 0.0f);
            }

            return handVectorProjection.normalized;
        }



        protected override IEnumerator BehaviourWhenSelected()
        {
            coroutineIsStarted = true;
            Selector.selectionPermisson = true;
            print("selectionPermisson = true");

            while (!finish)
            {
                yield return new WaitForEndOfFrame();

                if (CurrentRotatingHand != null)
                {
                    if (gestureInput.IsGestureRight(CurrentRotatingHand.KeyPose))
                    {
                        previousHandProjection = currentHandProjection;
                        currentHandProjection = GetCurrentHandVector(CurrentRotatingHand.KeyPose);

                        Vector3 crosProduct = Vector3.Cross(currentHandProjection, previousHandProjection).normalized;

                        if (crosProduct == GetPlaneNormal())
                            currentDegree += Vector3.Angle(previousHandProjection, currentHandProjection);
                        else
                            currentDegree -= Vector3.Angle(previousHandProjection, currentHandProjection);

                        continue;
                    }

                    CurrentRotatingHand = null;
                    previousHandProjection = new Vector3(0.0f, 0.0f, 0.0f);
                    currentHandProjection = new Vector3(0.0f, 0.0f, 0.0f);
                }

                if (gestureInput.IsGestureRight(MLHands.Left.KeyPose))
                {
                    CurrentRotatingHand = MLHands.Left;
                }
                else if (gestureInput.IsGestureRight(MLHands.Right.KeyPose))
                {
                    CurrentRotatingHand = MLHands.Right;
                }
            }
            coroutineIsStarted = false;
        }
    }
}
