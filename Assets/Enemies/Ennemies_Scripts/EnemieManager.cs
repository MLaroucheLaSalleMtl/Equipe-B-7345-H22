using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<EnemieData> ListOfEnemieData = new List<EnemieData>();

    [SerializeField] private GameObject[] enemiesPrefabs; //[0] chomper , [1] grenadier
    Vector3[] pos = new Vector3[2] { new Vector3(4, 0, 10), new Vector3(4, 0, 10) };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        ListOfEnemieData = new List<EnemieData>();
    }
    private void Start()
    {
        //Instantiate(enemiesPrefabs[0]   , pos[0], Quaternion.identity);
        //CreateEnemies(1, pos, EnemieType.CHOMPER);
    }

    //private GameObject KindOfEnemie(EnemieType enemieType)
    //{
    //    GameObject currentEnemie = null;

    //    if (enemieType == EnemieType.CHOMPER)
    //        currentEnemie = enemiesPrefabs[0];
    //    else if (enemieType == EnemieType.GRENADIER)
    //        currentEnemie = enemiesPrefabs[1];

    //    return currentEnemie;

    //    //else  return null;

    //}
    //private void CreateEnemies(int number,Vector3[] startPos, EnemieType enemieType)
    //{
    //    GameObject currentEnemie = null;

    //    if (enemieType == EnemieType.CHOMPER)
    //        currentEnemie = enemiesPrefabs[0];
    //    else if (enemieType == EnemieType.GRENADIER)
    //        currentEnemie = enemiesPrefabs[1];

    //    for (int i = 0; i < number; i++)
    //        {
    //            Instantiate(currentEnemie, startPos[i], Quaternion.identity);
    //        }
    //    //}
    //}
    //private void Update()
    //{
    //    //if (this.ListOfEnemieData.Count > 0)
    //    //StartCoroutine(EnemieReviver());
    //}
    //private void Reviver()
    //{
    //    foreach (EnemieData data in ListOfEnemieData)
    //    {
    //        GameObject enemieToRevive = KindOfEnemie(data.Type);
    //        enemieToRevive.GetComponent<Enemie>().Startpos = data.StartPos;

    //    }
    //}

    //private IEnumerator EnemieReviver()
    //{
    //    foreach (EnemieData data in ListOfEnemieData)
    //    {
    //        GameObject enemieToRevive = KindOfEnemie(data.Type);
    //        yield return new WaitForSeconds(data.Timer);
    //        Instantiate(enemieToRevive, data.StartPos, Quaternion.identity);
    //        this.ListOfEnemieData.Remove(data);
           
    //     }
    //}


}
