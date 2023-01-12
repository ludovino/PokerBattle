using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int index = list.Count;  
        while (index > 1) 
        {  
            index--;  
            int swapIndex = Random.Range(0, index + 1);
            T item = list[swapIndex];  
            list[swapIndex] = list[index];  
            list[index] = item;  
        }
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string RemoveWhitespace(this string input)
    {
        return sWhitespace.Replace(input, string.Empty);
    }

    public static void DestroyChildren(this Transform transform, float secondsDelay = 0)
    {
        for(int i = transform.childCount - 1; i >=0; i--)
        {
            Object.Destroy(transform.GetChild(i).gameObject, secondsDelay);
        }
    }
}
