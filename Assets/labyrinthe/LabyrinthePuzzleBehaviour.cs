using System.Collections.Generic;
using UnityEngine;

public class LabyrinthePuzzleBehaviour : MonoBehaviour
{
    public static LabyrinthePuzzleBehaviour instance = null;
    [Header("Boss Door")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject SecretWall;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject checkpoint;
    [Header("puzzle gameobject")]
    [SerializeField] private GameObject[] puzzle;
    [SerializeField] private GameObject[] sheetForPuzzle;
    private Vector3[] gameobjPos;
    private int[] rngNum;
    private int count = 0;

    private List<Color> ListOfColor;
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
        this.checkpoint.SetActive(false);
        this.ListOfColor = InitializeListOfColor();
        this.InitialiseRngNumArray();
        this.RandomizePuzzleOrder();
        this.InOrderPlatform(0, 0, this.puzzle.Length-1);
        this.ChargeTheColorRule(0, this.puzzle.Length);

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
            //foreach(var color in ListOfColor)
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
    private void InOrderPlatform(int i,int j, int lastElement)
    {
       if(this.puzzle[lastElement].transform.position == gameobjPos[lastElement])
            return;

        else if (this.puzzle[i].transform.position == this.gameobjPos[j])
        {
            var temp = this.puzzle[j];
            this.puzzle[j] = this.puzzle[i];
            this.puzzle[i] = temp;
            this.InOrderPlatform(0, ++j,this.puzzle.Length-1);
        }
        else
        {
            InOrderPlatform(++i, j, this.puzzle.Length - 1);

        }
    }
    private void InOrderPlatform()
    {

        int lastElement = puzzle.Length - 1;
        int i = 0, j = 0;
        while (this.puzzle[lastElement].transform.position != gameobjPos[lastElement] )
        {
            if (  this.puzzle[i].transform.position == this.gameobjPos[j] )
            {
                var temp = this.puzzle[j];
                this.puzzle[j] = this.puzzle[i];
                this.puzzle[i] = temp;
                j++;
                i = 0;
            }
            else
            {
                i++;
            }

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
        }

        if (count == puzzle.Length)
        {

            for(int i = 0; i <puzzle.Length; i++)
            {
                Destroy(this.puzzle[i]);
                Destroy(this.sheetForPuzzle[i]);
            }
            // Invoke(nameof(TeleportPlayer), 2f);
            //add ui to say that you complete the caleenge before tp
            SetDoorAnim(true);
            // Destroy(this.SecretWall, 2f);
            this.sheetForPuzzle = null;
            this.puzzle = null;
            //this.gameobjPos = null;
            this.checkpoint.SetActive(true);
        }
    }

    public void SetDoorAnim(bool isOpen)
    {
        anim.SetBool("isOpen", isOpen);
    }

    //private void TeleportPlayer()
    //{
    //    this.playerStats.LastCheckpoint = this.whereToTp.position;
    //    this.player.transform.position = this.whereToTp.position;
    //}

    #endregion

}
