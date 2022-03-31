using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;

public class StartNewLevel : MonoBehaviour
{
    public string levelToOpenName;
    private float blackingScreentimer = 2.5f;
    private Image blackSceen;
    private AudioSource audio;
    private StartCinematic sCine;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        blackSceen = GameObject.Find("BlackScreenTarget").GetComponent<Image>();
        sCine = GetComponent<StartCinematic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            audio.PlayOneShot(audio.clip);
            StartCoroutine(loadLevel());
        }
    }
    IEnumerator loadLevel()
    {
        if(SceneManager.GetActiveScene().name != "TutorialLevel")
        {
            blackSceen.gameObject.GetComponent<Animator>().SetTrigger("Start");
            yield return new WaitForSeconds(blackingScreentimer);
            SceneManager.LoadScene(levelToOpenName);
        }
        else
        {
            blackSceen.gameObject.GetComponent<Animator>().SetTrigger("Start");
            yield return new WaitForSeconds(blackingScreentimer);
            sCine.PlayCine();
            yield return new WaitForSeconds((float)sCine.cutscene.duration + blackingScreentimer);
            SceneManager.LoadScene(levelToOpenName);
        }

    }
}
