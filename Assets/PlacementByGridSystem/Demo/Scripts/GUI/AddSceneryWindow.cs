using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSceneryWindow : MonoBehaviour
{
    public Transform scrollableButtonsTransform;       //Трансформ об'єкт, який містить кнопки(встановлюється з редактора) 
    [SerializeField]
    private float leftScrollClamp, rightScrollClamp;   // Дистанція на яку можна зміщувати об'єкт
    [SerializeField]
    private float scrollSpeed;                         // Швидкість прокрутки

    private Vector2 startPosition;                     // стартова позиція дотику
    private float targetPos;

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    public void Scroll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition/Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            float curPosition = Input.mousePosition.x / Screen.width - startPosition.x;

            targetPos = Mathf.Clamp(scrollableButtonsTransform.localPosition.x + curPosition*scrollSpeed, -leftScrollClamp, rightScrollClamp);
            scrollableButtonsTransform.localPosition = new Vector3(targetPos, scrollableButtonsTransform.localPosition.y, scrollableButtonsTransform.localPosition.z);
        }
    }

    public void setActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        GlobalSetting.mainScreenControlAvailable = !isActive;
    }

    public void addSceneryOnMap()
    {
        setActive(false);
    }
}