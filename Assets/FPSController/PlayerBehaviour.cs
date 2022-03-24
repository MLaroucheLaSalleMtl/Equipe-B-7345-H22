using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject door;
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
            transform.position = player.LastCheckpoint;
            player.PlayerArea = "";
            Invoke("ResetHP", 1f);
            DeadInAreaBehaviour();
        }
    }
    private void ResetHP()
    {
        player.HealthPoints = this.player.MaxHP;
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
        Vector3 doorLookAt = new Vector3(door.transform.position.x, transform.position.y, door.transform.position.z);
        transform.LookAt(doorLookAt);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Area"))
        {
            this.player.PlayerArea = other.tag;
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Area") && this.player.PlayerArea != other.tag)
    //    {
    //        this.player.PlayerArea = other.tag;
    //    }

    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Area"))
    //    {
    //        this.player.PlayerArea = "";
    //    }
    //}

}
