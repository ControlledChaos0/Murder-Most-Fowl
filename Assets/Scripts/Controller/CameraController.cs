using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera currentVirtualCamera;
    [SerializeField] private LayerMask _hitMask;

    private Ray _cameraRayOut;

    public Camera MainCamera
    {
        get => mainCamera;
        private set => mainCamera = value;
    }
    public Transform CameraTransform {
        get => mainCamera.transform;
    }
    public Ray CameraRay
    {
        get => _cameraRayOut;
    }

    public LayerMask HitMask
    {
        get => _hitMask;
        set => _hitMask = value;
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentVirtualCamera.Priority = 100;
    }
    public PointerEventData Raycast(Vector2 screenPos)
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = screenPos
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        //bool ui = results.Where(r => r.gameObject.layer == LayerMask.NameToLayer("UI")).Count() > 0;

        //if (ui)
        //{
        //    return null;
        //}

        _cameraRayOut = MainCamera.ScreenPointToRay(screenPos);
        eventData.position = _cameraRayOut.origin;
        eventData.pointerClick = results.Count > 0 ? results[0].gameObject : null;
        return eventData;
    }

    public void SwitchCurrentCamera(CinemachineVirtualCamera virtualCamera)
    {
        currentVirtualCamera.Priority = 10;
        currentVirtualCamera = virtualCamera;
        currentVirtualCamera.Priority = 100;
    }
}
