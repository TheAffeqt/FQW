


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace TSUXRLab
{
    public class DependenciesController : MonoBehaviour
    {


        public bool _lockingStatus = false;
        public bool lockingStatus
        {
            get
            {
                return _lockingStatus;
            }

            private set
            {
                if (value != _lockingStatus)
                {
                    _lockingStatus = value;
                    SetLockingForSubscribers();
                }
            }
        }

        [SerializeField]
        List<Activity> subscribers = null;

        // groups activities into conjunction groups
        // i-th elem of List<int> DNF_formula corresponds to i-th elem of List<Activity> variables
        // Activities with the same abs(number) in an array of conjunctions ("conjunctions") are grouped into a single conjunction
        // Sign of number determines will that varable be with "inverse" sign in DNF or not

        [SerializeField]
        List<Activity> variables = null;

        [SerializeField]
        List<int> DNF_formula = null;
        List<int> conjunctions = new List<int>();

        void Start()
        {
            if ((DNF_formula == null) || (variables == null) || (DNF_formula.Count != variables.Count))
            {
                return;
            }

            foreach (int anotherConj in DNF_formula.Distinct())
            {
                int absValue = Mathf.Abs(anotherConj);
                if (conjunctions.Contains(absValue))
                    continue;

                conjunctions.Add(absValue);
            }

            foreach (Activity variable in variables)
            {
                variable.OnCompleteChange += OnChange;
            }

            RefreshStatus();
            SetLockingForSubscribers();
        }

        void OnChange(bool newStatus)
        {
            RefreshStatus();
        }

        void RefreshStatus()
        {
            //TODO add more intuitive system with DNF_formula setup in project
            foreach (int anotherConj in conjunctions)
            {
                int i = 0;
                for (; i < variables.Count; i += 1)
                {
                    if (DNF_formula[i] == anotherConj)
                    {
                        if (variables[i].Completed == false)
                            break;
                    }
                    else if (-DNF_formula[i] == anotherConj)
                    {
                        if (variables[i].Completed == true)
                            break;
                    }

                    if (i == variables.Count - 1)
                    {
                        lockingStatus = false;
                        return;
                    }
                }
            }
            lockingStatus = true;
            return;

        }

        void SetLockingForSubscribers()
        {
            foreach (Activity activity in subscribers)
            {
                print("Set " + activity.transform.name + " Locking = " + lockingStatus);
                activity.Locked = lockingStatus;
            }
        }
    }
}