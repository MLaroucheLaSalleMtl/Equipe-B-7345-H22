using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkipButtons : MonoBehaviour
{
    public bool needVerif = true;
    private InteractWithButton button;
    [SerializeField] private TMP_Text verifMessage;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStats PStats;
    [SerializeField] private GameObject[] checkpoints;
    [SerializeField] private CloseUI cUI;

    private void Start()
    {
        button = GetComponent<InteractWithButton>();
        verifMessage.gameObject.SetActive(false);
        needVerif = true;
    }

    public void MovePlayerSkip()
    {
        verifMessage.gameObject.SetActive(false);
        for(int i = 0; i<checkpoints.Length; i++)
        {
            if(checkpoints[i].transform.position == PStats.LastCheckpoint)
            {
                PStats.LastCheckpoint = checkpoints[i].transform.position;
                break;
            }
        }
        player.transform.position = PStats.LastCheckpoint;
        cUI.CloseUIFtc();
        needVerif = true;
    }
    public void NeedVerif()
    {
        verifMessage.gameObject.SetActive(true);
        needVerif = false;
    }

}
