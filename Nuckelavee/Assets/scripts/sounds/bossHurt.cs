using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class bossHurt : MonoBehaviour
{
    [field: SerializeField] public EventReference BHsound { get; private set; }
    private EventInstance _BHInstance;
    // Start is called before the first frame update
    void Start()
    {
        _BHInstance = RuntimeManager.CreateInstance(BHsound);


    }
    public void BHSound()
    {
        _BHInstance.start();
    }

    void OnDestroy()
    {
        _BHInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            BHSound();
        }

    }
}
