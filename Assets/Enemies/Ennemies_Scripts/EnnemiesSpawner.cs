using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSpawner : MonoBehaviour
{
    public GameObject[] enemie;
    private const float respawnTimer = 2f;
    public  bool isChomper;
    public  bool isGrenadier;
    public Vector3 startPosChomper;
    public Vector3 startPosGrenadier;
    public void ReviveChomper()
    {
        Instantiate(enemie[0], startPosChomper, Quaternion.identity);
        isChomper = false;

    }
    public void ReviveGrenadier()
    {
        Instantiate(enemie[1], startPosGrenadier, Quaternion.identity);
        isGrenadier = false;

    }


    private void Update()
    {
        if (isChomper)
        {
            if (!IsInvoking(nameof(ReviveChomper)))
            Invoke(nameof(ReviveChomper), respawnTimer);

        }
        else if (isGrenadier)
        {
               if (!IsInvoking(nameof(ReviveGrenadier)))

                Invoke(nameof(ReviveGrenadier), respawnTimer);

        }
    }
}
