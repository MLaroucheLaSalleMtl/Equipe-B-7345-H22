using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public bool dontInterrupt = true;
    public static MusicManager instance = null;
    private Scene currentScene;
    [SerializeField] private GameObject cinematic;

    private AudioSource audioS;
    [SerializeField] private AudioClip menuMusic, tutorialMusic, hubMusic, marcMusic, sebMusic, stevenMusic;

    // Start is called before the first frame update

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
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        audioS = GetComponent<AudioSource>();
        if (dontInterrupt)
            Object.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    void PauseForCinematics()
    {
        if (cinematic.activeSelf)
        {
            audioS.Pause();
        }
        else if (!cinematic.activeSelf)
        {
            audioS.UnPause();
        }
    }

    private void SceneMusic()
    {
        switch (currentScene.buildIndex)
        {
            case 0:
                audioS.PlayOneShot(menuMusic);
                break;
            case 1:
                audioS.PlayOneShot(tutorialMusic);
                break;
            case 2:
                audioS.PlayOneShot(hubMusic);
                break;
            case 3:
                audioS.PlayOneShot(marcMusic);
                break;
            case 4:
                audioS.PlayOneShot(sebMusic);
                break;
            case 5:
                audioS.PlayOneShot(stevenMusic);
                break;
            default:
                break;
        }

    }
}
