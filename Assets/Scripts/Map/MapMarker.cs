using UnityEngine;

public class MapMarker : MonoBehaviour
{
    [SerializeField] private string markerName;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool showOnMap = true;

    public string MarkerName => markerName;
    public Sprite Icon => icon;
    public bool ShowOnMap => showOnMap;
}
