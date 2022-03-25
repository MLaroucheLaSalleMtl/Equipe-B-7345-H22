using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartNewLevel : MonoBehaviour
{
    public int levelToOpenID;
    private float blackingScreentimer = 2.5f;
    private Image blackSceen;
    private AudioSource audio;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        blackSceen = GameObject.Find("BlackScreenTarget").GetComponent<Image>();
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
        blackSceen.gameObject.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(blackingScreentimer);
        SceneManager.LoadScene(levelToOpenID);
    }
}
