using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBehavior : Attack
{
    [SerializeField] private WeaponDamage damage;
    protected float resetTimeSwing = 0.9f; //time between each individual swings
    protected float range = 2.0f;
    private bool rayActivated = false;
    private AudioSource audio;
    [SerializeField] AudioClip swingSound;

    private void OnEnable()
    {
        attackOnce = true;
        isAiming = false;
        isShooting = false;
        isReloading = false;
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        DisplayUI();
        Debug.DrawRay(base.player.transform.position, base.player.transform.forward * range, Color.red);
        RaycastHit hit;
        if (control.FireInput && attackOnce)
        {
            control.FireInput = false;
            audio.clip = swingSound;
            audio.PlayOneShot(audio.clip);
            base.Attacking("Swing", resetTimeSwing);
        }
        if (rayActivated)
        {
            if (Physics.Raycast(base.player.gameObject.transform.position, base.player.gameObject.transform.forward, out hit, range))
            {
                if (hit.collider.tag == "Enemy")
                {
                    print(damage.Damage);
                    hit.collider.gameObject.GetComponent<DisplayDamage>().PrintDamage();
                    hit.collider.gameObject.GetComponent<Enemie>().ReceiveDamage(damage.Damage);
                    DeactivateRay();
                }
                if(hit.collider.tag == "Target")
                {
                    hit.collider.gameObject.GetComponent<DisplayDamageOnTargets>().PrintDamage(damage.Damage);
                    DeactivateRay();
                }
            }
        }
    }

    public void ActivateRay()
    {
        rayActivated = true;
    }
    public void DeactivateRay()
    {
        rayActivated = false;
    }
}
