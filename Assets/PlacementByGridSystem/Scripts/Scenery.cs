using UnityEngine;
public class Scenery : MonoBehaviour
{
    [SerializeField] private string displayedName;
    [SerializeField] private uint id;
    [SerializeField] private ushort size;

    public string DisplayedName => displayedName;
    public uint ID => id;
    public ushort Size => size;
}