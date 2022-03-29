using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabyrinthePuzzleBehaviour : MonoBehaviour
{
    public static LabyrinthePuzzleBehaviour instance = null;
    private EnemieManager enemieManager;

    [Header("Boss Door")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject SecretWall;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject checkpoint;
    [Header("puzzle gameobject")]
    [SerializeField] private GameObject[] puzzle;
    [SerializeField] private GameObject[] sheetForPuzzle;
    [SerializeField] private GameObject lifeBourne;
    [Header("Puzzle completed  Text")]
    [SerializeField] private GameObject puzzleDone_txt;

    //private value
    private Vector3[] gameobjPos;
    private int[] rngNum;
    private int count = 0;

    private List<Color> ListOfColor;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
        //get enemieManager instance
        this.enemieManager = EnemieManager.instance;
        //gameobject in the scene
        this.checkpoint.SetActive(false);
        this.lifeBourne.SetActive(false);
        //system to generate a random puzzle platform tosolve
        this.ListOfColor = InitializeListOfColor();
        this.InitialiseRngNumArray();
        this.RandomizePuzzleOrder();
        this.InOrderPlatform(0, 0, this.puzzle.Length - 1);
        this.ChargeTheColorRule(0, this.puzzle.Length);
    }

    private void Start()
    {
        this.EnableEnemieCount();

    }

    private void EnableEnemieCount()
    {
        this.playerStats.EnemiesCount = 0;
        this.enemieManager.CanUseEnemieCounter(true);
        this.enemieManager.DisplayEnemieCounter();
    }

    private List<Color> InitializeListOfColor()
    {
        List<Color> ListOfColor = new List<Color>();
        ListOfColor.Add(Color.red);
        ListOfColor.Add(Color.blue);
        ListOfColor.Add(Color.green);
        ListOfColor.Add(Color.yellow);
        ListOfColor.Add(Color.cyan);
        ListOfColor.Add(Color.magenta);
        ListOfColor.Add(Color.gray);
        return ListOfColor;
    }
    
    private void InitialiseRngNumArray()
    {
        //initialise to -1 to prevent stackoverflow default value is 0
        this.rngNum = new int[this.puzzle.Length];
        for (int i = 0; i < this.puzzle.Length; i++)
        {
            this.rngNum[i] = -1;
        }
    }

    #region Randomize platfromOrder
    private void RandomizePuzzleOrder()
    {
        this.gameobjPos = new Vector3[this.puzzle.Length];
      
        for (int i = 0; i< this.puzzle.Length; i++)
        {
            int num = Random.Range(0, this.puzzle.Length);
            while (VerifyNumber(num))
            {
                num = Random.Range(0, this.puzzle.Length);
            }
            this.rngNum[i] = num;
            this.gameobjPos[i] = this.puzzle[this.rngNum[i]].transform.position;
        }
        this.rngNum = null;
    }
    private bool VerifyNumber(int num)
    {
        for (int i = 0; i < this.puzzle.Length; i++)
        {
            if (num == rngNum[i])
                return true;
        }
        return false;
    }
    #endregion
   
    private void ChargeTheColorRule(int i ,int lastIndex)
    {
        if (i == lastIndex)
        {
            //foreach (Color color in ListOfColor)
            //{
            //    ListOfColor.Remove(color);
            //}
            ListOfColor = null;
            return;
        }
        int index = Random.Range(0, this.ListOfColor.Count - 1);
        this.puzzle[i].gameObject.GetComponent<Renderer>().material.color = this.ListOfColor[index];
        this.sheetForPuzzle[i].gameObject.GetComponent<Renderer>().material.color = this.ListOfColor[index];
        this.ListOfColor.RemoveAt(index);

        ChargeTheColorRule(++i, this.puzzle.Length );
    }

    #region OrderPlatform
    private void InOrderPlatform(int currentElement,int index, int lastElement)
    {
       if(this.puzzle[lastElement].transform.position == gameobjPos[lastElement])
            return;

        else if (this.puzzle[currentElement].transform.position == this.gameobjPos[index])
        {
            var temp = this.puzzle[index];
            this.puzzle[index] = this.puzzle[currentElement];
            this.puzzle[currentElement] = temp;
            this.InOrderPlatform(0, ++index,this.puzzle.Length-1);
        }
        else
        {
            InOrderPlatform(++currentElement, index, this.puzzle.Length - 1);
        }
    }
   
    #endregion


    #region Rule for the player
    public void OrderToFollow(Vector3 gameobjectPos)
    {
        if(gameobjectPos == gameobjPos[count])
        {
            print(gameobjectPos + "==== " + gameobjPos[count]);
            foreach (var platfrom in puzzle)
            {
                if(platfrom.transform.position == gameobjectPos)
                {
                    platfrom.SetActive(false);
                    this.count++;
                    break;
                }
            }
        }
        else if(gameobjectPos != gameobjPos[count])
        {
            this.count = 0;
            foreach (var platfrom in puzzle)
            {
                if (!platfrom.activeSelf)
                {
                    platfrom.SetActive(true);
                    //fail sound
                }
            }
            return;
        }

        if (count == puzzle.Length)
        {

            for(int i = 0; i <puzzle.Length; i++)
            {
                Destroy(this.puzzle[i]);
                Destroy(this.sheetForPuzzle[i]);
            }
            StartCoroutine(PuzzleCompletedDisplay());
            Invoke(nameof(BossEvent), 1f);
            this.sheetForPuzzle = null;
            this.puzzle = null;
        }
    }
    private IEnumerator PuzzleCompletedDisplay()
    {
        this.puzzleDone_txt.SetActive(true);
        this.puzzleDone_txt.GetComponent<TMP_Text>().text = "Puzzle completed - Secret door open";
        yield return new WaitForSeconds(3f);
        Destroy(puzzleDone_txt);
    }

    private void BossEvent()
    {
        SetDoorAnim(true);
        this.enemieManager.RemoveAllEnemies();
        this.enemieManager.CanUseEnemieCounter(false);
        this.playerStats.EnemiesCount = 0;
        this.checkpoint.SetActive(true);
        this.lifeBourne.SetActive(true);
    }

    public void SetDoorAnim(bool isOpen)
    {
        anim.SetBool("isOpen", isOpen);
    }
    #endregion

}
