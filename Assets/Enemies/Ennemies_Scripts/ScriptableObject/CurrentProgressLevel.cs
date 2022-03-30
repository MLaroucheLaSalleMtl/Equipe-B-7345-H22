using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CurrentProgressLevel : ScriptableObject
{
    public MyEventList[] eventProgress;
    public void ResetForNewGame()
    {
        for(int i = 0; i< eventProgress.Length; i++)
        {
            if (eventProgress[i].isCompleted)
                eventProgress[i].isCompleted = false;
        }
    }
    public string GetLastProgress()
    {
        for (int i = eventProgress.Length; i > 0; i--)
        {
            if (eventProgress[i].isCompleted)
                return eventProgress[i].eventName;
            
        }
        return null;
    }
}


[System.Serializable]
public struct MyEventList
{
    public string eventName;
    public bool isCompleted;
    public Vector3 eventCheckPoint;
}


