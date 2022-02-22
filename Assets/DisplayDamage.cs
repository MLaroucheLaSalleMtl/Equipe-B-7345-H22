using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayDamage : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private TMP_Text damageText;
    private float totalDamage = 0;

    public void PrintDamage(float damage)
    {
        totalDamage += damage;
        damageText.text = totalDamage.ToString();
        //StartCoroutine(ClearPrint());
    }

    public IEnumerator ClearPrint()
    {
        yield return new WaitForSeconds(2.0f);
        damageText.text = null;
        totalDamage = 0;
    }

    private void Update()
    {
        damageText.rectTransform.LookAt(player.gameObject.transform);
    }
}
