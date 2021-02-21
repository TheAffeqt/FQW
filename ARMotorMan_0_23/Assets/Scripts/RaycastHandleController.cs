using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSUXRLab
{
    public class RaycastHandleController
    {
        private static RaycastHandleController instance;

        private RaycastHandleController() { }

        public static RaycastHandleController getInstance()
        {
            if (instance == null)
                instance = new RaycastHandleController();
            return instance;
        }


        public void OnLockChange(Activity activity)
       {
            if (activity.Locked == true)
            {
                // Ignore Raycast Layer
                SetLayer(activity, 2);
            }
            else
            {
                // Default Raycast Layer
                SetLayer(activity, 0);
            }
        }

        public static void SetLayer(Activity activity, int layerNumber)
        {
            activity.gameObject.layer = layerNumber;
            foreach (GameObject go in activity.ActivityObjects)
            {
                go.layer = layerNumber;
            }
        }
    }

    
}
