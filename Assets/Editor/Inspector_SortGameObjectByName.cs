using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class Inspector_SortGameObjectByName
{
    List<int> lowestSiblingIndex = new List<int>();
    public void SortGameObjectByName(VisualElement root)
    {
        GameObject[] gameObjects = Selection.gameObjects;
        var objectsToSort = gameObjects.ToList();
        
        objectsToSort.Sort(SortByName); //Sort by name

        for(int i = 0; i < objectsToSort.Count; i++)
        {
            lowestSiblingIndex.Add(objectsToSort[i].transform.GetSiblingIndex());
        }

        lowestSiblingIndex.Sort(); //Sort by number

        for(int i = 0; i < objectsToSort.Count; i++)
        {
            objectsToSort[i].transform.SetSiblingIndex(lowestSiblingIndex[i]);
           // Undo.RecordObject(gameObjects.Select(o => o.transform), "---Can't Undo Hierarchy change---");
            foreach(GameObject g in gameObjects)
            {
                Undo.RecordObject(g, "---Can't Undo Hierarchy change---");
            }
        }

        lowestSiblingIndex.Clear();
    }

    
    public int SortByName(GameObject a, GameObject b) 
    {
        return a.name.CompareTo(b.name);
    }
}
