using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private Image mapImage;
    [SerializeField] private RectTransform mapRect;
    [SerializeField] private RectTransform playerMarker;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private Transform player;

    [Header("Map")]
    [SerializeField] private MapData mapData;

    [Header("Input")]
    [SerializeField] private KeyCode toggleKey = KeyCode.M;

    private bool isOpen;
    private readonly List<GameObject> markerObjects = new List<GameObject>();

    private void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        if (mapPanel != null)
            mapPanel.SetActive(false);

        ApplyMapData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleMap();
        }

        if (isOpen)
        {
            UpdatePlayerMarker();
        }
    }

    public void ToggleMap()
    {
        isOpen = !isOpen;

        if (mapPanel != null)
            mapPanel.SetActive(isOpen);

        if (isOpen)
        {
            ApplyMapData();
            RebuildMarkers();
            UpdatePlayerMarker();
        }
    }

    private void ApplyMapData()
    {
        if (mapData == null) return;

        if (mapImage != null)
            mapImage.sprite = mapData.mapSprite;
    }

    private void UpdatePlayerMarker()
    {
        if (player == null || playerMarker == null || mapData == null)
            return;

        playerMarker.anchoredPosition = WorldToMapPosition(player.position);
    }

    private void RebuildMarkers()
    {
        ClearMarkers();

        if (markerPrefab == null || mapRect == null || mapData == null)
            return;

        MapMarker[] markers = FindObjectsOfType<MapMarker>();

        foreach (MapMarker marker in markers)
        {
            if (!marker.ShowOnMap)
                continue;

            GameObject markerObject = Instantiate(markerPrefab, mapRect);

            RectTransform rectTransform = markerObject.GetComponent<RectTransform>();

            if (rectTransform != null)
                rectTransform.anchoredPosition = WorldToMapPosition(marker.transform.position);

            Image markerImage = markerObject.GetComponent<Image>();

            if (markerImage != null && marker.Icon != null)
                markerImage.sprite = marker.Icon;

            markerObjects.Add(markerObject);
        }
    }

    private Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        Vector2 normalizedPosition = mapData.WorldToNormalizedPosition(worldPosition);

        float x = (normalizedPosition.x - 0.5f) * mapRect.rect.width;
        float y = (normalizedPosition.y - 0.5f) * mapRect.rect.height;

        return new Vector2(x, y);
    }

    private void ClearMarkers()
    {
        foreach (GameObject markerObject in markerObjects)
        {
            if (markerObject != null)
                Destroy(markerObject);
        }

        markerObjects.Clear();
    }

    public void SetMapData(MapData newMapData)
    {
        mapData = newMapData;
        ApplyMapData();

        if (isOpen)
        {
            RebuildMarkers();
            UpdatePlayerMarker();
        }
    }
}