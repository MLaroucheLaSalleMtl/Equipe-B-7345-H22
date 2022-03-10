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
    Vector3[] pos = new Vector3[2] { new Vector3(4, 0, 10), new Vector3(4, 0, 10) };

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }

    }
    private void Start()
    {
        //Instantiate(enemiesPrefabs[0]   , pos[0], Quaternion.identity);
        //CreateEnemies(1, pos, EnemieType.CHOMPER);
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
    private void CreateEnemies(int number, Vector3[] startPos, EnemieType enemieType)
    {
        for (int i = 0; i < number; i++)
        {
            //Instantiate(currentEnemie, startPos[i], Quaternion.identity);
        }
        
    }

    public IEnumerator EnemieReviver(EnemieData data)
    {
        GameObject enemieToRevive = KindOfEnemie(data.Type);
        yield return new WaitForSeconds(data.Timer);
        print("Donne to instantiate");
        Instantiate(enemieToRevive, data.StartPos, Quaternion.identity);
    }
}
