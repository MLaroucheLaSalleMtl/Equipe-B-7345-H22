using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioM;
    [SerializeField] private string nameParam;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        float vol = PlayerPrefs.GetFloat(nameParam, 0.3f);
        slider.value = vol;
        //SoundManager.instance.ChangeMasterVolume(slider.value);
        //slider.onValueChanged.AddListener(val => SoundManager.instance.ChangeMasterVolume(val));
    }

    public void SetVol(float vol)
    {
        audioM.SetFloat(nameParam, Mathf.Log10(vol) * 30);
        PlayerPrefs.SetFloat(nameParam, vol);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
