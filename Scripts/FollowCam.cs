using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private Transform camTargetTr;
    private Vector2 minRange;
    private Vector2 maxRange;

    [Range(0.0f, 2.0f)]
    public float distX = 1.0f;

    [Range(0.0f, 2.0f)]
    public float distY = 2.0f;

    [Range(0.0f, 30.0f)]
    public float smoothX = 5.0f;

    [Range(1.0f, 10.0f)]
    public float smoothY = 5.0f;

    private TrackingZone trackingZone;

    // Start is called before the first frame update
    void Awake()
    {
        camTargetTr = GameObject.FindWithTag("CameraTarget").transform;
        trackingZone = GameObject.Find("Gizmo_TrackingZone").GetComponent<TrackingZone>();
        minRange = trackingZone.minXAndY;
        maxRange = trackingZone.maxXAndY;
        transform.position = new Vector3(minRange.x, minRange.y, transform.position.z);
    }

    bool CheckDistanceX()
    {
        return Mathf.Abs(transform.position.x - camTargetTr.position.x) > distX;
    }

    bool CheckDistanceY()
    {
        return Mathf.Abs(transform.position.y - camTargetTr.position.y) > distY;
    }

    void CameraTracking()
    {
        float camPosX = transform.position.x;
        float camPosY = transform.position.y;

        if (CheckDistanceX())
        {
            camPosX = Mathf.Lerp(transform.position.x, camTargetTr.position.x, smoothX * Time.deltaTime);
        }
        if (CheckDistanceY())
        {
            camPosY = Mathf.Lerp(transform.position.y, camTargetTr.position.y, smoothY * Time.deltaTime);
        }

        camPosX = Mathf.Clamp(camPosX, minRange.x, maxRange.x);
        camPosY = Mathf.Clamp(camPosY, minRange.y, maxRange.y);

        transform.position = new Vector3(camPosX, camPosY, transform.position.z);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraTracking();
    }
}
