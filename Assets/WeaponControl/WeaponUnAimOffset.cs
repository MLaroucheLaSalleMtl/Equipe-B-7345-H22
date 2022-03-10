using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnAimOffset : MonoBehaviour
{
    private Camera player;
    public Transform target;

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
            if(Vector3.Distance(hit.point, player.transform.position) <= 1.5f)
            {
                Vector3 offset = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z + 1.5f);
                target.localPosition = offset;
            }
            else
            {
                target.position = hit.point;
            }
            
            //this.gameObject.transform.LookAt(target);
            //if (hit.collider.tag == "Target")
            //{
                
            //}
        }   
    }
}
