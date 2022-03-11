using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBossDoor : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject[] puzzle;
    private Vector3[] gameobjPos;
    private int[] rngNum;
    private int count = 0;
    void Start()
    {
       this.anim = GetComponent<Animator>(); 
    }
    private void Awake()
    {
        this.RandomizePuzzleOrder();
        this.gameobjPos = new Vector3[puzzle.Length];
        this.rngNum = new int[puzzle.Length];
    }
    private void RandomizePuzzleOrder()
    {
        
        for(int i = 0; i< this.gameobjPos.Length; i++)
        {
            
            int num = Random.Range(0, this.puzzle.Length);
            while (!VerifyNumber(num))
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
        foreach(var value in rngNum)
        {
            if (value == num)
                return false;
        }
        return true;
    }

    public void OrderToFollow(Vector3 gameobjectPos)
    {
        if(gameobjectPos == gameobjPos[count])
        {
            this.puzzle[count].SetActive(false);
            this.count++;
        }
        else if(gameobjectPos != gameobjPos[count])
        {
            foreach(var platfrom in puzzle)
            {
                if (!platfrom.activeSelf)
                {
                    platfrom.SetActive(true);
                    //fail sound
                }
            }
        }

        if(count == puzzle.Length)
        {
            anim.SetBool("isOpen", true);
            if(anim.GetCurrentAnimatorStateInfo(0).length > 1f)
            {
                anim.SetBool("isOpen", false);
            }
        }
        
    }

   
}
