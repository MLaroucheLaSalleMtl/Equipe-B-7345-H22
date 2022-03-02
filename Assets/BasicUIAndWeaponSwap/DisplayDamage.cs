using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Scriptable_Stats_Enemies stats;

    public void PrintDamage()
    {
        damageText.text = GetComponent<Enemie>().HealthPoints.ToString() + "/" + stats.HealthPoints;
        CancelInvoke("ClearPrint");
        Invoke("ClearPrint", 2.0f);
    }

    public void ClearPrint()
    {
        damageText.text = null;
    }

    private void Update()
    {
        damageText.rectTransform.LookAt(player.gameObject.transform);
    }
}
