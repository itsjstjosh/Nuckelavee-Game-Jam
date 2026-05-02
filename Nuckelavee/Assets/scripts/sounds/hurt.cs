using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class hurt : MonoBehaviour
{
    [field: SerializeField] public EventReference hurtsound{ get; private set; }
    private EventInstance _hurtInstance;
    // Start is called before the first frame update
    void Start()
    {
        _hurtInstance = RuntimeManager.CreateInstance(hurtsound);


    }
    public void hurtSound()
    {
        _hurtInstance.start(); 
    }

    void OnDestroy()
    { 
        _hurtInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


        // Update is called once per frame
        void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            hurtSound();
        }

    }
}
