using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CurrentProgressLevel : ScriptableObject
{
    public string LevelName;
    public CheckPointInfo[] eventProgress;
    public void ResetForNewGame()
    {
        for(int i = 0; i< eventProgress.Length; i++)
        {
            if (eventProgress[i].isCompleted)
                eventProgress[i].isCompleted = false;
        }
    }
    public Vector3 GetLastProgress()
    {
        for (int i = eventProgress.Length -1 ; i > 0; i--)
        {
            if (eventProgress[i].isCompleted)
                return eventProgress[i].checkPoint;
            
        }
        return eventProgress[0].checkPoint; // startPoint in the map
    } 
    
    public string GetLastProgressName()
    {
        for (int i = eventProgress.Length -1 ; i > 0; i--)
        {
            if (eventProgress[i].isCompleted)
                return eventProgress[i].CheckPointName;
            
        }
        return eventProgress[0].CheckPointName; // startPoint in the map
    }
}


[System.Serializable]
public struct CheckPointInfo
{
   [SerializeField] private string checkPointName;
    public bool isCompleted;
    public Vector3 checkPoint;

    public string CheckPointName { get => checkPointName; }
}


