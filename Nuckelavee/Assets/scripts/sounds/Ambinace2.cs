using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambinace2 : MonoBehaviour
{

    [SerializeField] private EventReference ambianceLevel2;
    FMOD.Studio.EventInstance ambinaceInstace;

   
    void Start()
    {
        ambinaceInstace = RuntimeManager.CreateInstance(ambianceLevel2);
        ambinaceInstace.start();


    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        // Release memory
        ambinaceInstace.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}

