using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie_MeleeAttack : MonoBehaviour
{

    private Enemie enemie;
    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.CompareTag("Player"))
    //    {
    //        print(123);
    //        this.enemie.AdaptiveForce(other);
    //        other.gameObject.GetComponentInParent<PlayerController>().Hp -= this.InflictDamage();
    //        //print(other.gameObject.GetComponentInParent<PlayerController>().Hp);
    //    }
    //}
    //private void AdaptiveForce(Collider other)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit))
    //    {
    //        var contact = hit.point - transform.position;

    //        print(transform.position - hit.point);
    //        other.gameObject.GetComponentInParent<Rigidbody>().AddForce(contact.normalized * this.impluseForce, ForceMode.Impulse);
    //    }
    //    //other.GetContact(0).point
    //}
}
