using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public List<Enemie> ListOfChomper = new List<Enemie>();
    [SerializeField] private TMP_Text EnemieCount;
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(this);

        this.CanUseEnemieCounter(false);
    }
    private void Start()
    {
        //Invoke("RemoveAllEnemies", 5f);
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
        //verify
        GameObject enemieToRevive = KindOfEnemie(data.Type);
        yield return new WaitForSeconds(data.Timer);
       var enemieCreated = Instantiate(enemieToRevive, data.StartPos, Quaternion.identity);
        if(data.Type == EnemieType.CHOMPER)
        {
            ListOfChomper.Add(enemieCreated.GetComponent<Enemie>());
        }
    }

    public void RemoveAllEnemies()
    {
       for(int count = ListOfChomper.Count-1; count >= 0 ; count--)
       {
            var current = ListOfChomper[count];
            Destroy(current.gameObject);
            ListOfChomper.Remove(current);
       }
        ListOfChomper = null;
    }

    public void DisplayEnemieCounter()
    {
        if (EnemieCount != null)
            EnemieCount.text = "[count] : " + playerStats.EnemiesCount;
    }
    public void CanUseEnemieCounter(bool isEnable)
    {
        if(EnemieCount != null)
             EnemieCount.gameObject.SetActive(isEnable);
    }
}
