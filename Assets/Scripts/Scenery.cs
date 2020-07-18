using UnityEngine;
public class Scenery : MonoBehaviour         //Статичний елемент на мапі
{
    [SerializeField]
    public string nameScenery;          //назва об'єкта
    [SerializeField]
    uint id;                    //ідентифікаційний номер
    [SerializeField]
    ushort size;                //розмір в клітинках (завжди квадрат)

    public string Name { get { return nameScenery; } }
    public uint ID { get { return id; } }
    public ushort Size { get { return size; } }
}