using UnityEngine;

[CreateAssetMenu(menuName = "Game/Map Data")]
public class MapData : ScriptableObject
{
    [Header("Map Image")]
    public Sprite mapSprite;

    [Header("World Bounds")]
    public Vector2 worldMin;
    public Vector2 worldMax;

    public Vector2 WorldToNormalizedPosition(Vector3 worldPosition)
    {
        float x = Mathf.InverseLerp(worldMin.x, worldMax.x, worldPosition.x);
        float y = Mathf.InverseLerp(worldMin.y, worldMax.y, worldPosition.z);

        return new Vector2(
            Mathf.Clamp01(x),
            Mathf.Clamp01(y)
        );
    }
}