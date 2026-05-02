using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sowrd : MonoBehaviour
{
    [field: SerializeField] public EventReference swordsound { get; private set; }
    private EventInstance _swordInstance;
    // Start is called before the first frame update
    void Start()
    {
        _swordInstance = RuntimeManager.CreateInstance(swordsound);


    }
    public void swordSound()
    {
        _swordInstance.start();
    }

    void OnDestroy()
    {
        _swordInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            swordSound();
        }

    }
}
    

