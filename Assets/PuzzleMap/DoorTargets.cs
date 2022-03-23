using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTargets : MonoBehaviour
{
    [SerializeField] Material defaultMat;
    [SerializeField] Material hitMat;
    [SerializeField] OpenDoors connectedDoor;
    [SerializeField] public bool interactOnce = true;

    [SerializeField] private bool isFakeButton = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (isFakeButton) return;
        this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
        connectedDoor = GetComponentInParent<OpenDoors>();
        connectedDoor.targetNeeded += 1;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            TargetIsHit();
        }
    }
    public void TargetIsHit()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = hitMat;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Invoke("ResetFakeButtons", 5.0f);
        if (isFakeButton) return;
        connectedDoor.targetAmount += 1;
        if (connectedDoor.targetAmount == 1) connectedDoor.firstIsHit = true;
        if (connectedDoor.firstIsHit)
        {
            connectedDoor.firstIsHit = false;
            StartCoroutine(connectedDoor.ResetAllTargets());
        }
    }
    private void ResetFakeButtons()
    {
        if(isFakeButton)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
            interactOnce = true;
        }
    }
    public void ResetTarget()
    {
        if(!connectedDoor.targetCountHit)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
            interactOnce = true;
        }
    }
}
