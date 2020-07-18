using UnityEngine;

public class GridLinesButton:MonoBehaviour
{
    public GameObject grid;    //Сітка

    //Обробка GUI. Ввімкнення/вимкнення сітки координат
    public void setActive()
    {
        if (grid.activeSelf)
            grid.SetActive(false);
        else
            grid.SetActive(true);
    }
}