using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePlatfrom : MonoBehaviour
{
    private LabyrinthePuzzleBehaviour bossDoor;

    private void Start()
    {
        bossDoor = LabyrinthePuzzleBehaviour.instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            print("hit");
            bossDoor.OrderToFollow(transform.position);
        }
    }
}
