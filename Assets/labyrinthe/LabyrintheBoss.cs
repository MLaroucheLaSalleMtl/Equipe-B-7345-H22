using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrintheBoss : MonoBehaviour
{
    [SerializeField] private Enemie Boss;
    [SerializeField] private Scriptable_Stats_Enemies grenadier_stats;
    [SerializeField] GameObject reward;

    private void Start()
    {
        reward.SetActive(false);
    }
    public void ResetBossHp()
    {
         Boss.HealthPoints = grenadier_stats.HealthPoints;
         Boss.AttackPower = grenadier_stats.AttackPower;
        if(Boss.transform.localScale != Vector3.one)
        {
            Boss.transform.localScale = Vector3.one;
        }
    }

    private void DeadBossBeahaviour()
    {
        reward.SetActive(true);
    }
    private void Update()
    {
        if (Boss.HealthPoints <= 0)
            Invoke(nameof(DeadBossBeahaviour), 1f);
    }

}
