using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDone : MonoBehaviour
{
    [SerializeField] private WeaponDamage damage;
    [SerializeField] private Transform particule;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemie>().ReceiveDamage(damage.Damage);
            other.GetComponent<DisplayDamage>().PrintDamage();
        }
        //OnDestroy();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<DisplayDamageOnTargets>().PrintDamage(damage.Damage);
        }
        OnDestroy();
    }

    private void OnDestroy()
    {
        particule.parent = null;
        particule.GetComponent<DestroyParticule>().DestroyMe();
        Destroy(gameObject);
    }

    public IEnumerator BreakDistance()
    {
        yield return new WaitForSeconds(2.5f);
        if (gameObject != null) OnDestroy();
    }
}
