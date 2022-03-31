using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance = null;
    
    [SerializeField] private CurrentProgressLevel currentProgress;
    [SerializeField] private Transform []checkpointPos;
    private CheckPointInfo[] ListOfProgress;

    public CurrentProgressLevel CurrentLevel { get => currentProgress;}

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(this);


        this.ListOfProgress = currentProgress.eventProgress;
        AssignEventPosition();
    }
    private void AssignEventPosition()
    {
        //this mean that all position are set correclty
        if (ListOfProgress[ListOfProgress.Length - 1].checkPoint != Vector3.zero) 
            return;

            for (int i = 0; i < checkpointPos.Length; i++)
            {
                ListOfProgress[i].checkPoint = checkpointPos[i].position;
            }
    }

    public bool UpdateProgress(Vector3 checkPointPos)
    {
        for(int i = 0; i <ListOfProgress.Length; i++)
        {
            if (ListOfProgress[i].checkPoint == checkPointPos)
            {
                ListOfProgress[i].isCompleted = true;
                return true;
            }
        }
        return false;
    }



}
