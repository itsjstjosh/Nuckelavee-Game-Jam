using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class enemyDeath : MonoBehaviour
{
    [field: SerializeField] public EventReference EDsound { get; private set; }
    private EventInstance _EDInstance;
    // Start is called before the first frame update
    void Start()
    {
        _EDInstance = RuntimeManager.CreateInstance(EDsound);


    }
    public void EDSound()
    {
        _EDInstance.start();
    }

    void OnDestroy()
    {
        _EDInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EDSound();
        }

    }
}
