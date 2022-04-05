using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClossingWallBoss : MonoBehaviour
{
   private LabyrinthePuzzleBehaviour door;
    [SerializeField] private BoxCollider coll;
    private void Start()
    {
        CollTrigger(true);
        door = LabyrinthePuzzleBehaviour.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.SetDoorAnim(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollTrigger(false);
        }
    }


    public void CollTrigger(bool isTrigger)
    {
        coll.isTrigger = isTrigger;
    }
}
