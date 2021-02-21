using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TSUXRLab
{
    public abstract class Activity : MonoBehaviour
    {
        [SerializeField]
        protected Hint hint;
        
        public delegate void boolNotificationDelegate(bool status);
        public delegate void activtiyNotificationDelegate(Activity activity);

        [SerializeField]
        protected GesturesInputHandler gestureInput;

        public GesturesInputHandler GestureInput
        {
            get
            {
                return gestureInput;
            }
        }
        protected GameObject Hint;

        [SerializeField]
        protected List<GameObject> activityObjects = null;
        public List<GameObject> ActivityObjects
        {
            get
            {
                return activityObjects;
            }

            protected set
            {
                activityObjects = value;
            }

        }

        [SerializeField]
        protected int _ID;
        public int ID
        {

            get
            {
                return _ID;
            }

        }

        protected bool _Completed = false;
        public UnityEvent CompleteChangingEditorEvent;
        public boolNotificationDelegate OnCompleteChange;
        public bool Completed
        {
            get
            {
                return _Completed;
            }

            protected set
            {
                if (_Completed != value)
                {
                    _Completed = value;
                    OnCompleteChange?.Invoke(value);
                    CompleteChangingEditorEvent.Invoke();
                }
            }
        }

        [SerializeField]
        protected bool _Locked;
        public activtiyNotificationDelegate OnLockedChange;
        public bool Locked
        {
            get
            {
                return _Locked;
            }

            set
            {
                if (_Locked != value)
                {
                    _Locked = value;
                    print(transform.name);
                    if(transform.name == "boltsHolder")
                    {
                        OnLockedChange(transform.GetComponent<Rotatable>());
                    }
                    else
                    {
                        OnLockedChange(this);
                    }
                }
            }

        }


        // base.Awake in override Awake is required!!! 
        protected void Awake()
        {
            if (activityObjects == null)
            {
                activityObjects = new List<GameObject> { gameObject };
            }

            OnLockedChange += RaycastHandleController.getInstance().OnLockChange;
            Locked = _Locked;
        }

        protected abstract IEnumerator BehaviourWhenSelected();
        protected bool finish = false;
        protected bool coroutineIsStarted = false;

        public void Select()
        {
            finish = false;
            if (coroutineIsStarted == false)
                StartCoroutine(BehaviourWhenSelected());

            print("Select: " + transform.name);
        }

        public void UnSelect()
        {
            finish = true;
            print("unSelect: " + transform.name);
        }

    }
}