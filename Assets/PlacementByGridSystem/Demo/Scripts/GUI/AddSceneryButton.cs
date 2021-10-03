using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSceneryButton : MonoBehaviour
{
    public Scenery scenery;        // 3D елемент встановлений на кнопці
    public Text buttonText;        // текст кнопки
    public bool isAutoSet3Dmodel;  // Чи розміщувати 3D модель автоматично (в іншому випадку її необхідно буде встановити вручну)

    void Start()
    {
        buttonText.text = SceneryManager.GetTextInfo(scenery);
        if(isAutoSet3Dmodel)
            Set3DModel();
    }

    void Set3DModel()
    {
        float buttonHeight = gameObject.GetComponent<RectTransform>().rect.height; //Допускається що кнопка квадратна, в іншому випадку потрібно шукати більшу сторону

        GameObject modelOnButton =  GameObject.Instantiate(scenery.gameObject, transform.position, transform.rotation, transform);
        modelOnButton.transform.localPosition = new Vector3(0, -buttonHeight / 3, - buttonHeight / 4);        // Поправки розташування відносно центра кнопки

        //Програмний скейл моделі по розмірах кнопки
        float scaleRatio = buttonHeight / (getMaxModelSize(modelOnButton)*2);     //Розтягую модель на половину розміра кнопки
        modelOnButton.transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
    }

    //Визначення максимального габаритного розміру моделі, без врахування Z
    private float getMaxModelSize(GameObject modelOnButton) //Визначити максимальну розмірність
    {
        Vector3 size = modelOnButton.GetComponent<MeshFilter>().sharedMesh.bounds.size;
        return Mathf.Max(size.y, size.x);      
    }
}