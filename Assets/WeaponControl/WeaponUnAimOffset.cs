using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnAimOffset : MonoBehaviour
{
    private Camera player;


    private void Start()
    {
        player = GetComponentInParent<Camera>();

    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.gameObject.transform.position, player.gameObject.transform.forward, out hit, 50f))
        {
            this.gameObject.transform.LookAt(hit.transform);
        }   
    }
}
