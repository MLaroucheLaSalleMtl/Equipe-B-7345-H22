using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDamageOnTargets : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private TMP_Text damageText;
    private float totalDamage = 0;
    [SerializeField] private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        
    }
    public void PrintDamage(float damage)
    {
        audio.PlayOneShot(audio.clip);
        totalDamage += damage;
        damageText.text = totalDamage.ToString();
        CancelInvoke("ClearPrint");
        Invoke("ClearPrint", 2.0f);
    }

    public void ClearPrint()
    {
        damageText.text = null;
        totalDamage = 0;
    }

    private void Update()
    {
        damageText.rectTransform.LookAt(player.gameObject.transform);
    }
}
