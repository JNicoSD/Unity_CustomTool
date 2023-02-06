using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
public class Inspector_SnapObjectToPosition
{
    public void SnapObjectsToPosition(VisualElement root)
    {
        //VisualElement root = rootVisualElement;

        GameObject[] gameObjects = Selection.gameObjects;

        Vector3Field snapPosition = root.Q<Vector3Field>("snap-values");

        foreach(GameObject g in gameObjects)
        {
            Undo.RecordObject( g.transform, "Snap Objects");
            g.transform.position = snapPosition.value;
        }
    }
}

