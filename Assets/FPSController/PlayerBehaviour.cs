using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private TMP_Text hpText;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP: " + player.HealthPoints.ToString() + "/" + player.MaxHP;
    }
}
