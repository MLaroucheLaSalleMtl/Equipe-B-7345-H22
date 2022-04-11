using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class StartGameVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioM;
    private float iniVol = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        audioM.GetFloat("MasterVol", out iniVol);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
