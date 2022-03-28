using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnemieType { CHOMPER, GRENADIER };
public struct EnemieData
{

    EnemieType type;
    Vector3 startPos;
    float timer;
    public EnemieData(EnemieType type, Vector3 startPosition, float reviveTimer)
    {
        this.type = type;
        this.startPos = startPosition;
        this.timer = reviveTimer;
    }

    public EnemieType Type { get => type; set => type = value; }
    public Vector3 StartPos { get => startPos; set => startPos = value; }
    public float Timer { get => timer; set => timer = value; }
}
public class EnemieManager : MonoBehaviour
{
    public static EnemieManager instance = null;
    [SerializeField] private GameObject[] enemiesPrefabs; //[0] chomper , [1] grenadier

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(this);

        }
    }
    private void Start()
    {
        //Instantiate(enemiesPrefabs[0], new Vector3(-49.0200005f, 0f, 40.0900002f), Quaternion.identity);
    }


    private GameObject KindOfEnemie(EnemieType enemieType)
    {
        GameObject currentEnemie = null;

        if (enemieType == EnemieType.CHOMPER)
            currentEnemie = enemiesPrefabs[0];
        else if (enemieType == EnemieType.GRENADIER)
            currentEnemie = enemiesPrefabs[1];

        return currentEnemie;
    }
    

    public IEnumerator EnemieReviver(EnemieData data)
    {
        //GameObject enemieToRevive = KindOfEnemie(data.Type);
        yield return new WaitForSeconds(data.Timer);
        Instantiate(enemiesPrefabs[0], data.StartPos, Quaternion.identity);
    }
}
