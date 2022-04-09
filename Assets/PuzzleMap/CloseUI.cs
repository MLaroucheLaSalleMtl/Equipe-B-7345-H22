using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseUI : MonoBehaviour
{
    [SerializeField] private Image canvasTimer;
    [SerializeField] private Image targetTimer;

    private void OnTriggerEnter(Collider other)
    {
        CloseUIFtc();
    }
    public void CloseUIFtc()
    {
        canvasTimer.gameObject.SetActive(false);
        targetTimer.gameObject.SetActive(false);
    }
}
