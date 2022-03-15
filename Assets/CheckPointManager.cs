using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& playerStats.LastCheckpoint != transform.position)
        {
            this.playerStats.LastCheckpoint = transform.position;
        }
    }
}