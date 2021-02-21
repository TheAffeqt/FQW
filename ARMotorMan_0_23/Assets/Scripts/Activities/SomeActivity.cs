using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TSUXRLab
{
    public class SomeActivity : Activity
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override IEnumerator BehaviourWhenSelected()
        {
            coroutineIsStarted = true;
            Selector.selectionPermisson = true;

            int finishTime = 20;
            for (int timer = 0; timer < finishTime;)
            {
                print(timer);
                if (!finish)
                {
                    timer = 0;
                    gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                else
                {
                    timer += 1;
                    gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
                yield return new WaitForSeconds(0.01f);
            }
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            coroutineIsStarted = false;
        }
    }
}


