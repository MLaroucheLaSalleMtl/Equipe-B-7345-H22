using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCinematic : MonoBehaviour
{
    public PlayableDirector cutscene;
    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject camera;
    [SerializeField] private PlayerController player;
    private void Start()
    {
        ship.SetActive(false);
        camera.SetActive(false);
        player.gameObject.SetActive(true);
    }
    public void PlayCine()
    {
        player.gameObject.SetActive(false);
        ship.SetActive(true);
        camera.SetActive(true);
        cutscene.Play();
    }
}
