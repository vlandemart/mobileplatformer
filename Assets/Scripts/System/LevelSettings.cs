using UnityEngine;

[CreateAssetMenu(menuName = "Level/Settings")]
public class LevelSettings : ScriptableObject {
    [SerializeField]
    private Vector2 minCameraPos;
    [SerializeField]
    private Vector2 maxCameraPos;

    [SerializeField]
    private bool hasParallax;
    [SerializeField]
    private float perfectTime;

    public Vector2 MinCameraPos
    {
        get
        {
            return minCameraPos;
        }

        set
        {
            minCameraPos = value;
        }
    }

    public Vector2 MaxCameraPos
    {
        get
        {
            return maxCameraPos;
        }

        set
        {
            maxCameraPos = value;
        }
    }

    public bool HasParallax
    {
        get
        {
            return hasParallax;
        }

        set
        {
            hasParallax = value;
        }
    }

    public float PerfectTime
    {
        get
        {
            return perfectTime;
        }

        set
        {
            perfectTime = value;
        }
    }
}
