using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDoorStatus : MonoBehaviour
{
    public bool doorIsClosed = true;
    [SerializeField] private AudioSource audio;
    private void Start()
    {
        audio = GetComponentInParent<AudioSource>();
    }
    public void DoorIsClosed() 
    { 
        doorIsClosed = true; 
    } //animator event
    public void DoorIsOpened() 
    { 
        doorIsClosed = false; 
    } //animator event
    public void PlayAudio()
    {
        audio.PlayOneShot(audio.clip);
    }
}
