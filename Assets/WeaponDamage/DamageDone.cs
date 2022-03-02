using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDone : MonoBehaviour
{
    [SerializeField] private WeaponDamage damage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print(damage.Damage);
            other.GetComponent<DisplayDamage>().PrintDamage();
            other.GetComponent<Enemie>().ReceiveDamage(damage.Damage);
            Destroy(gameObject);
        }
        if(other.gameObject.layer == 9 || other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Target")
        {
            other.GetComponent<DisplayDamageOnTargets>().PrintDamage(damage.Damage);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 6 || collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator BreakDistance()
    {
        yield return new WaitForSeconds(2.5f);
        if (gameObject != null) Destroy(gameObject);
    }
}
