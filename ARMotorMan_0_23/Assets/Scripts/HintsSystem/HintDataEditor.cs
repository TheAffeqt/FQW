using UnityEditor;
using UnityEngine;


namespace TSUXRLab
{
    [CustomEditor(typeof(HintData))]
    public class HintDataEditor : Editor
    {
        private HintData data;

        private void Awake()
        {
            data = (HintData)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("RemoveAll"))
            {
                data.ClearData();
            }
            if (GUILayout.Button("Remove"))
            {
                data.RemoveCurrentElement();
            }
            if (GUILayout.Button("Add"))
            {
                data.AddHint();
            }
            if (GUILayout.Button("<="))
            {
                data.GetPrevious();
            }
            if (GUILayout.Button("=>"))
            {
                data.GetNext();
            }

            GUILayout.EndHorizontal();
            base.OnInspectorGUI();
        }
    }

}
