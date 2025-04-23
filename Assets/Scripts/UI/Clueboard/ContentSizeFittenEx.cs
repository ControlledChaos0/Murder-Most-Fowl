using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ContentSizeFitterEx : ContentSizeFitter
{

    // Define the min and max size
    public Vector2 sizeMin = new Vector2(0f, 0f);
    public Vector2 sizeMax = new Vector2(1920f, 1080f);


    public override void SetLayoutHorizontal()
    { // Override for width
        base.SetLayoutHorizontal();
        // get the rectTransform
        var rectTransform = transform as RectTransform;
        var sizeDelta = rectTransform.sizeDelta; // get the size delta
        // Clamp the x value based on the min and max size
        sizeDelta.x = Mathf.Clamp(sizeDelta.x, sizeMin.x, sizeMax.x);
        // set the size with current anchors to avoid possible problems.
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeDelta.x);
    }


    public override void SetLayoutVertical()
    { // Override for height
        base.SetLayoutVertical();
        // get the rectTransform
        var rectTransform = transform as RectTransform;
        var sizeDelta = rectTransform.sizeDelta; // get the size delta
        // Clamp the y value based on the min and max size
        sizeDelta.y = Mathf.Clamp(sizeDelta.y, sizeMin.y, sizeMax.y);
        // set the size with current anchors to avoid possible problems.
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeDelta.y);
    }
}


[CustomEditor(typeof(ContentSizeFitterEx))]
public class ContentSizeFitterExEditor : Editor
{
    // override the editor to be able to show the public variables on the inspector.
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}