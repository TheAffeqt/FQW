using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;


namespace TSUXRLab
{ 
	public class Selector : WorldRaycastEyes
	{
		private static Selector _instance;
		public static Selector Instance { get { return _instance; } }

		public Transform camera;

		[SerializeField]
		public static bool selectionPermisson;
		public static Activity selectedActivity { get; private set; }
		public static Activity gazeTargetActivity { get; private set; }

		[SerializeField]
		float _sphereCastRadius = 0.1f;
		[SerializeField]
		float _rayCastDistance = 1000f;
		[SerializeField]
		GameObject target;

		// Update is called once per frame

		private void Awake()
		{
			selectedActivity = null;
			gazeTargetActivity = null;
			target.SetActive(false);
			selectionPermisson = true;

			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				_instance = this;
			}
		}

		void Update()
		{
			DetectGazeTarget();
			if (selectionPermisson)
				SetSelection();
		}
		
		private void DetectGazeTarget()
		{
			RaycastHit hit;

			Vector3 EyeGazeDirection;
			Vector3 HeadPosePosition = camera.position;

			

			//if (MLEyes.FixationPoint == Vector3.zero)
			//{
			EyeGazeDirection = camera.transform.TransformVector(new Vector3(0, 0, 1));
				target.SetActive(true);
				target.transform.position = HeadPosePosition + EyeGazeDirection / 2.5f;
			//}
			//else
			//{
			//	EyeGazeDirection = Direction;
			//	target.SetActive(false);
			//}

			//lineRend.SetPosition(0, HeadPosePosition);
			//lineRend.SetPosition(1, HeadPosePosition + EyeGazeDirection);

			Physics.SphereCast(HeadPosePosition, _sphereCastRadius, EyeGazeDirection, out hit, _rayCastDistance);

			try
			{
				gazeTargetActivity = hit.transform.GetComponent<Activity>();
				//print("GazeTarget = " + gazeTargetActivity.transform.name);
			}
			catch
			{

				//print("No gaze target");
				gazeTargetActivity = null;
			}
		}

		private void SetSelection()
		{
			print("SetSelection");
            if (gazeTargetActivity != selectedActivity)
			{
                if(selectedActivity != null)
					selectedActivity.UnSelect();

				selectedActivity = gazeTargetActivity;
				if (selectedActivity != null)
					print("Selected = " + selectedActivity.transform.name);
				else
					print("Empty selected");

				if (selectedActivity != null)
				{
					selectionPermisson = false;
					selectedActivity.Select();
				}					
            }
		}
	}
}

