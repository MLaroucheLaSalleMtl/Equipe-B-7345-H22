    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractWithButton : MonoBehaviour
{
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private bool canInteract = false;
    [SerializeField] public bool interactOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        interactOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canInteract && interactOnce)
        {
            interactOnce = false;
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
