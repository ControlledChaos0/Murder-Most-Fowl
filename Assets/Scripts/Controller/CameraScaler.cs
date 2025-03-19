using System;
using UnityEngine;


[ExecuteInEditMode]
public class CameraScaler : MonoBehaviour
{
    private Camera _camera;

    private float _currentRatio;
    private float _prevRatio;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _prevRatio = 0.0f;
        _currentRatio = (float)Screen.width / (float)Screen.height;
    }

    private void Update()
    {
        _currentRatio = (float)Screen.width / (float)Screen.height;
        //TODO - store old screen width and height and only call if they have changed
        if (Math.Abs(_prevRatio - _currentRatio) > 0.0001)
        {
            ResizeCamera();
        }
    }

    private void ResizeCamera()
    {
        float targetRatio = 16 / 9f;
        float scaleheight = _currentRatio / targetRatio;

        Rect rect = _camera.rect;

        if (scaleheight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
        }

        _camera.rect = rect;

        _prevRatio = _currentRatio;
    }
}