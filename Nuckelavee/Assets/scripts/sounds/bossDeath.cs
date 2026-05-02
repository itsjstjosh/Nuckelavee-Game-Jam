using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class bossDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference BDsound { get; private set; }
    private EventInstance _BDInstance;
    // Start is called before the first frame update
    void Start()
    {
        _BDInstance = RuntimeManager.CreateInstance(BDsound);


    }
    public void BDSound()
    {
        _BDInstance.start();
    }

    void OnDestroy()
    {
        _BDInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            BDSound();
        }

    }
}
