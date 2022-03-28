using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfMarcPiece : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private MeshRenderer circle;
    [SerializeField] private GameObject piece;
    [SerializeField] private BoxCollider col;
    [SerializeField] private Material closedMat;
    
    void Start()
    {
        col = GetComponent<BoxCollider>();
        circle = GetComponentInChildren<EmptyScript>().GetComponent<MeshRenderer>();
        piece = GameObject.Find("MarcPiece");
        piece.gameObject.SetActive(false);

        if (player.GotMarcPiece)
        {
            //piece.GetComponent<MeshRenderer>().enabled = true;
            //piece.GetComponent<BoxCollider>().enabled = true;
            piece.gameObject.SetActive(true);
            col.enabled = false;
            circle.material = closedMat;
        }
    }
}
