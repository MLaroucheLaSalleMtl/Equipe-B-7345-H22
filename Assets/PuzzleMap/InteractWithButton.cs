    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractWithButton : MonoBehaviour
{
    [SerializeField] private PlayerController control;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private bool canInteract = false;
    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(control.InteractInput && canInteract && GetComponent<DoorTargets>().interactOnce)
        {
            control.InteractInput = false;
            GetComponent<DoorTargets>().interactOnce = false;
            canInteract = false;
            GetComponent<DoorTargets>().TargetIsHit();
            interactText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            interactText.gameObject.SetActive(true);
            canInteract = true;
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
