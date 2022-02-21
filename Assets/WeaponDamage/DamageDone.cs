using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDone : MonoBehaviour
{
    [SerializeField] private WeaponDamage damage;
    [SerializeField] private LayerMask isEnviro;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<Enemie>().ReceiveDamage(this.damage.Damage);
            print(this.damage.Damage);
        }
        if(other.gameObject.layer == isEnviro && other.gameObject.tag == "Enemy")
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
