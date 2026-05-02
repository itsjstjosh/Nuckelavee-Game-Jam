using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class enemenyHurt : MonoBehaviour
{
    [field: SerializeField] public EventReference EHurtsound { get; private set; }
    private EventInstance _EHurtInstance;
    // Start is called before the first frame update
    void Start()
    {
        _EHurtInstance = RuntimeManager.CreateInstance(EHurtsound);


    }
    public void EHurtSound()
    {
        _EHurtInstance.start();
    }

    void OnDestroy()
    {
        _EHurtInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EHurtSound();
        }

    }
}
