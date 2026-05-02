using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class punch : MonoBehaviour
{
    [field: SerializeField] public EventReference punchsound { get; private set; }
    private EventInstance _punchInstance;
    // Start is called before the first frame update
    void Start()
    {
        _punchInstance = RuntimeManager.CreateInstance(punchsound);


    }
    public void PunchSound()
    {
        _punchInstance.start();
    }

    void OnDestroy()
    {
        _punchInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            PunchSound();
        }

    }
}
