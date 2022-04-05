using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GetPiece : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private int currentLevel;
    [SerializeField] private AudioSource audio;
    

    void Start()
    {
        audio = GetComponent<AudioSource>();
        Scene scene;
        scene = SceneManager.GetActiveScene();
        currentLevel = scene.buildIndex;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            audio.PlayOneShot(audio.clip);
            switch (currentLevel)
            {
                case 3: player.GotMarcPiece = true;
                    break;
                case 4:
                    player.GotSebPiece = true;
                    break;
                case 5:
                    player.GotStevenPiece = true;
                    break;
                default:
                    break;
            }
        }
    }
}
