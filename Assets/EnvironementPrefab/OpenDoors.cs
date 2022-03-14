using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int targetAmount = 0;
    public int targetNeeded = 0;
    [SerializeField] private TMP_Text targetCount;
    [SerializeField] private Image canvasTimer;
    [SerializeField] private TMP_Text timerText;
    public bool targetCountHit = false;
    [SerializeField] private DoorTargets[] allTargets;
    public bool firstIsHit = false;
    [Range(5f, 200f)] [SerializeField] float resetTime = 5f;

    

    private void Start()
    {
        allTargets = GetComponentsInChildren<DoorTargets>();
        canvasTimer.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("Open", false);
        }
    }

    void Update()
    {
        if (targetNeeded > 0 && !targetCountHit)
        {
            targetCount.gameObject.SetActive(true);
            targetCount.text = targetAmount + "/" + targetNeeded;
        }
        if(targetAmount == targetNeeded && targetNeeded > 0)
        {
            anim.SetBool("Open", true);
            targetCountHit = true;
        }
    }
    float timer;
    public IEnumerator ResetAllTargets()
    {
        if(targetNeeded > 1)
        {
            canvasTimer.gameObject.SetActive(true);
            for (timer = resetTime; timer > 0; timer -= Time.deltaTime)
            {
                if (targetAmount != targetNeeded)
                {
                    float temp = (float)Math.Round(timer, 2);
                    timerText.text = "Timer: " + temp.ToString();
                }
                yield return null;
            }
            //timerText.text = ;
            //yield return new WaitForSeconds(resetTime);
            for (int i = 0; i < allTargets.Length; i++)
            {
                allTargets[i].ResetTarget();
                targetAmount = 0;
                canvasTimer.gameObject.SetActive(false);
            }
        }
    }
}
