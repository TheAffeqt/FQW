using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using TMPro;

namespace TSUXRLab
{


    public class Hint : MonoBehaviour
    {
        [SerializeField]
        GameObject canvas;

        Activity aimForFolowing;

        //For LineRender

        LineRenderer line;
        [SerializeField]
        float startWidth = 0.01f;
        [SerializeField]
        float endWidth = 0.001f;
        [SerializeField]
        Material lineMaterial;
        [SerializeField]
        Color startColor;
        [SerializeField]
        Color endColor;

        //For search corners of canvas 

        RectTransform rt;

        public void Toggle(bool status)
        {
            canvas.SetActive(status);
        }

        private Sprite GetIcon(Activity target)
        {
            print(target.GestureInput.Gestures[0]);
            return GestureIconContainer.gesturesIcons[target.GestureInput.Gestures[0]];
        
        }

        public void SetHint(Activity targetObj)
        {
            Data hintData = HintCreator.Instance.hintData.GetData(targetObj.ID);
            SetCanvas(hintData.NameObject, hintData.DescriptionObject, hintData.DescriptionGesture, GetIcon(targetObj), targetObj);
            line = canvas.AddComponent<LineRenderer>();
            line.startWidth = startWidth;
            line.endWidth = endWidth;
            line.startColor = startColor;
            line.endColor = endColor;
            line.material = lineMaterial;
        }

        void SetCanvas(string header, string description, string gestureText, Sprite gestureImage, Activity target)
        {
            Vector3 hintPositoin = (target.transform.position + Camera.main.transform.position)/2 + new Vector3(0,0,0.3f);

            canvas.transform.position = hintPositoin;
            canvas.transform.rotation = Quaternion.identity;

            canvas.transform.Find("LineSeparator/MainText").GetComponent<TextMeshProUGUI>().text = description;
            canvas.transform.Find("LineSeparator/MainText/HeaderText").GetComponent<TextMeshProUGUI>().text = header;
            canvas.transform.Find("LineSeparator/GestureImage").GetComponent<Image>().sprite = gestureImage;
            canvas.transform.Find("LineSeparator/GestureImage/GestureText").GetComponent<TextMeshProUGUI>().text = gestureText;

            rt = canvas.transform.GetComponent<RectTransform>();
            aimForFolowing = target;
        }
        
        void Update()
        {
            if (canvas !=null)
            {
                Camera camera = Camera.main;
                canvas.transform.LookAt(canvas.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

                Vector3[] worldCorners = new Vector3[4];
                rt.GetWorldCorners(worldCorners);
                line.GetComponent<LineRenderer>().SetPosition(0, (worldCorners[0] + worldCorners[1]) / 2);
                line.GetComponent<LineRenderer>().SetPosition(1, aimForFolowing.transform.position);
            }
        }
    }
}
