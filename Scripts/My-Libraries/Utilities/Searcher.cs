using System;
using System.Collections.Generic;
using PaleLuna.DataHolder;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Searcher
{
    public static List<T> ListOfAllByInterface<T>() where T : class
    {
         MonoBehaviour[] listOfMono = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
         List<T> listOfT = new();
    
         foreach (MonoBehaviour item in listOfMono)
             if (item is T)
                 listOfT.Add(item as T);
    
         return listOfT;
    }
    public static List<T> ListOfAllByInterface<T>(Func<T, bool> predicate) where T : class
    {
        MonoBehaviour[] listOfMono = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        List<T> listOfT = new();

        foreach (MonoBehaviour item in listOfMono)
            if (item is T && predicate(item as T))
                listOfT.Add(item as T);

        return listOfT;
    }   

    private static DataHolder<T> DataHolderOfAllByInterface<T>() where T : class
    {
        return new DataHolder<T>(ListOfAllByInterface<T>());
    }
}
