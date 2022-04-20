using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public int targetAmount = 0;
    public int targetNeeded = 0;
    [SerializeField] private TMP_Text targetCount;
    [SerializeField] private Image canvasTimer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image targetTimer;
    [SerializeField] private TMP_Text targetTimerText;
    public bool targetCountHit = false;
    [SerializeField] private DoorTargets[] allTargets;
    [SerializeField] private InteractWithButton button;
    public bool firstIsHit = false;
    [Range(5f, 200f)] [SerializeField] float resetTime = 5f;

    public bool isSkipped = false;

    private void Awake()
    {
        allTargets = GetComponentsInChildren<DoorTargets>();
        button = GetComponentInChildren<InteractWithButton>();
        
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "PuzzleLevel")
        {
            //canvasTimer = GameObject.Find("TimerDoors").GetComponent<Image>();
            //timerText = GameObject.Find("TimerCount(Text)").GetComponent<TMP_Text>();
            //targetTimer = GameObject.Find("TargetTimer").GetComponent<Image>();
            //targetTimerText = GameObject.Find("TargetTimer(Text)").GetComponent<TMP_Text>();


            canvasTimer.gameObject.SetActive(false);
            targetTimer.gameObject.SetActive(false);
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && anim.GetComponent<CheckDoorStatus>().doorIsClosed)
        {
            anim.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            anim.SetBool("Open", false);
            if (SceneManager.GetActiveScene().name == "PuzzleLevel")
            {
                targetAmount = 0;
                targetCountHit = false;
                button.GetComponent<DoorTargets>().ResetTarget();
            }
        }
    }

    void Update()
    {
        if (targetNeeded > 0 && !targetCountHit)
        {
            targetCount.gameObject.SetActive(true); //on the door
            targetCount.text = targetAmount + "/" + targetNeeded;
        }
        if (targetAmount == targetNeeded && targetNeeded > 0 && anim.GetComponent<CheckDoorStatus>().doorIsClosed)
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
            for (timer = resetTime; timer > 0; timer -= Time.deltaTime)
            {
                if(isSkipped)
                {
                    isSkipped = false;
                    break;
                }
                canvasTimer.gameObject.SetActive(true);
                targetTimer.gameObject.SetActive(true); //on the player
                if (targetAmount != targetNeeded)
                {
                    float temp = (float)Math.Round(timer, 2);
                    timerText.text = "Timer: " + temp.ToString();
                }
                targetTimerText.text = targetAmount + "/" + targetNeeded;
                if (targetAmount == targetNeeded)
                {
                    yield return new WaitForSeconds(1f);
                    canvasTimer.gameObject.SetActive(false);
                    targetTimer.gameObject.SetActive(false); //on the player
                    //StopCoroutine(ResetAllTargets());
                    break;
                }
                yield return null;
            }
            
            //yield return new WaitForSeconds(resetTime);
            if(targetAmount != targetNeeded)
            {
                for (int i = 0; i < allTargets.Length; i++)
                {
                    allTargets[i].ResetTarget();
                    targetAmount = 0;
                    canvasTimer.gameObject.SetActive(false);
                    targetTimer.gameObject.SetActive(false); //on the player
                }
            }
        }
    }
}
