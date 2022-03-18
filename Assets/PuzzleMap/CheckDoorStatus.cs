using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDoorStatus : MonoBehaviour
{
    public bool doorIsClosed = true;
    public void DoorIsClosed() 
    { 
        doorIsClosed = true; 
    } //animator event
    public void DoorIsOpened() 
    { 
        doorIsClosed = false; 
    } //animator event
}
