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
            other.GetComponent<Enemie>().ReceiveDamage(damage.Damage);
            other.GetComponent<DisplayDamage>().PrintDamage();
            Destroy(gameObject);
        }
        if(other.gameObject.layer == 9 || other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 6 || collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Target")
        {
            collision.gameObject.GetComponent<DisplayDamageOnTargets>().PrintDamage(damage.Damage);
            Destroy(gameObject);
        }
    }

    public IEnumerator BreakDistance()
    {
        yield return new WaitForSeconds(2.5f);
        if (gameObject != null) Destroy(gameObject);
    }
}
