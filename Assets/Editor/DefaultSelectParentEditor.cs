using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultSelectParent))]
public class DefaultSelectParentEditor : Editor
{
    private void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(0);

        var t = (target as DefaultSelectParent);
        //Get the transform of the component with the selection base attribute
        Transform selectionBaseTransform = t.transform;

        //Detect mouse events
        if (Event.current.type == EventType.MouseDown)
        {
            //get picked object at mouse position
            GameObject pickedObject = HandleUtility.PickGameObject(Event.current.mousePosition, true);

            //If selected null or a non child of the component gameobject
            if (pickedObject == null || !pickedObject.transform.IsChildOf(selectionBaseTransform))
            {
                //Set selection to the picked object
                Selection.activeObject = pickedObject;
            }
        }
    }
}
