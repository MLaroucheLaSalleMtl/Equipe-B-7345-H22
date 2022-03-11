using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTargets : MonoBehaviour
{
    [SerializeField] Material defaultMat;
    [SerializeField] Material hitMat;
    [SerializeField] OpenDoors connectedDoor;
    [SerializeField] public bool interactOnce = true;
    // Start is called before the first frame update
    void Awake()
    {
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
        connectedDoor.targetAmount += 1;
        if (connectedDoor.targetAmount == 1) connectedDoor.firstIsHit = true;
        if (connectedDoor.firstIsHit)
        {
            connectedDoor.firstIsHit = false;
            StartCoroutine(connectedDoor.ResetAllTargets());
            print("help");
        }
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
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
