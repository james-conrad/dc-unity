using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResizer : MonoBehaviour {

    public float minWidth = 100.0f;
    public float minHeight = 61.5f;

    Camera attachedCamera;
    int lastScreenWidth;
    int lastScreenHeight;

    // Use this for initialization
    void Start ()
    {
        attachedCamera = GetComponent<Camera>();
        ResizeCamera();
	}

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != lastScreenWidth ||
            Screen.height != lastScreenHeight)
        {
            ResizeCamera();
        }
    }

    void ResizeCamera()
    {
        float currentAspect = (float)Screen.width / (float)Screen.height;
        float desiredAspect = minWidth / minHeight;

        if (currentAspect > desiredAspect)
        {
            // Wider, use the minimum height.
            attachedCamera.orthographicSize = minHeight * 0.5f;
        }
        else
        {
            // Taller, use the minimum width.
            float unitsPerPixel = minWidth / (float)Screen.width;
            float desiredHeight = (float)Screen.height * unitsPerPixel;
            attachedCamera.orthographicSize = desiredHeight * 0.5f;
        }

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }
}
