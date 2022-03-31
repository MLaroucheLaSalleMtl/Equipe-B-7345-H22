using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private bool isMainMenu;
    [SerializeField] private PlayerStats player;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject settingCanvas;
    [SerializeField] private PlayerController control;
    [SerializeField] private Button resume;

    [SerializeField] private Button btnContinue;
    private AudioSource audio;

    private int sceneToLoad;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        if(isMainMenu)
        {
            if (player.LastCheckpoint == Vector3.zero) btnContinue.GetComponent<Button>().interactable = false;
            else btnContinue.GetComponent<Button>().interactable = true;
        }
    }
    public void StartNewGame()
    {
        audio.PlayOneShot(audio.clip);
        SceneManager.LoadScene("TutorialLevel");
        //sceneToLoad = SceneManager.GetSceneByName("TutorialLevel");
        //print(sceneToLoad);
        //SceneManager.LoadScene(sceneToLoad);
    }
    public void ContinueLevel()
    {
        audio.PlayOneShot(audio.clip);
        player.IsContinuing = true;
        sceneToLoad = player.PlayerLevel;
        SceneManager.LoadScene(sceneToLoad);
    }
    public void SwitchMenu()
    {
        audio.PlayOneShot(audio.clip);
        if (mainCanvas.activeInHierarchy)
        {
            mainCanvas.SetActive(false);
            settingCanvas.SetActive(true);
        }
        else if(settingCanvas.activeInHierarchy)
        {
            mainCanvas.SetActive(true);
            settingCanvas.SetActive(false);
        }
    }
    public void Quit()
    {
        audio.PlayOneShot(audio.clip);
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        audio.PlayOneShot(audio.clip);
        SceneManager.LoadScene("MainMenu");
    }
    public void CloseMenu()
    {
        audio.PlayOneShot(audio.clip);
        control.PauseInput = true;
    }
    public void BackToHub()
    {
        audio.PlayOneShot(audio.clip);
        SceneManager.LoadScene("MainMap");
    }
}
