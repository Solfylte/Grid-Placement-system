using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSceneryWindow : MonoBehaviour
{
    public Transform buttonsHolder;
    [SerializeField] private float leftScrollClamp, rightScrollClamp;
    [SerializeField] private float scrollSpeed;

    private Vector2 startPosition;
    private float targetPos;

    void Update()
    {
        UpdateScroll();
    }

    public void UpdateScroll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition/Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            float curPosition = Input.mousePosition.x / Screen.width - startPosition.x;

            targetPos = Mathf.Clamp(buttonsHolder.localPosition.x + curPosition*scrollSpeed, -leftScrollClamp, rightScrollClamp);
            buttonsHolder.localPosition = new Vector3(targetPos, buttonsHolder.localPosition.y, buttonsHolder.localPosition.z);
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