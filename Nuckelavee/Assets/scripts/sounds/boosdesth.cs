using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class bossdesth : MonoBehaviour
{
    [field: SerializeField] public EventReference bossDeathsound { get; private set; }
    private EventInstance _bossDeathInstance;
    // Start is called before the first frame update
    void Start()
    {
        _bossDeathInstance = RuntimeManager.CreateInstance(bossDeathsound);


    }
    public void bossDeathSound()
    {
        _bossDeathInstance.start();
    }

    void OnDestroy()
    {
        _bossDeathInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            bossDeathSound();
        }

    }
}
