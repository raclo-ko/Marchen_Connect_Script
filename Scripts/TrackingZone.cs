using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingZone : MonoBehaviour
{
    public Vector2 minXAndY;
    public Vector2 maxXAndY;
    public Color mainColor = new Color(0.0f, 1.0f, 1.0f, 1.0f);
    
    public Mesh titleMesh;

    [Range(0.0f, 10.0f)]
    public float titleXpos = 1.0f;

    [Range(-1.0f, 1.0f)]
    public float titleYpos = 1.0f;

    [Range(0.0f, 1.0f)]
    public float titleSize = 1.0f;

    private void OnDrawGizmos()
    {
        Color subColor = new Color(mainColor.r, mainColor.g, mainColor.b, 0.1f * mainColor.a);
        Vector2 titlePos = new Vector2(maxXAndY.x - titleXpos, maxXAndY.y + titleYpos);
        Vector2 titleScale = new Vector2(titleSize, titleSize);

        Gizmos.DrawMesh(titleMesh, titlePos, transform.rotation, titleScale);

        for (float x = minXAndY.x; x <= maxXAndY.x; x++)
        {
            if(x-maxXAndY.x == 0 || x - minXAndY.x == 0)
            {
                Gizmos.color = mainColor;
            }
            else
            {
                Gizmos.color = subColor;
            }

            Vector2 pos1 = new Vector2(x, minXAndY.y);
            Vector2 pos2 = new Vector2(x, maxXAndY.y);
            Gizmos.DrawLine(pos1, pos2);
        }
        Gizmos.color = mainColor;
        Vector2 p1 = new Vector2(maxXAndY.x, minXAndY.y);
        Vector2 p2 = new Vector2(maxXAndY.x, maxXAndY.y);
        Gizmos.DrawLine(p1, p2);

        for (float y = minXAndY.y; y <= maxXAndY.y; y++)
        {
            if (y - maxXAndY.y == 0 || y - minXAndY.y == 0)
            {
                Gizmos.color = mainColor;
            }
            else
            {
                Gizmos.color = subColor;
            }

            Vector2 pos1 = new Vector2(minXAndY.x, y);
            Vector2 pos2 = new Vector2(maxXAndY.x, y);
            Gizmos.DrawLine(pos1, pos2);
        }
        Gizmos.color = mainColor;
        p1 = new Vector2(minXAndY.x, maxXAndY.y);
        p2 = new Vector2(maxXAndY.x, maxXAndY.y);
        Gizmos.DrawLine(p1, p2);
    }
}
