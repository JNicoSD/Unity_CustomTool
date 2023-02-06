using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class CustomInspector : EditorWindow
{
    public Inspector_ChangeName inspector_ChangeName = new Inspector_ChangeName();
    public Inspector_SnapObjectToPosition inspector_SnapObjectToPosition = new Inspector_SnapObjectToPosition();
    public Inspector_SortGameObjectByName inspector_SortGameObjectByName = new Inspector_SortGameObjectByName();
    VisualElement root;

    [MenuItem("JNSD Custom Tools/Custom Tools")]
    public static void ShowWindow()
    {
        CustomInspector wnd = GetWindow<CustomInspector>();
        wnd.titleContent = new GUIContent("Custom Tools");
    }
    public VisualTreeAsset editorUI;

    public void CreateGUI()
    {
        editorUI = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/EditorUI.uxml");

        root = rootVisualElement;
        editorUI.CloneTree(root);

        SetUpButtonHandler();
    }

    public void SetUpButtonHandler()
    {
        Button snapButton = root.Q<Button>("snap-button");
        snapButton.clicked += SnapObjectsToPosition;

        Button changeNameButton = root.Q<Button>("change-name-button");
        changeNameButton.clicked += ChangeName;

        Button sortGameObjectByNameButton = root.Q<Button>("sort-by-name-button");
        sortGameObjectByNameButton.clicked += SortGameObjectByName;
    }

    SerializedObject so;
    SerializedProperty buttonChange;

    public void SnapObjectsToPosition()
    {
        //VisualElement root = rootVisualElement;
        inspector_SnapObjectToPosition.SnapObjectsToPosition(root);
    }

    public void ChangeName()
    {
        //VisualElement root = rootVisualElement;
        inspector_ChangeName.ChangeName(root);
    }
    public void SortGameObjectByName()
    {
        inspector_SortGameObjectByName.SortGameObjectByName(root);
    }
/*
    public void ChangeName()
    {
        VisualElement root = rootVisualElement;

        GameObject[] gameObjects = Selection.gameObjects;
        TextField nameField = root.Q<TextField>("change-name-value");
        TextField suffixField = root.Q<TextField>("numeric-name-value");
#if false
        (int, int, int) num = (0,0,0);
        string suffix = suffixField.value;

        num = suffixField.value.Length > 0 ? ParseNumber(suffix) : (-1,-1,-1);
        
        //startIndex or num.Item1 is higher than -1 if there is a number in the suffix
        if(num.Item1 > -1) 
        {
            foreach(GameObject g in gameObjects)
            {
                Undo.RecordObject( g, "Name Change");
                g.name = nameField.value + suffix[0..num.Item1] + num.Item2++ + suffix[(num.Item3+1)..(suffix.Length)];
            }
        }
        else
        {
            foreach(GameObject g in gameObjects)
            {
                Undo.RecordObject( g, "Name Change");
                g.name = nameField.value + suffix;
            }
        }
#endif
        List<string> suffix = ParseNumber(suffixField.value);
        int index = 0;
        foreach(GameObject g in gameObjects)
        {
            Undo.RecordObject( g, "Name Change");
            g.name = nameField.value + suffix[index];
            index++;
        }
    }
#if true
    public List<string> ParseNumber(string name)
    {
        List<string> suffix = new List<string>();

        string input = name;

        string pattern =  @"0|(\d+\.\d+|\d+)";

        var match = Regex.Match(input, pattern, RegexOptions.RightToLeft);

        if (match.Success)
        {
            string numberString = match.Value;
            double incrementValue = 1;
            int decimalLength = 0;
            
            if(numberString.Contains('.'))
            {
                decimalLength = numberString[numberString.IndexOf('.')..(numberString.Length - 1)].Length;
                incrementValue = 1/(Math.Pow(10,decimalLength));
            }
            
            double number = double.Parse(numberString);
            
            int startIndex = numberString.Length > number.ToString().Length
            ? match.Index + numberString.Length - 1
            : input.LastIndexOf(numberString);
            
            string decimalZero = incrementValue < 1 
            ? "." + String.Concat(Enumerable.Repeat("0", decimalLength))
            : "";

            int numberLength = number.ToString().Length;

            string incrementedString = "";
            
            bool wasWholeNum = false;
            
            for (int i = 0; i < 5; i++)
            {
                if(number == (int)number)
                {
                    incrementedString = number.ToString() + decimalZero;
                    wasWholeNum = true;
                }
                else
                {
                    incrementedString = number.ToString().Length < numberLength
                    ? number.ToString() + String.Concat(Enumerable.Repeat("0", numberLength - number.ToString().Length))
                    : number.ToString();
                }
                
                input = input.Remove(startIndex, numberLength);
                
                input = input.Insert(startIndex, incrementedString);
                
                ///// ------------------------- Add To List --------------------------/////
                suffix.Add(input); //////////////////////////////////////////
                ///// ------------------------- Add To List --------------------------/////
                
                input = number.ToString().Length < numberLength
                ? input.Remove(input.IndexOf(incrementedString) , numberLength - number.ToString().Length)
                : input;
                
                numberLength = number.ToString().Length;
                
                number += incrementValue;
                
                if(wasWholeNum == true)
                {
                    input = input.Remove(input.IndexOf(decimalZero), decimalZero.Length);
                    wasWholeNum = false;
                }
            }
        }
        else {
            
        }
        return suffix;
    }
#endif
*/
#if false
    public (int, int, int) ParseNumber(string name)
    {
        bool isANumber;
        bool prevIsNum = false;
        int startIndex = -1, endIndex = -1;
        int lastMoreThanZero = -1;
        bool wasZero = false;
        int temp = 0;

        for(int i = name.Length - 1; startIndex == -1; i--) //check array from the last index
        {
            isANumber = Int32.TryParse(name[i].ToString(), out temp);
            
            if(isANumber == true) //If a number and still looking for the startIndex
            {
                if(endIndex == -1)
                {
                    endIndex = i;
                }
                if(name.Length - 1 == 0 || (i == 0 && startIndex == -1))
                {
                    if(name[i] != '0')
                    {
                        startIndex = i;
                    }
                    else
                    {
                        startIndex = endIndex;
                    }
                }
                if(name[i] != '0') //saves the index of the last char that is not '0'. This is to remove trailing zeros(0) in the beginning of the string.
                {
                    lastMoreThanZero = i;
                }
                wasZero = temp == 0 ? true : false;
                prevIsNum = true;
            }
            else if(isANumber == false) //When it is not a number
            {
                if(prevIsNum == true) //and the previous was, it should end the the search for numbers
                {
                    if(wasZero == false) //If the last number was not zero(0), let that last number's index be the start index.
                    {
                        startIndex = i+1;
                        prevIsNum = false;
                    }
                    else if(lastMoreThanZero != -1)//But if the last found number was zero(0), let the last number that is more than zero be the start index. This is to remove trailing zeros(0) in the beginning of the string.
                    {
                        startIndex = lastMoreThanZero;
                    }
                    else //if all numbers are zero(0), let the zero(0) near the end/ first found zero(0) be the start Index
                    {
                        startIndex = endIndex;
                    }
                }
                if(i == 0 && startIndex == -1)
                {
                    break; //no found numbers. Break the loop
                }
            }
            
        }

        //Assign the number found in the suffix to parsedNumber. If startIndex -1, assign 0 instead.
        //If startIndex is -1, it means that there is no number in the string
        int parsedNumber = startIndex > -1 ? Int32.Parse(name[startIndex..(endIndex+1)]) : 0; 

        return (startIndex, parsedNumber, endIndex);
    }
#endif
#if false
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        editorUI.CloneTree(root);
        return root;
    }
#endif
}
