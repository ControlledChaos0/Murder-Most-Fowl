using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEditor.PlayerSettings;

[CustomEditor(typeof(PlayerInteractable),true)]
public class PlayerInteractableEditor : Editor
{
    private PlayerInteractable pI;
    private Transform tr;
    private int moveRightId;
    private int moveLeftId;
    private bool cached = false;
    private Vector3 currOffset;
    Vector2 previousMousePosition = Vector3.zero;
    private void OnSceneGUI()
    {
        if (!cached)
        {
            pI = target as PlayerInteractable;
            tr = pI.transform;
            moveRightId = GUIUtility.GetControlID(FocusType.Passive);
            moveLeftId = GUIUtility.GetControlID(FocusType.Passive);
            cached = true;
        }

        currOffset = new Vector3(pI.Radius, 0f, 0f);
        UpdateHandles(moveRightId);
        UpdateHandles(moveLeftId);
    }

    private void UpdateHandles(int moveId)
    {
        var pos = tr.position;
        var evt = Event.current;
        switch (evt.GetTypeForControl(moveId))
        {
            case EventType.Layout:
                {
                    // Set the nearest control ID to "controlID" if the mouse cursor is
                    // near or directly above the solid disc handle.
                    Vector3 offset = moveId == moveRightId ? pos + currOffset : pos - currOffset;
                    Handles.DotHandleCap(
                        moveId,
                        offset,
                        tr.rotation * Quaternion.LookRotation(Vector3.forward),
                        .1f,
                        EventType.Layout
                    );
                    break;
                }
            case EventType.MouseDown:
                {
                    // Log the nearest control ID if the click is near or directly above
                    // the solid disc handle.
                    if (moveId == HandleUtility.nearestControl && evt.button == 0)
                    {
                        Debug.Log($"The nearest control is {moveId}.");
                        previousMousePosition = evt.mousePosition;
                        GUIUtility.hotControl = moveId;
                        evt.Use();
                    }
                    break;
                }
            case EventType.MouseDrag:
                {
                    // Log the nearest control ID if the click is near or directly above
                    // the solid disc handle.
                    if (moveId == GUIUtility.hotControl && evt.button == 0)
                    {
                        Debug.Log($"The nearest control is {moveId}.");
                        Vector3 dir = moveId == moveRightId ? Vector3.right : Vector3.left;
                        float move = HandleUtility.CalcLineTranslation(previousMousePosition, evt.mousePosition, pos, dir);
                        pI.Radius += move;
                        if (pI.Radius < 0.0f)
                        {
                            pI.Radius = 0.0f;
                        }
                        previousMousePosition = evt.mousePosition;
                        evt.Use();
                    }
                    break;
                }
            case EventType.MouseUp:
                {
                    if (GUIUtility.hotControl == moveId && evt.button == 0)
                    {
                        GUIUtility.hotControl = 0;
                        previousMousePosition = Vector3.zero;
                        evt.Use();
                    }
                    break;
                }
            case EventType.Repaint:
                {
                    Vector3 offset = moveId == moveRightId ? pos + currOffset : pos - currOffset;
                    Handles.color = moveId == HandleUtility.nearestControl ? Color.red : Color.blue;
                    Handles.DotHandleCap(
                    moveId,
                    offset,
                    tr.rotation * Quaternion.LookRotation(Vector3.forward),
                        .1f,
                        EventType.Repaint
                    );
                    Handles.DrawLine(pos, offset);

                    break;
                }

        }
    }
}
