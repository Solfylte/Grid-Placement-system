using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSceneryButton : MonoBehaviour
{
    public Scenery scenery;
    public Text buttonText;
    public bool isAutoSet3Dmodel;

    void Start()
    {
        buttonText.text = SceneryManager.GetTextInfo(scenery);
        AddSceneryPrewievModel();
    }

    void AddSceneryPrewievModel()
    {
        float buttonHeight = gameObject.GetComponent<RectTransform>().rect.height;

        GameObject previewModel =  GameObject.Instantiate(scenery.gameObject, transform.position, transform.rotation, transform);
        previewModel.transform.localPosition = new Vector3(0, -buttonHeight * 0.3f, - buttonHeight * 0.25f);

        float scaleRatio = buttonHeight / (getMaxModelSize(previewModel) *2);
        previewModel.transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
    }

    private float getMaxModelSize(GameObject modelOnButton)
    {
        Vector3 size = modelOnButton.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        return Mathf.Max(size.y, size.x);      
    }
}