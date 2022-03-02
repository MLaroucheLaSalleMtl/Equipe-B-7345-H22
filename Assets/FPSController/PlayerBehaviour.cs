using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDeath();
    }

    void PlayerDeath()
    {
        Vector3 doorCheckpoint = new Vector3(115f, 1f, 32f);

        if (player.HealthPoints <= 0)
        {
            transform.localPosition = doorCheckpoint;
            player.HealthPoints = 100;
        }
    }

    void PlayerLookAt()
    {
        Vector3 doorLookAt = new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z);
        transform.LookAt(doorLookAt);
    }
}
