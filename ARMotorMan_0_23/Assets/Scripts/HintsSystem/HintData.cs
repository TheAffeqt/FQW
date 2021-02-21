using System;
using System.Collections.Generic;
using UnityEngine;

namespace TSUXRLab
{
    [CreateAssetMenu(menuName = "Data Hints/Hints", fileName = "Hints")]
    public class HintData : ScriptableObject
    {
        [SerializeField, HideInInspector] private List<Data> hintList;

        [SerializeField] private Data currentHint;

        private int currentIndex = 0;

        #region Methods for Insepctor
        public void AddHint()
        {
            if (hintList == null)
                hintList = new List<Data>();

            currentHint = new Data();
            hintList.Add(currentHint);
            currentIndex = hintList.Count - 1;
        }

        public Data GetNext()
        {
            if (currentIndex < hintList.Count - 1)
                currentIndex++;

            currentHint = this[currentIndex];
            return currentHint;
        }

        public Data GetPrevious()
        {
            if (currentIndex > 0)
                currentIndex--;

            currentHint = this[currentIndex];
            return currentHint;
        }

        public void ClearData()
        {
            hintList.Clear();
            hintList.Add(new Data());
            currentHint = hintList[0];
            currentIndex = 0;
        }

        public void RemoveCurrentElement()
        {
            if (currentIndex > 0)
            {
                currentHint = hintList[--currentIndex];
                hintList.RemoveAt(++currentIndex);
            }
            else
            {
                hintList.Clear();
                currentHint = null;
            }
        }
        #endregion

        public Data GetData(int ID)
        {
            foreach (Data data in hintList)
            {
                if (data.ID == ID)
                {
                    Debug.Log("!!!!!!!!!! ID IS " + data.ID);
                    return data;
                }
            }
            Debug.Log("Null, motherfucker~!!!!!!!!!!!!!!!!!!!!!!!!!!~~~");
            return null;
        }

        #region Indexator
        public Data this[int index]
        {
            get
            {
                if (hintList != null && index >= 0 && index < hintList.Count)
                    return hintList[index];
                return null;
            }

            set
            {
                if (hintList == null)
                    hintList = new List<Data>();

                if (index >= 0 && index < hintList.Count && value != null)
                    hintList[index] = value;
                else Debug.LogError("Выход за границы массива либо value = null");
            }
        }
        #endregion
    }

    [Serializable]
    public class Data
    {
        [Tooltip("ID Activity")]
        [SerializeField] private int _ID;
        public int ID
        {
            get { return _ID; }
            protected set { }
        }

        [Tooltip("Название объекта")]
        [SerializeField] private string _nameObject;
        public string NameObject
        {
            get { return _nameObject; }
            protected set { }
        }

        [Tooltip("Описание объекта")]
        [SerializeField] private string _descriptionObject;
        public string DescriptionObject
        {
            get { return _descriptionObject; }
            protected set { }
        }

        [Tooltip("Описание для жеста, что должно призойти после его показа")]
        [SerializeField] private string _descriptionGesture;
        public string DescriptionGesture
        {
            get { return _descriptionGesture; }
            protected set { }
        }

    }

}

