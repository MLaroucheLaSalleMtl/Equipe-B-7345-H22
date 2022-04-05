    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractWithButton : MonoBehaviour
{
    [SerializeField] private PlayerController control;
    [SerializeField] private TMP_Text interactText;
    public bool canInteract = false;
    [SerializeField] private AudioSource audio;
    [SerializeField] private bool isSkipLevel = false;
    private SkipButtons skipbutton = null;
    // Start is called before the first frame update
    void Start()
    {
        if (isSkipLevel)
        {
            skipbutton = GetComponent<SkipButtons>();
        }
        audio = GetComponent<AudioSource>();
        control = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (control.InteractInput && canInteract && GetComponent<DoorTargets>().interactOnce)
        {
            control.InteractInput = false;
            audio.PlayOneShot(audio.clip);
            GetComponent<DoorTargets>().interactOnce = false;
            canInteract = false;
            GetComponent<DoorTargets>().TargetIsHit();
            interactText.gameObject.SetActive(false);
            if (isSkipLevel)
            {
                if (!skipbutton.needVerif)
                {
                    skipbutton.MovePlayerSkip();
                }
                if (skipbutton.needVerif)
                {
                    skipbutton.NeedVerif();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInteract = true;
            if (isSkipLevel && !skipbutton.needVerif) return;
            interactText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactText.gameObject.SetActive(false);
            canInteract = false;
        }
    }
}
