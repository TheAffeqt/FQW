using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace TSUXRLab
{
    public class Highlighter : MonoBehaviour
    {
        protected MeshRenderer[] highlightRenderers;
        protected MeshRenderer[] existingRenderers;
        protected GameObject highlightHolder;
        protected static Material highlightMat;

        Activity previuousSelected = null;

        void Start()
        {
            highlightMat = (Material)Resources.Load("SteamVR_HoverHighlight", typeof(Material));

            if (highlightMat == null)
                Debug.LogError(" Hover Highlight Material is missing. Please create a material named 'SteamVR_HoverHighlight' and place it in a Resources folder", this);
        }


        void CreateHighlightRenderers()
        {
            
            highlightHolder = new GameObject("Highlighter");
            MeshFilter[] existingFilters = Selector.selectedActivity.GetComponentsInChildren<MeshFilter>(true);
            existingRenderers = new MeshRenderer[existingFilters.Length];
            highlightRenderers = new MeshRenderer[existingFilters.Length];

            for (int filterIndex = 0; filterIndex < existingFilters.Length; filterIndex++)
            {
                MeshFilter existingFilter = existingFilters[filterIndex];
                MeshRenderer existingRenderer = existingFilter.GetComponent<MeshRenderer>();

                if (existingFilter == null || existingRenderer == null)
                    continue;

                GameObject newFilterHolder = new GameObject("FilterHolder");
                newFilterHolder.transform.parent = highlightHolder.transform;
                MeshFilter newFilter = newFilterHolder.AddComponent<MeshFilter>();
                newFilter.sharedMesh = existingFilter.sharedMesh;
                MeshRenderer newRenderer = newFilterHolder.AddComponent<MeshRenderer>();

                Material[] materials = new Material[existingRenderer.sharedMaterials.Length];
                for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
                {
                    materials[materialIndex] = highlightMat;
                }
                newRenderer.sharedMaterials = materials;

                highlightRenderers[filterIndex] = newRenderer;
                existingRenderers[filterIndex] = existingRenderer;
            }
        }

        void UpdateHighlightRenderers()
        {
            if (highlightHolder == null)
                return;

            for (int rendererIndex = 0; rendererIndex < highlightRenderers.Length; rendererIndex++)
            {
                MeshRenderer existingRenderer = existingRenderers[rendererIndex];
                MeshRenderer highlightRenderer = highlightRenderers[rendererIndex];

                if (existingRenderer != null && highlightRenderer != null)
                {
                    highlightRenderer.transform.position = existingRenderer.transform.position;
                    highlightRenderer.transform.rotation = existingRenderer.transform.rotation;
                    highlightRenderer.transform.localScale = existingRenderer.transform.lossyScale;
                    highlightRenderer.enabled = Selector.selectedActivity != null && existingRenderer.enabled && existingRenderer.gameObject.activeInHierarchy;
                }
                else if (highlightRenderer != null)
                    highlightRenderer.enabled = false;
            }
        }


        void Update()
        {
            /// Проверяем наличие выбранного селектором объкта  и не создан ли уже highlightHolder
            if (Selector.selectedActivity != previuousSelected)
            {
                if (previuousSelected != null)
                {
                    Destroy(highlightHolder);
                }

                if (Selector.selectedActivity != null)
                    CreateHighlightRenderers();
            }
            else UpdateHighlightRenderers();

            previuousSelected = Selector.selectedActivity;

        }
    }
}
