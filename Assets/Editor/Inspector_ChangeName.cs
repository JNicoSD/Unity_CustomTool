using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class Inspector_ChangeName
{
    public void ChangeName(VisualElement root)
    {
        GameObject[] gameObjects = Selection.gameObjects;

        var gameObjectList = gameObjects.ToList();

        gameObjectList.Sort(SortBySiblingIndex);

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
        List<string> suffix = ParseNumber(suffixField.value, gameObjectList.Count - 1);
        int index = 0;
        foreach(GameObject g in gameObjectList)
        {
            Undo.RecordObject( g, "Name Change");
            g.name = $"{nameField.value} {suffix[index]}";
            index++;
        }
    }
public int SortBySiblingIndex(GameObject a, GameObject b) 
    {
        return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
    }
    public List<string> ParseNumber(string name, int objectLength)
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
            ? match.Index + (numberString.Length - number.ToString().Length)
            : input.LastIndexOf(numberString);
            
            string decimalZero = incrementValue < 1 
            ? "." + String.Concat(Enumerable.Repeat("0", decimalLength))
            : "";

            int numberLength = number.ToString().Length;

            string incrementedString = "";
            
            for (int i = 0; i <= objectLength; i++)
            {
                if(number == (int)number)
                {
                    incrementedString = number.ToString() + decimalZero;
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
                suffix.Add(input); ////////////////////////////////////////////////////////
                ///// ------------------------- Add To List --------------------------/////
                
                input = number.ToString().Length < numberLength
                ? input.Remove(input.IndexOf(incrementedString) , numberLength - number.ToString().Length)
                : input;
                
                numberLength = number.ToString().Length;
                
                number += incrementValue;
            }
        }
        else {
            for (int i = 0; i <= objectLength; i++)
            {
                suffix.Add(input);
            }
        }
        return suffix;
    }

}
