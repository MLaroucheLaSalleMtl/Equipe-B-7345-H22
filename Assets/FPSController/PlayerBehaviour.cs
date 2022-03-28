using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private UnityEvent m_labEvent; //labyrinthe ----- BossBoxCollider, labPuzlle.dooranim

    private void Awake()
    {
        player.HealthPoints = this.player.MaxHP;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerDeath();
        hpText.text = "HP: " + player.HealthPoints.ToString() + "/" + player.MaxHP;
        
    }
    //changed
    void PlayerDeath()
    {
        

        if (player.HealthPoints <= 0)
        {
            player.HealthPoints = this.player.MaxHP;
            transform.position = player.LastCheckpoint;
            //player.PlayerArea = "";
            DeadInAreaBehaviour();
        }
    }

    private void DeadInAreaBehaviour()
    {
        if(player.PlayerArea == "BossArea")
        {
            m_labEvent.Invoke();
        }
    }



    void PlayerLookAt()
    {
      //  Vector3 doorLookAt = new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z);
       // transform.LookAt(doorLookAt);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Area"))
        {
            this.player.PlayerArea = other.tag;
        }
    }

}
