using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class playerDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference PDsound { get; private set; }
    private EventInstance _PDInstance;
    // Start is called before the first frame update
    void Start()
    {
        _PDInstance = RuntimeManager.CreateInstance(PDsound);


    }
    public void PDSound()
    {
        _PDInstance.start();
    }

    void OnDestroy()
    {
        _PDInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PDSound();
        }

    }
}
