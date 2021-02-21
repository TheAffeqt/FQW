using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using TMPro;

namespace TSUXRLab
{


    public class HintCreator : MonoBehaviour
    {
        [SerializeField]
        Hint hintPrefab;
        public HintData hintData;
        private static HintCreator _instance;
        public static HintCreator Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public Hint CreateHint(Activity activity)
        {
            Hint hint = Instantiate(hintPrefab, transform.position, Quaternion.identity);
            hint.SetHint(activity);
            return hint;
        }
    }
}
