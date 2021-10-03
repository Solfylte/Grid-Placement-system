using UnityEngine;

public class GridLinesButton:MonoBehaviour
{
    public GameObject grid;

    public void setActive()
    {
        if (grid.activeSelf)
            grid.SetActive(false);
        else
            grid.SetActive(true);
    }
}